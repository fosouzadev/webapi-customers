using FluentAssertions;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Repositories;
using FoSouzaDev.Customers.Domain.ValueObjects;
using FoSouzaDev.Customers.Infrastructure.Repositories;
using MongoDB.Driver;
using System.Net;
using Xunit.Gherkin.Quick;

namespace FoSouzaDev.Customers.FunctionalTests.Features
{
    [FeatureFile("./Features/Customer.feature")]
    public abstract class BaseCustomerFeature : BaseFeature
    {
        protected const string Route = "api/v1/customer";

        protected ICustomerRepository CustomerRepository { get; private init; }

        protected static DateTime ValidBirthDate => DateTime.Now.AddYears(-18).Date;
        protected static string ValidEmail => "test@test.com";

        protected BaseCustomerFeature(MongoDbFixture mongoDbFixture) : base(mongoDbFixture)
        {
            MongoClient mongoClient = new(mongoDbFixture.MongoDbContainer.GetConnectionString());
            this.CustomerRepository = new CustomerRepository(mongoClient.GetDatabase(base.DefaultConfiguration["MongoDbSettings:DatabaseName"]));

            this.Fixture.Customize<BirthDate>(a => a.FromFactory(() => new BirthDate(ValidBirthDate)));
            this.Fixture.Customize<Email>(a => a.FromFactory(() => new Email(ValidEmail)));
        }

        [Then(@"The request response must be successful with status code (\d+)")]
        public void ValidateResponse(int httpStatusCode)
        {
            base.HttpResponse!.StatusCode.Should().Be((HttpStatusCode)httpStatusCode);
        }
    }
}