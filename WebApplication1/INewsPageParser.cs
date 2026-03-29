namespace MyWebApp;

public interface INewsPageParser
{
    Task<List<NewsItem>> ParseHtmlAsync(string html);
}