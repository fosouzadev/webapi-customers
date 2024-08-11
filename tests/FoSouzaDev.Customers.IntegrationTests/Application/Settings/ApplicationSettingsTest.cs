using FluentAssertions;
using FoSouzaDev.Customers.Application.Services;
using FoSouzaDev.Customers.Application.Settings;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Repositories;
using FoSouzaDev.Customers.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace FoSouzaDev.Customers.IntegrationTests.Application.Settings;

[Collection("MongoDbFixture")]
public sealed class ApplicationSettingsTest(MongoDbFixture mongoDbFixture)
{
    [Fact]
    public void AddApplicationServices_Success_DependenciesConfigured()
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new List<KeyValuePair<string, string?>>
            {
                new("MongoDbSettings:ConnectionURI", mongoDbFixture.MongoDbContainer.GetConnectionString()),
                new("MongoDbSettings:DatabaseName", "testDb")
            })
            .Build();

        IServiceCollection services = new ServiceCollection();
        services.AddApplicationServices(configuration);

        // Act
        ServiceProvider serviceProvider = services.BuildServiceProvider();

        // Assert
        serviceProvider.GetService<ICustomerRepository>().Should().NotBeNull();
        serviceProvider.GetService<ICustomerService>().Should().NotBeNull();
        serviceProvider.GetService<ICustomerApplicationService>().Should().NotBeNull();

        serviceProvider.GetService<IMongoDatabase>().Should().NotBeNull();
    }
}