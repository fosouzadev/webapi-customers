namespace FoSouzaDev.Customers.WebApi.Infrastructure.Repositories
{
    public sealed class MongoDbSettings
    {
        public string? ConnectionURI { get; init; }
        public string? DatabaseName { get; init; }
    }
}