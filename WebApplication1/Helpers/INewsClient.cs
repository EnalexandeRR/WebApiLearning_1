namespace MyWebApp;

public interface INewsClient
{
    Task<string> FetchNewsAsync(CancellationToken cancellationToken);
}