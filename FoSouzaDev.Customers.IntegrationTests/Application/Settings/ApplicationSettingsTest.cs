using FluentAssertions;
using FoSouzaDev.Customers.Application.Settings;
using FoSouzaDev.Customers.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoSouzaDev.Customers.IntegrationTests.Application.Settings;

public sealed class ApplicationSettingsTest(MongoDbFixture mongoDbFixture) : IClassFixture<MongoDbFixture>
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
    }
}