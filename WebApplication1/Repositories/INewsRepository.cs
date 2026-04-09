using WebApplication1.Models;

namespace WebApplication1.Repositories;
public interface INewsRepository
{
    Task<bool> SaveNewsToDbAsync(IEnumerable<NewsItem> newsItem);
    Task<IEnumerable<NewsItem>> GetNewsAsync(GetNewsRequest request);
    Task<bool> AddNewsToDbAsync(AddNewsRequest request);
    Task<bool> DeleteNewsByIdAsync(DeleteByIdRequest request);
    Task<DateTimeOffset?> GetLastAddedTime();
}