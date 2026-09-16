using NOVA.API.Common.Mapping;
using NOVA.Domain.Common.Results;
namespace NOVA.API.Middlewares
{
    public sealed class GlobalExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
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
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
        {
            var traceId = context.TraceIdentifier;

            _logger.LogError(
                exception,
                "Unhandled exception. TraceId: {TraceId}",
                traceId);

            var error = Error.Unexpected();

            var mapped = ErrorResultMapper.Map(
                [error],
                traceId);

            context.Response.StatusCode = mapped.StatusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(mapped.Body);
        }

    }
}
