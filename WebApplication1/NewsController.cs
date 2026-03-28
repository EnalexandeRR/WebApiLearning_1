using Microsoft.AspNetCore.Mvc;

namespace WebApplication1;

[ApiController]
[Route("[controller]")]
public class NewsController : ControllerBase
{


    [HttpGet]
    public IActionResult GetNewsForPeriod([FromQuery]DateTimeOffset  from,  [FromQuery]DateTimeOffset  to)
    {
        int a = 5;
        a = a / 3;
        Console.WriteLine($"A is equal = {a}");
        var requestedTime = new Times(from, to);
        Console.WriteLine($"TIMES = {requestedTime.StartDate} - {requestedTime.EndDate}");
        return Ok(requestedTime);
    }
    
}

public class Times
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate{ get; set; }
    
    public  Times(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        this.StartDate = startDate;
        this.EndDate = endDate;
    }
}