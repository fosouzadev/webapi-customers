using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.Application.Factories;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Repositories;
using FoSouzaDev.Customers.Domain.ValueObjects;
using FoSouzaDev.Customers.Infrastructure.Repositories;
using FoSouzaDev.Customers.Infrastructure.Repositories.Factories;
using FoSouzaDev.Customers.WebApi.Responses;
using Microsoft.Extensions.Logging;
using Xunit.Gherkin.Quick;

namespace FoSouzaDev.Customers.FunctionalTests.Features.CustomerTests;

public abstract class BaseCustomerFeature : BaseFeature
{
    protected const string Route = "api/v1/customer";

    protected ICustomerRepository Repository { get; private init; }
    protected ICustomerFactory ApplicationFactory { get; private init; }
    protected string CustomerId { get; set; }

    protected BaseCustomerFeature(MongoDbFixture mongoDbFixture) : base(mongoDbFixture)
    {
        Repository = new CustomerRepository(
            mongoDbFixture.MongoDatabase,
            new LoggerFactory().CreateLogger<CustomerRepository>(),
            new CustomerEntityFactory());

        ApplicationFactory = new CustomerFactory();

        Fixture.Customize<BirthDate>(a => a.FromFactory(() => new BirthDate(ValidDataGenerator.ValidBirthDate)));
        Fixture.Customize<Email>(a => a.FromFactory(() => new Email(ValidDataGenerator.ValidEmail)));
    }

    [Given("I choose a customer with id: (.*)")]
    public async Task SetCustomerId(string id)
    {
        if (id != "valid")
        {
            CustomerId = id;
            return;
        }

        Customer existingCustomer = Fixture.Create<Customer>();
        existingCustomer.Id = string.Empty;

        await Repository.AddAsync(existingCustomer);

        CustomerId = existingCustomer.Id;
    }

    [And("The response contains the following value for the ErrorMessage field: (.*)")]
    public async Task ValidateResponseErrorMessage(string errorMessage)
    {
        ResponseData<string> responseData = await GetResponseDataAsync<string>();
        responseData.Should().NotBeNull();

        responseData.ErrorMessage.Should().Be(errorMessage);
    }

    [And("The response contains the following value for the Data field: (.*)")]
    public async Task ValidateResponseData(string data)
    {
        ResponseData<string> responseData = await GetResponseDataAsync<string>();
        responseData.Should().NotBeNull();

        responseData.Data.Should().Be(data);
    }
}