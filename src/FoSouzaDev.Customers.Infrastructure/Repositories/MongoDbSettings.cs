namespace FoSouzaDev.Customers.Infrastructure.Repositories;

internal sealed class MongoDbSettings
{
    public string? ConnectionURI { get; init; }
    public string? DatabaseName { get; init; }
}