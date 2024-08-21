using FoSouzaDev.Customers.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;

namespace FoSouzaDev.Customers.FunctionalTests;

public sealed class WebApiFactory(IDictionary<string, string?> configuration) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureLogging(builder => builder.AddConsole())
            .UseEnvironment("Testing")
            .UseDefaultServiceProvider((_, options) =>
            {
                options.ValidateScopes = true;
                options.ValidateOnBuild = true;
            })
            .UseTestServer();

        foreach (string key in configuration.Keys)
            builder.UseSetting(key, configuration[key]);
    }
}