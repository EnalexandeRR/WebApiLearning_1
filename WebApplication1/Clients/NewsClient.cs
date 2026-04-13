using Microsoft.Extensions.Options;
using WebApplication1.Configuration;

namespace WebApplication1.Clients;

public class NewsClient : INewsClient
{
    private readonly HttpClient _httpClient;

    public NewsClient(HttpClient? httpClient, IOptions<NewsClientOptions> clientOptions)
    {
        _httpClient = httpClient;
        var options = clientOptions.Value;
        _httpClient.BaseAddress = new Uri(options.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
    }
    
    public async Task<string> FetchNewsAsync(CancellationToken stoppingToken)
    {
        var response = await _httpClient.GetAsync("/ru/news",  stoppingToken);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync(stoppingToken);
        }
        return string.Empty;
    }
}