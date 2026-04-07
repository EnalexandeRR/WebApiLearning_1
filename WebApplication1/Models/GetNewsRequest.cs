namespace WebApplication1.Models;

public class GetNewsRequest
{
    public DateTimeOffset from { get; set; }
    public DateTimeOffset to { get; set; }
}