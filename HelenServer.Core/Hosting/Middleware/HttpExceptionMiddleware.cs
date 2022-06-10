namespace HelenServer.Core;

internal class HttpExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public HttpExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/problem+json";

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await JsonSerializer.SerializeAsync(
                context.Response.Body,
                OperationResult.Error(ex),
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}