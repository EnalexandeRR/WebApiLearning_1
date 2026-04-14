using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Models;

namespace WebApplication1.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    
    
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ArgumentException exception)
        {
            context.Result = new OkObjectResult(new BaseResponse
            {
                StatusCode = -999,
                Message = exception.Message
            });
        }
    }
}