namespace WebApplication1.Models;

public class DeleteByPeriodRequest
{
    public DateTimeOffset from { get; set; }
    public DateTimeOffset to { get; set; }
}