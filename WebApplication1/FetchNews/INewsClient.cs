namespace WebApplication1.Interfaces;

public interface INewsClient
{
    Task FetchNewsAsync(CancellationToken cancellationToken);
}