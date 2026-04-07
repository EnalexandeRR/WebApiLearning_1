namespace MyWebApp.Models;

public class AddNewsRequest
{
    public string Title { get; set; }
    public DateTimeOffset ReleaseTime{ get; set; }
    public int ViewCount { get; set; }
}