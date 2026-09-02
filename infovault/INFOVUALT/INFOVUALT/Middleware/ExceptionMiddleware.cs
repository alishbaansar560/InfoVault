using System.Text.Json;

namespace INFOVUALT.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = _env.IsDevelopment()
                    ? new
                    {
                        StatusCode = 500,
                        Message = "Something went wrong.",
                        Error = ex.Message,
                        StackTrace = ex.StackTrace
                    }
                    : new
                    {
                        StatusCode = 500,
                        Message = "Something went wrong.",
                        Error = (string?)null,
                        StackTrace = (string?)null
                    };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}