using Quartz;

namespace MyWebApp;

public class BackgroundGetNewsJob : IJob
{
    private readonly INewsClient _newsClient;
    private readonly INewsPageParser _parser;
    private readonly INewsRepository _repository;
    private readonly INewsService _newsService;

    public BackgroundGetNewsJob(INewsService  newsService)
    {
        _newsService = newsService;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await _newsService.FetchAndSaveNewsAsync(context);
    }
}