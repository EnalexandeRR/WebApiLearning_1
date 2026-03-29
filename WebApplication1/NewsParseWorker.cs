using MyWebApp.Interfaces;

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
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    Console.WriteLine($"Start fetching news: {DateTime.Now}");
                    var newClient = scope.ServiceProvider.GetRequiredService<INewsClient>();
                    await newClient.FetchNewsAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}