namespace WebApplication1.Models;

public class GetNewsResponse : BaseResponse
{
    public IEnumerable<NewsItem> NewsItemsCollection { get; set; }
}