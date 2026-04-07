using MyWebApp.Models;

namespace MyWebApp;
public interface INewsRepository
{
    Task SaveNewsToDbAsync(IEnumerable<NewsItem> newsItem);
    Task<IEnumerable<NewsItem>> GetNewsAsync(GetNewsRequest request);
    Task<bool> AddNewsToDbAsync(AddNewsRequest request);
}