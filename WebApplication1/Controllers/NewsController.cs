using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

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

    [HttpDelete("delete/id")]
    public async Task<IActionResult> DeleteNewsByIdAsync([FromBody] DeleteByIdRequest request)
    {
        var isDeleted = await _newsService.DeleteNewsByIdAsync(request);
        if(isDeleted) return Ok(new BaseResponse{StatusCode = 0, Message = $"News id: {request.Id} deleted successfully!"});
        return Ok(new BaseResponse{StatusCode = -999, Message = $"Something went wrong while deleting the news id: {request.Id}!"});
    }
}