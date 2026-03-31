using MyWebApp.Interfaces;
using Quartz;

namespace MyWebApp;

public class BackgroundGetNewsJob : IJob
{
    private readonly INewsClient _newsClient;
    private readonly INewsPageParser _parser;
    private readonly INewsRepository _repository;

    public BackgroundGetNewsJob(INewsClient newsClient, INewsPageParser parser, INewsRepository newsRepository)
    {
        _newsClient = newsClient;
        _parser = parser;
        _repository = newsRepository;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            string htmlContent = await _newsClient.FetchNewsAsync(context.CancellationToken);
            List<NewsItem> newsItemsList = await _parser.ParseHtmlAsync(htmlContent);
            await _repository.SaveNewsToDb(newsItemsList);
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