namespace MyWebApp.Models;

public class AddNewsRequest
{
    public string Title { get; set; }
    public DateTimeOffset ReleaseDate { get; set; }
    public int ViewCount { get; set; }
}