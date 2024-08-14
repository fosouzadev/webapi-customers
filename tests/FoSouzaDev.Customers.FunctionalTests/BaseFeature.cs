using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.WebApi.Responses;
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

    [Then(@"The http response should be (\d+)")]
    public void ValidateResponse(int httpStatusCode)
    {
        HttpResponse!.StatusCode.Should().Be((HttpStatusCode)httpStatusCode);
    }

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

    protected void StartApplication()
    {
        HttpClient = new WebApiFactory(DefaultConfiguration).CreateClient();
    }

    internal async Task<ResponseData<T>?> GetResponseDataAsync<T>()
    {
        string jsonContent = await HttpResponse!.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ResponseData<T>?>(jsonContent, JsonSerializerOptions);
    }

    public void Dispose()
    {
        HttpClient!.Dispose();
        HttpResponse!.Dispose();
    }
}