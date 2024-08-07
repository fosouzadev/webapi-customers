namespace FoSouzaDev.Customers.WebApi.Responses;

public sealed class ResponseData<T>(T? data = default, string? errorMessage = default)
{
    public T? Data { get; private set; } = data;
    public string? ErrorMessage { get; private set; } = errorMessage;
}