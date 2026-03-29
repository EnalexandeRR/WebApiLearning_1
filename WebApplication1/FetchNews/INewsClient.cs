namespace MyWebApp.Interfaces;

public interface INewsClient
{
    Task<string> FetchNewsAsync(CancellationToken cancellationToken);
}