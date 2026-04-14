using WebApplication1.Models;
using WebApplication1.Repositories;
using Quartz;
using WebApplication1.Clients;
using WebApplication1.Helpers;

namespace WebApplication1.Services;

public class NewsService : INewsService
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

            newsItemsList.Sort((a, b) => a.ReleaseTime.CompareTo(b.ReleaseTime));
            await _repository.SaveNewsToDbAsync(newsItemsList);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong while parsing and saving news! Message: {ex.Message}, {ex.StackTrace}");
        }
    }

    public async Task<IEnumerable<NewsItem>> GetNewsByPeriodAsync(GetNewsRequest request)
    {
        if (request.From == null && request.To == null) throw new ValidationException("No data period specified!");
        if(request.From > request.To) throw new ValidationException("Start date must be less than end date!");
        
        return await _repository.GetNewsAsync(request);
    }

    public async Task<bool> AddNewsManualAsync(AddNewsRequest request)
    {
        if (string.IsNullOrEmpty(request.Title)) throw new ValidationException("No news title specified!");
        
        return await _repository.AddNewsToDbAsync(request);
    }

    public async Task<bool> DeleteNewsByIdAsync(DeleteByIdRequest request)
    {
        if(request.Id == null) throw new ValidationException("No news id specified!");
        
        return await _repository.DeleteNewsByIdAsync(request);
    }

    public async Task<int> DeleteNewsByPeriodAsync(DeleteByPeriodRequest request)
    {
        if (request.From == null && request.To == null) throw new ValidationException("No data period specified!");
        if(request.From > request.To) throw new ValidationException("Start date must be less than end date!");
        
        return await _repository.DeleteNewsByPeriodAsync(request);
    }

}