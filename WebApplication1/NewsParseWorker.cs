namespace MyWebApp;

public class NewsParseWorker: BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(3000, stoppingToken);
            Console.WriteLine("Logging some data every 3 seconds!");
        }
    }
}