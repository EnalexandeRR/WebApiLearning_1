using MyWebApp.Models;
using Quartz;

namespace MyWebApp;

public interface INewsService
{
    Task FetchAndSaveNewsAsync(IJobExecutionContext context);
    Task<IEnumerable<NewsItem>> GetNewsByPeriodAsync(GetNewsRequest request);
    Task<bool> AddNewsManualAsync(AddNewsRequest request);
    Task<bool> DeleteNewsAsync();
}