using Microsoft.Data.Sqlite;
using WebApplication1.Models;

namespace WebApplication1.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next  = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case ValidationException:
                {
                    httpContext.Response.StatusCode = StatusCodes.Status200OK;

                    var response = new BaseResponse()
                    {
                        StatusCode = -1,
                        Message = ex.Message
                    };

                    await httpContext.Response.WriteAsJsonAsync(response);
                } 
                    break;
                case SqliteException:
                {
                    httpContext.Response.StatusCode = StatusCodes.Status200OK;

                    var response = new BaseResponse()
                    {
                        StatusCode = -888,
                        Message = "Something is wrong with database!!!"
                    };

                    await httpContext.Response.WriteAsJsonAsync(response);
                } 
                    break;
                default:
                {
                    throw;
                }
            }
            
        }
    }
    
    
}