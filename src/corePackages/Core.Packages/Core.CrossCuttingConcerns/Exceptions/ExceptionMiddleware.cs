using Core.CrossCuttingConcerns.Exceptions.Handlers;
using Microsoft.AspNetCore.Http;

namespace Core.CrossCuttingConcerns.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpExceptionHandler _exceptionHandler;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
        _exceptionHandler = new();
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context.Response, ex);
              
        }
    }
    private Task HandleExceptionAsync(HttpResponse response,Exception ex)
    {
        response.ContentType = "application/json";
        _exceptionHandler.Response = response;
        return _exceptionHandler.HandlerExceptionAsync(ex);
    }
}
