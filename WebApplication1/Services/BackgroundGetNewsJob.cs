using Quartz;

namespace WebApplication1.Services;

public class BackgroundGetNewsJob : IJob
{
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