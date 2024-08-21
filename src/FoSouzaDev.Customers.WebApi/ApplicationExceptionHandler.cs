using FoSouzaDev.Customers.Domain.Exceptions;
using FoSouzaDev.Customers.WebApi.Responses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Newtonsoft.Json;

namespace FoSouzaDev.Customers.WebApi;

public sealed class ApplicationExceptionHandler(ILogger<ApplicationExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        object response;

        switch (exception)
        {
            case ValidateException:
            case JsonPatchException:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                response = new ResponseData<string>(errorMessage: exception.Message);
                break;
            case NotFoundException ex:
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                response = new ResponseData<string>(data: ex.Id, errorMessage: exception.Message);
                break;
            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response = new ResponseData<string>(errorMessage: "Internal server error.");
                break;
        }

        logger.LogError(exception, message: "Response: {Response} - Exception message: {ExceptionMessage}", JsonConvert.SerializeObject(response), exception.Message);

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}