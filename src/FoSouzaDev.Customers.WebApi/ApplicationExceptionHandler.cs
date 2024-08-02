using FoSouzaDev.Customers.WebApi.Domain.Exceptions;
using FoSouzaDev.Customers.WebApi.Responses;
using Microsoft.AspNetCore.Diagnostics;

namespace FoSouzaDev.Customers.WebApi
{
    public sealed class ApplicationExceptionHandler(ILogger<ApplicationExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            object response;

            switch (exception)
            {
                case ValidateException:
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = new ResponseData<string>(errorMessage: exception.Message);
                    break;
                case NotFoundException ex:
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    response = new ResponseData<string>(data: ex.Id);
                    break;
                default:
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response = new ResponseData<string>(errorMessage: "Internal server error.");
                    break;
            }

            logger.LogError(exception, message: exception.Message, response);

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}