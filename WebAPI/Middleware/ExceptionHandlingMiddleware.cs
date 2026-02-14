using System.Net;
using System.Text.Json;
using Application.Common.Exceptions;
using Domain.Exceptions;

namespace WebAPI.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await Handle(context, e);
        }
    }

    private static Task Handle(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ConflictException => HttpStatusCode.Conflict,
            ForbiddenException => HttpStatusCode.Forbidden,
            NotFoundException => HttpStatusCode.NotFound,
            UnauthorizedException => HttpStatusCode.Unauthorized,
            BusinessRuleException => HttpStatusCode.BadRequest,
            OperationFailedException => HttpStatusCode.InternalServerError,
            _ => HttpStatusCode.InternalServerError
        };

        var result = JsonSerializer.Serialize(new
        {
            error = exception.Message,
            type = exception.GetType().Name
        });
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        
        return context.Response.WriteAsync(result);
    }
}