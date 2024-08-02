namespace FoSouzaDev.Customers.Domain.Exceptions;

public sealed class ValidateException(string message) : Exception(message)
{
}