using Quartz;
using WebApplication1.Clients;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class NewsService: INewsService
{
    private readonly INewsRepository _repository;
    private readonly INewsClient _newsClient;
    
    public NewsService(INewsRepository repository, INewsClient newsClient)
    {
        _repository = repository;
        _newsClient = newsClient;
    }
    
    public async Task FetchAndSaveNewsAsync(IJobExecutionContext context)
    {
        try
        {
            async Task<List<NewsItem>> ParseNews()
            {
                string htmlContent = await _newsClient.FetchNewsAsync(context.CancellationToken);
                return await NewsPageParser.ParseHtmlAsync(htmlContent);
            }
            Task<List<NewsItem>> parseNewsTask = ParseNews();
            Task<DateTimeOffset?> getLastAddedTime = _repository.GetLastAddedTime();
            await Task.WhenAll(parseNewsTask, getLastAddedTime);
            
            List<NewsItem> newsItemsList = parseNewsTask.Result;
            if (newsItemsList.Count == 0) return;
            
            if (getLastAddedTime.Result != null)
            {
                newsItemsList.RemoveAll((article) => article.ReleaseTime <= getLastAddedTime.Result);
            }
            foreach (var newsItem in newsItemsList)
            {
                newsItem.IsAutoAdded = true;
            }
            newsItemsList.Sort((a,b)=> a.ReleaseTime.CompareTo(b.ReleaseTime) );
            await _repository.SaveNewsToDbAsync(newsItemsList);
            Console.WriteLine("Received new news!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong! Message: {ex.Message}, {ex.StackTrace}");
        }
    }

    public async Task<IEnumerable<NewsItem>> GetNewsByPeriodAsync(GetNewsRequest request)
    {
        return await _repository.GetNewsAsync(request);
    }

    public async Task<bool> AddNewsManualAsync(AddNewsRequest request)
    {
        return await _repository.AddNewsToDbAsync(request);
    }

    public async Task<bool> DeleteNewsByIdAsync(DeleteByIdRequest request)
    {
        return await _repository.DeleteNewsByIdAsync(request);
    }
}