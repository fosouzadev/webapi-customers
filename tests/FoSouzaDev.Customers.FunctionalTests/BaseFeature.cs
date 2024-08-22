using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.WebApi.Responses;
using Newtonsoft.Json;
using System.Net;
using Xunit.Gherkin.Quick;

namespace FoSouzaDev.Customers.FunctionalTests;

[Collection("MongoDbFixture")]
public abstract class BaseFeature : Feature, IDisposable
{
    protected Fixture Fixture { get; private init; }

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

        DefaultConfiguration = new Dictionary<string, string?>
        {
            { "Logging:LogLevel:Default", "Information" },
            { "Logging:LogLevel:Microsoft.AspNetCore", "Critical" },
            { "Logging:LogLevel:Microsoft", "Critical" },
            { "Logging:LogLevel:Microsoft.Hosting.Lifetime", "Critical" },
            { "AllowedHosts", "*" },
            { "MongoDbSettings:ConnectionURI", mongoDbFixture.ConnectionString },
            { "MongoDbSettings:DatabaseName", mongoDbFixture.DatabaseName },
        };
    }

    protected void StartApplication()
    {
        HttpClient = new WebApiFactory(DefaultConfiguration).CreateClient();
    }

    internal async Task<ResponseData<T>?> GetResponseDataAsync<T>()
    {
        string jsonContent = await HttpResponse!.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ResponseData<T>?>(jsonContent);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        HttpClient!.Dispose();
        HttpResponse!.Dispose();
    }
}