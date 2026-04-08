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
    
    public async Task<bool> FetchAndSaveNewsAsync(IJobExecutionContext context)
    {
        try
        {
            string htmlContent = await _newsClient.FetchNewsAsync(context.CancellationToken);
            
            List<NewsItem> newsItemsList = await NewsPageParser.ParseHtmlAsync(htmlContent);
            
            if (newsItemsList.Count > 0)
            {
                newsItemsList.Sort((a,b)=> a.ReleaseTime.CompareTo(b.ReleaseTime) );
                var isSaved  = await _repository.SaveNewsToDbAsync(newsItemsList);
                Console.WriteLine("Received new news!");
                return isSaved;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong! Message: {ex.Message}, {ex.StackTrace}");
            return false;
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