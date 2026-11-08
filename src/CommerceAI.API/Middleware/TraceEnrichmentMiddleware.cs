using Serilog.Context;
using System.Diagnostics;

namespace CommerceAI.API.Middleware
{
    public sealed class TraceEnrichmentMiddleware
    {
        private readonly RequestDelegate _next;

        public TraceEnrichmentMiddleware(RequestDelegate next)
        {
            _next=next;
        }

        public async Task Invoke(HttpContext context)
        {
            var activity = Activity.Current;

            var traceId = activity!.TraceId.ToString() ?? context.TraceIdentifier;

            var spanId = activity!.SpanId.ToString();

            using (LogContext.PushProperty("TaceId", traceId))
            using (LogContext.PushProperty("SpanId", spanId))
            using (LogContext.PushProperty("RequestId", context.TraceIdentifier))
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers["X-Trace-Id"] = traceId;
                    context.Response.Headers["X-Request-Id"] = context.TraceIdentifier;

                    return Task.CompletedTask;
                });

                await _next(context);
            }
        }
    }
}
