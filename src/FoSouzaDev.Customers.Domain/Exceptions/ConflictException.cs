namespace FoSouzaDev.Customers.Domain.Exceptions;

public sealed class ConflictException(string email) : Exception(message: "Already registered.")
{
    public string Email => email;
}