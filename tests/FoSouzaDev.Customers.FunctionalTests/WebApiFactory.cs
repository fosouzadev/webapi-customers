using FoSouzaDev.Customers.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FoSouzaDev.Customers.FunctionalTests;

internal sealed class WebApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.UseSetting("MongoDbSettings:ConnectionURI", "");
    }
}