using FoSouzaDev.Customers.Application.Services;
using FoSouzaDev.Customers.Domain.Repositories;
using FoSouzaDev.Customers.Infrastructure.Repositories;
using FoSouzaDev.Customers.Infrastructure.Repositories.Settings;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FoSouzaDev.Customers.WebApi.Settings;

public static class ApiSettings
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));

        services.AddSingleton<ICustomerRepository, CustomerRepository>();
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

    public static void AddApiHealthChecks(this IHealthChecksBuilder healthChecksBuilder)
    {
        healthChecksBuilder.AddMongoDb(provider => provider.GetRequiredService<IMongoClient>(), "MongoDB", HealthStatus.Unhealthy);
    }
}