using FoSouzaDev.Customers.Application.Services;
using FoSouzaDev.Customers.Domain.Repositories;
using FoSouzaDev.Customers.Domain.Services;
using FoSouzaDev.Customers.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FoSouzaDev.Customers.Application.Settings
{
    public static class ApplicationSettings
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));

            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<ICustomerService, CustomerService>();
            services.AddSingleton<ICustomerApplicationService, CustomerApplicationService>();

            services.AddSingleton<IMongoDatabase>(provider =>
            {
                MongoDbSettings mongoDbSettings = provider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(mongoDbSettings.ConnectionURI).GetDatabase(mongoDbSettings.DatabaseName);
            });
        }
    }
}