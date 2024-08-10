using FoSouzaDev.Customers.Application.Services;
using FoSouzaDev.Customers.Domain.Repositories;
using FoSouzaDev.Customers.Domain.Services;
using FoSouzaDev.Customers.Infrastructure.Repositories;
using FoSouzaDev.Customers.Infrastructure.Repositories.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FoSouzaDev.Customers.Application.Settings;

public static class ApplicationSettings
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));

        services.AddSingleton<ICustomerRepository, CustomerRepository>();
        services.AddSingleton<ICustomerService, CustomerService>();
        services.AddSingleton<ICustomerApplicationService, CustomerApplicationService>();

        services.AddSingleton<IMongoClient>(provider =>
        {
            MongoDbSettings mongoDbSettings = provider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(mongoDbSettings.ConnectionURI);
        });

        services.AddSingleton<IMongoDatabase>(provider =>
        {
            MongoDbSettings mongoDbSettings = provider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            IMongoClient mongoClient = provider.GetRequiredService<IMongoClient>();

            return mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
        });
    }

    public static void AddApplicationHealthChecks(this IHealthChecksBuilder healthChecksBuilder)
    {
        healthChecksBuilder.AddMongoDb(provider => provider.GetRequiredService<IMongoClient>(), "MongoDB", HealthStatus.Unhealthy);
    }
}