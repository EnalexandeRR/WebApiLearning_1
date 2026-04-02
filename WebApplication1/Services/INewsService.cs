using Quartz;

namespace MyWebApp;

public interface INewsService
{
    Task FetchAndSaveNewsAsync(IJobExecutionContext context);
    Task<bool> GetNewsByPeriodAsync();
    Task<bool> AddNewsManualAsync();
    Task<bool> DeleteNewsAsync();
}