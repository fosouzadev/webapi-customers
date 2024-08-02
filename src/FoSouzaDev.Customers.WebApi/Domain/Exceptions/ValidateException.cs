namespace FoSouzaDev.Customers.WebApi.Domain.Exceptions;

public sealed class ValidateException(string message) : Exception(message)
{
}