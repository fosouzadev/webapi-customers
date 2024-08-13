namespace FoSouzaDev.Customers.WebApi.Responses;

internal sealed class ResponseData<T>
{
    public ResponseData(T? data = default, string? errorMessage = default)
    {
        if (data == null && errorMessage == null)
            throw new ArgumentNullException(message: "Invalid data.", null);

        this.Data = data;
        this.ErrorMessage = errorMessage;
    }

    public T? Data { get; private init; }
    public string? ErrorMessage { get; private init; }
}