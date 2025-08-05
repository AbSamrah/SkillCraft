using System.Net;
using System.Text.Json;

namespace SkillCraft.Api.Middleware;

public class ApiErrorResponse
{
    public string Message { get; set; }
    public string? Details { get; set; } 
}

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _env = env;
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound, 
                ArgumentException => (int)HttpStatusCode.BadRequest,   
                _ => (int)HttpStatusCode.InternalServerError          
            };

            var message = "An internal server error has occurred."; 
            if (ex is KeyNotFoundException || ex is ArgumentException)
            {
                message = ex.Message;
            }

            var response = _env.IsDevelopment()
                ? new ApiErrorResponse { Message = ex.Message, Details = ex.StackTrace?.ToString() }
                : new ApiErrorResponse { Message = message };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}