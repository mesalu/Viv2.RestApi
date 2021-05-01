using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Viv2.API.AppInterface
{ 
    public class Middleware
    {
        readonly ILogger _log;
        readonly RequestDelegate _next;
        public Middleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger("Request-Response");
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            string contentLength = context.Response.ContentLength == null ? "" : " " + context.Response.ContentLength.ToString() + " bytes";
            string contentType = (context.Request.ContentType == null || context.Request.ContentType.CompareTo("") == 0) ? "" : $" ({context.Request.ContentType})";
            _log.LogInformation($"{context.Connection.RemoteIpAddress} {context.Request.Method} {context.Request.Path}{contentType} -> {context.Response.StatusCode}{contentLength}");
        }
    }


    // extends IApplicationBuilder to have a method that adds our middleware component.
    public static class MiddlewareExtender
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Middleware>();
        }
    }
}