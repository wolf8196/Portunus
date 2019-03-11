using Microsoft.AspNetCore.Builder;
using Portunus.Web.Middleware;

namespace Portunus.Web.Bootstrap
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UsePortunusExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}