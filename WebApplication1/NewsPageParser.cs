using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace MyWebApp;

public class NewsPageParser: INewsPageParser
{
    public async Task<List<NewsItem>> ParseHtmlAsync(string html)
    {
        var newsList = new List<NewsItem>();
        var parser = new HtmlParser();
        var document = await parser.ParseDocumentAsync(html);
        
        var newsBoxes = document.QuerySelectorAll(".iq-blog-box");
        
        foreach (var newsBox in newsBoxes)
        {
            var  title = newsBox.QuerySelector(".blog-heading");
            var dateElement = newsBox.QuerySelector(".iq-blog-meta li");
            var viewCount = newsBox.QuerySelectorAll(".iq-blog-meta li")[1];

            newsList.Add(new NewsItem
            {
                Id = 1,
                Title = title.TextContent.Trim(),
                ReleaseTime = dateElement.TextContent.Trim(),
                ViewCount = int.TryParse(viewCount.TextContent.Trim(), out var count) ? count : 0
            });
        }
        return newsList;
    }
}