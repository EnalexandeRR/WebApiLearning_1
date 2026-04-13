namespace WebApplication1.Models;

public class DeleteByPeriodRequest
{
    public DateTimeOffset? From { get; set; }
    public DateTimeOffset? To { get; set; }
}