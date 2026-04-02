using Quartz;

namespace MyWebApp;

public class NewsService: INewsService
{
    private readonly INewsRepository _repository;
    private readonly INewsClient _newsClient;
    private readonly INewsPageParser _parser;
    public NewsService(INewsRepository repository, INewsClient newsClient, INewsPageParser parser)
    {
        _repository = repository;
        _newsClient = newsClient;
        _parser = parser;
    }
    
    public async Task FetchAndSaveNewsAsync(IJobExecutionContext context)
    {
        try
        {
            string htmlContent = await _newsClient.FetchNewsAsync(context.CancellationToken);
            
            List<NewsItem> newsItemsList = await _parser.ParseHtmlAsync(htmlContent);
            
            if (newsItemsList.Count > 0)
            {
                await _repository.SaveNewsToDb(newsItemsList);
            }
            
            //TODO: for debug only, delete later!
            /*foreach (var newsItem in newsItemsList)
            {
                Console.WriteLine(newsItem.ToString());
            }*/
            Console.WriteLine("Received new news!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong! Message: {ex.Message}, {ex.StackTrace}");
        }
    }

    public async Task<bool> GetNewsByPeriodAsync()
    {
        Console.WriteLine("GetNewsByPeriodAsync");
        return true;
    }

    public async Task<bool> AddNewsManualAsync()
    {
        Console.WriteLine("AddNewsManualAsync");
        return true;
    }

    public async Task<bool> DeleteNewsAsync()
    {
        Console.WriteLine("DeleteNewsAsync");
        return true;
    }
}