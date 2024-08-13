using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Repositories;
using FoSouzaDev.Customers.Domain.ValueObjects;
using FoSouzaDev.Customers.Infrastructure.Repositories;
using MongoDB.Driver;
using System.Net;
using Xunit.Gherkin.Quick;

namespace FoSouzaDev.Customers.FunctionalTests.Features.CustomerTests;

public abstract class BaseCustomerFeature : BaseFeature
{
    protected const string Route = "api/v1/customer";

    protected ICustomerRepository CustomerRepository { get; private init; }

    protected static DateTime ValidBirthDate => DateTime.Now.AddYears(-18).Date;
    protected static string ValidEmail => "test@test.com";
    protected string? ExistingCustomerId { get; private set; }

    protected BaseCustomerFeature(MongoDbFixture mongoDbFixture) : base(mongoDbFixture)
    {
        MongoClient mongoClient = new(mongoDbFixture.MongoDbContainer.GetConnectionString());
        CustomerRepository = new CustomerRepository(mongoClient.GetDatabase(base.DefaultConfiguration["MongoDbSettings:DatabaseName"]));

        base.Fixture.Customize<BirthDate>(a => a.FromFactory(() => new BirthDate(ValidBirthDate)));
        base.Fixture.Customize<Email>(a => a.FromFactory(() => new Email(ValidEmail)));
    }

    [Given("I choose an existing customer id")]
    public async Task GenerateExistingCustomer()
    {
        Customer existingCustomer = base.Fixture.Create<Customer>();
        existingCustomer.Id = string.Empty;

        await CustomerRepository.AddAsync(existingCustomer);

        ExistingCustomerId = existingCustomer.Id;
    }

    [Then(@"The request response must be successful with status code (\d+)")]
    public void ValidateResponse(int httpStatusCode)
    {
        base.HttpResponse!.StatusCode.Should().Be((HttpStatusCode)httpStatusCode);
    }
}