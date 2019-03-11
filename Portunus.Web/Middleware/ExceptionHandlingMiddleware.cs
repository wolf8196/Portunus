using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Portunus.SSO.Exceptions;

namespace Portunus.Web.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (AppNotRegisteredException ex)
            {
                LogError(ex);
                SetResponse(context, StatusCodes.Status404NotFound);
            }
            catch (ArgumentException ex)
            {
                LogError(ex);
                SetResponse(context, StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                LogError(ex);
                SetResponse(context, StatusCodes.Status500InternalServerError);
            }
        }

        private void SetResponse(HttpContext context, int statusCode)
        {
            context.Response.Clear();
            context.Response.StatusCode = statusCode;
        }

        private void LogError(Exception ex)
        {
            logger.LogError(ex, "Portunus threw an exception");
        }
    }
}