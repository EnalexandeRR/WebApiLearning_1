namespace WebApplication1.Clients;

public interface INewsClient
{
    Task<string> FetchNewsAsync(CancellationToken cancellationToken);
}