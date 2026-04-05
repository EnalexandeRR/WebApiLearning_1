using Quartz;

namespace MyWebApp;

public interface INewsService
{
    Task FetchAndSaveNewsAsync(IJobExecutionContext context);
    Task<IEnumerable<NewsItem>> GetNewsByPeriodAsync(DateTimeOffset from,DateTimeOffset to);
    Task<bool> AddNewsManualAsync();
    Task<bool> DeleteNewsAsync();
}