using AutoFixture;
using FoSouzaDev.Customers.CommonTests;
using Xunit.Gherkin.Quick;

namespace FoSouzaDev.Customers.FunctionalTests
{
    [Collection("MongoDbFixture")]
    public abstract class BaseFeature : Feature, IDisposable
    {
        protected IDictionary<string, string?> DefaultConfiguration { get; private init; }
        protected Fixture Fixture { get; private init; }
        protected HttpClient? HttpClient { get; private set; }
        protected HttpResponseMessage? HttpResponse { get; set; }

        protected BaseFeature(MongoDbFixture mongoDbFixture)
        {
            this.Fixture = new();
            this.DefaultConfiguration = new Dictionary<string, string?>
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
            this.HttpClient = new WebApiFactory(this.DefaultConfiguration).CreateClient();
        }

        public void Dispose()
        {
            this.HttpClient?.Dispose();
            this.HttpResponse?.Dispose();
        }
    }
}