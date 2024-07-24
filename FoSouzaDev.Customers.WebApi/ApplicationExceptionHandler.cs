using FoSouzaDev.Customers.WebApi.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace FoSouzaDev.Customers.WebApi
{
    public sealed class ApplicationExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            string errorMessage = exception.Message;

            switch (exception)
            {
                case ValidateException:
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                case NotFoundException:
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;
                default:
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    errorMessage = "Internal server error.";
                    break;
            }

            await httpContext.Response.WriteAsJsonAsync(errorMessage, cancellationToken);

            return true;
        }
    }
}
