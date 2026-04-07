namespace MyWebApp.Models;

public class GetNewsResponse : BaseResponse
{
    public IEnumerable<NewsItem> NewsItemsCollection { get; set; }
}