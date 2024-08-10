using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace FoSouzaDev.Customers.FunctionalTests;

public sealed class WebApiFactory(MongoDbFixture mongoDbFixture) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseEnvironment("Testing")
            .UseSetting("MongoDbSettings:ConnectionURI", mongoDbFixture.MongoDbContainer.GetConnectionString())
            .UseDefaultServiceProvider((_, options) =>
            {
                options.ValidateScopes = true;
                options.ValidateOnBuild = true;
            })
            .UseTestServer();
    }
}