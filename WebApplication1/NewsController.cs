using Microsoft.AspNetCore.Mvc;

namespace MyWebApp;

[ApiController]
[Route("[controller]")]
public class NewsController : ControllerBase
{


    [HttpGet]
    public IActionResult GetNewsForPeriod([FromQuery] DateTimeOffset from, [FromQuery] DateTimeOffset to)
    {
        return BadRequest();
    }

    [HttpPost]
    public IActionResult AddNews([FromBody] NewsItem  newsItem)
    {
        return BadRequest();
    }

    [HttpPost]
    public IActionResult DeleteNews()
    {
        return BadRequest();
    }
}

public class NewsItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTimeOffset releaseTime { get; set; }
    public int ViewCount { get; set; }
}