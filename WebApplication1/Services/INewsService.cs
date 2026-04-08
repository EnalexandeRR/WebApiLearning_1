using Quartz;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface INewsService
{
    Task<bool> FetchAndSaveNewsAsync(IJobExecutionContext context);
    Task<IEnumerable<NewsItem>> GetNewsByPeriodAsync(GetNewsRequest request);
    Task<bool> AddNewsManualAsync(AddNewsRequest request);
    Task<bool> DeleteNewsByIdAsync(DeleteByIdRequest request);
}