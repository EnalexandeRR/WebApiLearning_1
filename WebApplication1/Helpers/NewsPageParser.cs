using System.Globalization;
using AngleSharp.Html.Parser;

namespace MyWebApp;

public static class NewsPageParser
{
    public static async Task<List<NewsItem>> ParseHtmlAsync(string html)
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
                ReleaseTime = ParseDate(dateElement.TextContent.Trim()),
                ViewCount = int.TryParse(viewCount.TextContent.Trim(), out var count) ? count : 0
            });
        }
        
        return newsList;
    }

    private static DateTimeOffset ParseDate(string dateString)
    {
        var dateFormat = "dd.MM.yyyy, HH:mm";

        bool isParseOk = DateTime.TryParseExact(
            dateString,
            dateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime parsedLocalTime);
        
        if (!isParseOk) throw new FormatException($"Не удалось распарсить дату с сайта: {dateString}");
        
        TimeSpan kazakhstanOffset = TimeSpan.FromHours(5);
        DateTimeOffset kzTime = new DateTimeOffset(parsedLocalTime, kazakhstanOffset);
        return kzTime.ToUniversalTime();
    }
}