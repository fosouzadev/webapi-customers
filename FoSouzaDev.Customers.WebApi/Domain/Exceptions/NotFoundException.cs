namespace FoSouzaDev.Customers.WebApi.Domain.Exceptions
{
    public sealed class NotFoundException(string message, string id) : Exception(message)
    {
        public string Id => id;
    }
}