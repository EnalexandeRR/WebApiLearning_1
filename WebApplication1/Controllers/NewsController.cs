using Microsoft.AspNetCore.Mvc;

namespace MyWebApp;

[ApiController]
[Route("[controller]")]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNewsForPeriod([FromQuery] DateTimeOffset from, [FromQuery] DateTimeOffset to)
    {
        var isOk = await _newsService.GetNewsByPeriodAsync();
        if(isOk) return Ok();
        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> AddNews([FromBody] NewsItem  newsItem)
    {
        var isOk = await _newsService.AddNewsManualAsync();
        if(isOk) return Ok();
        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteNews()
    {
        var isOk = await _newsService.DeleteNewsAsync();
        if(isOk) return Ok();
        return BadRequest();
    }
}