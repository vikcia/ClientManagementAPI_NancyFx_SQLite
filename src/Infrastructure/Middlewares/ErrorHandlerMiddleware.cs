using Domain.CustomException;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;
using System.Text.Json;

namespace Infrastructure.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case ConfigException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case NotFoundException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case ItemNotFoundException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case BadRequestException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error
                    _logger.Error(error, error.Message);
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}