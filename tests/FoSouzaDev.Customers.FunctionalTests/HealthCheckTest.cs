using FluentAssertions;
using FoSouzaDev.Customers.CommonTests;
using System.Net;

namespace FoSouzaDev.Customers.FunctionalTests;

[Collection("MongoDbFixture")]
public class HealthCheckTest
{
    private readonly HttpClient _httpClient;

    public HealthCheckTest(MongoDbFixture mongoDbFixture)
    {
        WebApiFactory webApiFactory = new(new Dictionary<string, string?>
        {
            { "MongoDbSettings:ConnectionURI", mongoDbFixture.MongoDbContainer.GetConnectionString() }
        });
        this._httpClient = webApiFactory.CreateClient();
    }

    [Fact]
    public async Task TestGet()
    {
        var response = await this._httpClient.GetAsync("/api/health-check");
        string data = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data.Should().Be("Healthy");
    }
}