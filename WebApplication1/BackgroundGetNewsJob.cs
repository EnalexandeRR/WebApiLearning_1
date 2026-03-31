using MyWebApp.Interfaces;
using Quartz;

namespace MyWebApp;

public class BackgroundGetNewsJob : IJob
{
    private readonly INewsClient _newsClient;
    private readonly INewsPageParser _parser;

    public BackgroundGetNewsJob(INewsClient newsClient, INewsPageParser parser)
    {
        _newsClient = newsClient;
        _parser = parser;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            string htmlContent = await _newsClient.FetchNewsAsync(context.CancellationToken);
            List<NewsItem> newsItemsList = await _parser.ParseHtmlAsync(htmlContent);
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
}