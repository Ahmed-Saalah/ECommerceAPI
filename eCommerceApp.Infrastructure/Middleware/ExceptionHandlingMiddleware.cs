using eCommerceApp.Application.Services.Interfaces.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerceApp.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DbUpdateException ex)
            {
                var logger = context.RequestServices
                    .GetRequiredService<IAppLogger<ExceptionHandlingMiddleware>>();
                
                context.Response.ContentType = "application/json";
                if (ex.InnerException is SqlException innerException)
                {
                    logger.LogError(innerException, "SQL exception");
                    switch (innerException.Number)
                    {
                        case 2627:
                            context.Response.StatusCode = StatusCodes.Status409Conflict;
                            await context.Response.WriteAsync("Unique constraint violation");
                            break;

                        case 515:
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync("Cannot insert null");
                            break;

                        case 547:
                            context.Response.StatusCode = StatusCodes.Status409Conflict;
                            await context.Response.WriteAsync("Foreign key constraint violation");
                            break;
                        
                        default:
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response.WriteAsync("An error occured while processing your request");
                            break;
                    }
                }
                else
                {
                    logger.LogError(ex, "Related EfCore exception");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("An error occured while savin the entity changes");
                }
            }
            catch (Exception ex)
            {
                var logger = context.RequestServices
                     .GetRequiredService<IAppLogger<ExceptionHandlingMiddleware>>();

                logger.LogError(ex, "Unknwon exception");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An error occured:" + ex.Message);
            }
        }
    }
}
