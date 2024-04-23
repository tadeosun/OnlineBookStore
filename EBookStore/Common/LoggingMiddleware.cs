using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace EBookStore.Common
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Start timing and log the request
            var watch = Stopwatch.StartNew();
            try
            {
                await _next(context);  // Continue the pipeline execution
            }
            catch (Exception ex)
            {
                watch.Stop();
                _logger.LogError(ex, "An error occurred processing {Method} {Path} after {ElapsedMilliseconds}ms",
                    context.Request.Method, context.Request.Path, watch.ElapsedMilliseconds);
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var errorResponse = new
                    {
                        error = true,
                        status = HttpStatusCode.InternalServerError,
                        message = "An unexpected error occurred. Please try again later."
                    };
                    var jsonResponse = JsonSerializer.Serialize(errorResponse);

                    await context.Response.WriteAsync(jsonResponse);

                    return;

                }
                return;
            }
            finally
            {
                watch.Stop();
                // Log response details and response status
                _logger.LogInformation("Response {Method} {Path} completed with {StatusCode} in {ElapsedMilliseconds}ms. Response length: {ContentLength}",
                    context.Request.Method, context.Request.Path, context.Response.StatusCode, watch.ElapsedMilliseconds, context.Response.ContentLength);
            }
        }
    }
}
