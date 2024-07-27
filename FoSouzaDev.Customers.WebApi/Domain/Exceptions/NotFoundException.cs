namespace FoSouzaDev.Customers.WebApi.Domain.Exceptions
{
    public sealed class NotFoundException(string id) : Exception
    {
        public string Id => id;
    }
}