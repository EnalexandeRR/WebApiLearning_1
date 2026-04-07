using MyWebApp.Models;
using Quartz;


namespace MyWebApp;

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
            string htmlContent = await _newsClient.FetchNewsAsync(context.CancellationToken);
            
            List<NewsItem> newsItemsList = await NewsPageParser.ParseHtmlAsync(htmlContent);
            
            if (newsItemsList.Count > 0)
            {
                newsItemsList.Sort((a,b)=> a.ReleaseTime.CompareTo(b.ReleaseTime) );
                await _repository.SaveNewsToDbAsync(newsItemsList);
            }
            Console.WriteLine("Received new news!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong! Message: {ex.Message}, {ex.StackTrace}");
        }
    }

    public async Task<IEnumerable<NewsItem>> GetNewsByPeriodAsync(GetNewsRequest request)
    {
        var news = await _repository.GetNewsAsync(request);
        foreach (var newsItem in news)
        {
            Console.WriteLine($"GetNewsByPeriodAsync result {newsItem}");
        }
        return news;
    }

    public async Task<bool> AddNewsManualAsync(AddNewsRequest request)
    {
        return await _repository.AddNewsToDbAsync(request);
    }

    public async Task<bool> DeleteNewsAsync()
    {
        Console.WriteLine("DeleteNewsAsync");
        return true;
    }
}