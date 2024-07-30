namespace FoSouzaDev.Customers.WebApi.Domain.Exceptions
{
    public sealed class NotFoundException(string id) : Exception(message: "Not found.")
    {
        public string Id => id;
    }
}