using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.CommonTests;
using System.Net;
using System.Text.Json;
using Xunit.Gherkin.Quick;

namespace FoSouzaDev.Customers.FunctionalTests;

[Collection("MongoDbFixture")]
public abstract class BaseFeature : Feature, IDisposable
{
    protected Fixture Fixture { get; private init; }
    protected JsonSerializerOptions JsonSerializerOptions { get; private init; }

    protected IDictionary<string, string?> DefaultConfiguration { get; private init; }
    protected HttpClient? HttpClient { get; private set; }
    protected HttpResponseMessage? HttpResponse { get; set; }

    protected BaseFeature(MongoDbFixture mongoDbFixture)
    {
        Fixture = new();
        JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        DefaultConfiguration = new Dictionary<string, string?>
        {
            { "Logging:LogLevel:Default", "Information" },
            { "Logging:LogLevel:Microsoft.AspNetCore", "Critical" },
            { "Logging:LogLevel:Microsoft", "Critical" },
            { "Logging:LogLevel:Microsoft.Hosting.Lifetime", "Critical" },
            { "AllowedHosts", "*" },
            { "MongoDbSettings:ConnectionURI", mongoDbFixture.MongoDbContainer.GetConnectionString() },
            { "MongoDbSettings:DatabaseName", "Test" },
        };
    }

    [Then(@"The request response must be successful with status code (\d+)")]
    public void ValidateResponse(int httpStatusCode)
    {
        HttpResponse!.StatusCode.Should().Be((HttpStatusCode)httpStatusCode);
    }

    protected void StartApplication()
    {
        HttpClient = new WebApiFactory(DefaultConfiguration).CreateClient();
    }

    public void Dispose()
    {
        HttpClient?.Dispose();
        HttpResponse?.Dispose();
    }
}