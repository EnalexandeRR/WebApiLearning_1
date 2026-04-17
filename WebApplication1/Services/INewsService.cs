using WebApplication1.Models;
using Quartz;

namespace WebApplication1.Services;

public interface INewsService
{
    Task FetchAndSaveNewsAsync(IJobExecutionContext context);
    Task<IEnumerable<GetNewsResponseDTO>> GetNewsByPeriodAsync(GetNewsRequest request);
    Task<bool> AddNewsManualAsync(AddNewsRequest request);
    Task<bool> DeleteNewsByIdAsync(DeleteByIdRequest request);
    Task<int> DeleteNewsByPeriodAsync(DeleteByPeriodRequest request);
}