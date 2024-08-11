using FoSouzaDev.Customers.CommonTests;

namespace FoSouzaDev.Customers.IntegrationTests;

[CollectionDefinition("MongoDbFixture")]
public class MongoDbCollectionFixture : ICollectionFixture<MongoDbFixture>
{
}