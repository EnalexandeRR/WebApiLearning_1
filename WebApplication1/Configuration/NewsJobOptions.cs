namespace WebApplication1.Configuration;

public class NewsJobOptions
{
    public string JobName { get; set; } 
    public int StartDelayInSeconds { get; set; }
    public int IntervalInSeconds { get; set; }
}