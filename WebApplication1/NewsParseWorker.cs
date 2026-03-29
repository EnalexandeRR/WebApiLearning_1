using MyWebApp.Interfaces;
using System.Text.Json;

namespace MyWebApp;

public class NewsParseWorker: BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public NewsParseWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    Console.WriteLine($"Start fetching news: {DateTime.Now}");
                    var newClient = scope.ServiceProvider.GetRequiredService<INewsClient>();
                    var parser = scope.ServiceProvider.GetRequiredService<INewsPageParser>();
                    
                    string htmlContent = await newClient.FetchNewsAsync(stoppingToken);
                    List<NewsItem> newsItemsList = await parser.ParseHtmlAsync(htmlContent);
                    //TODO: for debug only, delete later!
                    foreach (var newsItem in newsItemsList)
                    {
                       Console.WriteLine(newsItem.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Got exception while executing command: {ex.Message}");
            }
        }
    }
}