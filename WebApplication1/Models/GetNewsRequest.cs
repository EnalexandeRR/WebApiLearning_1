namespace WebApplication1.Models;

public class GetNewsRequest
{
    public DateTimeOffset? From { get; set; }
    public DateTimeOffset? To { get; set; }
}