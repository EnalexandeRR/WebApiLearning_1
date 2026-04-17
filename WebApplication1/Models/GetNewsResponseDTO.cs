namespace WebApplication1.Models;

public class GetNewsResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTimeOffset ReleaseTime { get; set; }
    public int ViewCount { get; set; }
}