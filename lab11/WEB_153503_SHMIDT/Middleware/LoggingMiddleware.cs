using Serilog.Core;

namespace WEB_153503_SHMIDT.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Logger _logger;

        public LoggingMiddleware(RequestDelegate next, Logger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
            var statusCode = context.Response.StatusCode;
            if (statusCode < 200 || statusCode >= 300)
            {
                _logger.Information($"---> request {context.Request.Path + context.Request.QueryString.ToUriComponent()} returns {statusCode}");
            }
        }
    }
}
