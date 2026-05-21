using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class PostgreNewsRepository: INewsRepository
{
    public Task<bool> SaveNewsToDbAsync(IEnumerable<NewsItem> newsItem)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GetNewsResponseDTO>> GetNewsAsync(GetNewsRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddNewsToDbAsync(AddNewsRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteNewsByIdAsync(DeleteByIdRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteNewsByPeriodAsync(DeleteByPeriodRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<DateTimeOffset?> GetLastAddedTime()
    {
        throw new NotImplementedException();
    }
}