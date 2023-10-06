using Core.CrossCuttingConcerns.Exceptions.Extensions;
using Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using ValidationProblemDetails = Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails.ValidationProblemDetails;

namespace Core.CrossCuttingConcerns.Exceptions.Handlers;

public class HttpExceptionHandler : ExceptionHandle
{
    private HttpResponse? _httpResponse;
    public HttpResponse Response
    {
        get => _httpResponse?? throw new ArgumentNullException(nameof(_httpResponse));
        set => _httpResponse = value;
    }
    protected override Task HandlerException(BusinessException businessException)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        string details = new BusinessProblemDetails(businessException.Message).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandlerException(Exception exception)
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError;
        string details = new InternalServerErrorProblemDetails(exception.Message).AsJson();
        return Response.WriteAsync(details);    
    }

    protected override Task HandlerException(ValidationException validationException)
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError;
        string details = new ValidationProblemDetails(validationException.Errors).AsJson();
        return Response.WriteAsync(details);
    }
}
