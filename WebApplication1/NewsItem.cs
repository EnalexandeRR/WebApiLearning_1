namespace MyWebApp;

public class NewsItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTimeOffset ReleaseTime { get; set; }
    public int ViewCount { get; set; }
    
    public override string ToString()
    {
        return $"Новость: ID:{Id} Заголовок: {Title} Дата публикации:{ReleaseTime} Просмотров: {ViewCount}";
    }
}