namespace MyWebApp.Interfaces;

public class NewsClient : INewsClient
{
    private readonly HttpClient _httpClient;

    public NewsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task FetchNewsAsync(CancellationToken stoppingToken)
    {
        var response = await _httpClient.GetAsync("/ru/news",  stoppingToken);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("NEWS  RESPONSE COMPLETED!");
        }
    }
}