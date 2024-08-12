namespace FoSouzaDev.Customers.WebApi.Responses;

internal sealed class ResponseData<T>(T? data = default, string? errorMessage = default)
{
    public T? Data { get; init; } = data;
    public string? ErrorMessage { get; init; } = errorMessage;
}