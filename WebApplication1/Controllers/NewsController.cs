using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;

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

    [HttpGet("getbyperiod")]
    public async Task<IActionResult> GetNewsForPeriodAsync([FromQuery] GetNewsRequest request)
    {
        var newsResultList = await _newsService.GetNewsByPeriodAsync(request);
        if(newsResultList.Any()) return Ok(new GetNewsResponse{StatusCode = 0, Message = "", NewsItemsCollection = newsResultList});
        return Ok(new BaseResponse{StatusCode = 0, Message = "No news found for period!"});
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddNewsAsync([FromBody] AddNewsRequest request)
    {
        var isAdded = await _newsService.AddNewsManualAsync(request);
        if(isAdded) return Ok(new BaseResponse{StatusCode = 0, Message = "News article added!"});
        return Ok(new BaseResponse{StatusCode = -999, Message = "Failed to add new article!"});
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteNewsAsync()
    {
        var isOk = await _newsService.DeleteNewsAsync();
        if(isOk) return Ok();
        return BadRequest();
    }
}