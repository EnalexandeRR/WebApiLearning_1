namespace MyWebApp.Interfaces;

public interface INewsClient
{
    Task FetchNewsAsync(CancellationToken cancellationToken);
}