using FluentAssertions;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace FoSouzaDev.Customers.CommonTests;

public sealed class MongoDbFixture : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithImage("mongo:latest")
        .Build();

    public string DatabaseName => "test";
    public string? ConnectionString { get; private set; }
    public IMongoDatabase? MongoDatabase { get; private set; }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
        
        ConnectionString = _mongoDbContainer.GetConnectionString();
        MongoDatabase = new MongoClient(ConnectionString).GetDatabase(DatabaseName);

        (await _mongoDbContainer.ExecScriptAsync(@"db.customers.createIndex( { ""email"": 1 }, { unique: true } );"))
            .Stdout.Should().Contain("true");
    }

    public Task DisposeAsync() => _mongoDbContainer.DisposeAsync().AsTask();
}