using FluentAssertions;
using FoSouzaDev.Customers.CommonTests;
using Xunit.Gherkin.Quick;

namespace FoSouzaDev.Customers.FunctionalTests.Features;

[FeatureFile("./Features/HealthCheck.feature")]
public sealed class HealthCheckFeature(MongoDbFixture mongoDbFixture) : BaseFeature(mongoDbFixture)
{
    [Given("All dependencies are ok")]
    public void DependenciesOk()
    {
        base.StartApplication();
    }

    [When("I send the request")]
    public async Task SendRequest()
    {
        base.HttpResponse = await base.HttpClient!.GetAsync("api/health-check");
    }

    [And("The response data must be (.*)")]
    public async Task ValidateResponseData(string expectedData)
    {
        string data = await base.HttpResponse!.Content.ReadAsStringAsync();

        data.Should().Be(expectedData);
    }
}