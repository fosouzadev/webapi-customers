using FluentAssertions;
using FoSouzaDev.Customers.Application.Infrastructure.Repositories;
using FoSouzaDev.Customers.Application.Services;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Infrastructure.Repositories.Settings;
using FoSouzaDev.Customers.WebApi.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FoSouzaDev.Customers.IntegrationTests.WebApi.Settings;

[Collection("MongoDbFixture")]
public sealed class ApiSettingsTest(MongoDbFixture mongoDbFixture)
{
    [Fact]
    public void AddApplicationServices_Success_DependenciesConfigured()
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new List<KeyValuePair<string, string?>>
            {
                new("MongoDbSettings:ConnectionURI", mongoDbFixture.ConnectionString),
                new("MongoDbSettings:DatabaseName", mongoDbFixture.DatabaseName)
            })
            .Build();

        IServiceCollection services = new ServiceCollection();
        services.AddLogging(a => a.AddConsole());

        services.AddApiServices(configuration);

        // Act
        ServiceProvider serviceProvider = services.BuildServiceProvider();

        // Assert
        serviceProvider.GetService<ICustomerRepository>().Should().NotBeNull();
        serviceProvider.GetService<ICustomerApplicationService>().Should().NotBeNull();

        serviceProvider.GetService<IOptions<MongoDbSettings>>().Should().NotBeNull();
        serviceProvider.GetService<IMongoClient>().Should().NotBeNull();
        serviceProvider.GetService<IMongoDatabase>().Should().NotBeNull();
    }
}