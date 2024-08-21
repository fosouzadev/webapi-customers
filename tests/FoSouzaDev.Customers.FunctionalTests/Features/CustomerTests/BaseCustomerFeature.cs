using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.Application.Infrastructure.Repositories;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.ValueObjects;
using FoSouzaDev.Customers.Infrastructure.Repositories;
using FoSouzaDev.Customers.WebApi.Responses;
using MongoDB.Driver;
using Xunit.Gherkin.Quick;

namespace FoSouzaDev.Customers.FunctionalTests.Features.CustomerTests;

public abstract class BaseCustomerFeature : BaseFeature
{
    protected const string Route = "api/v1/customer";

    protected ICustomerRepository CustomerRepository { get; private init; }

    protected static DateTime ValidBirthDate => DateTime.Now.AddYears(-18).Date;
    protected static string ValidEmail => "test@test.com";
    protected string? CustomerId { get; set; }

    protected BaseCustomerFeature(MongoDbFixture mongoDbFixture) : base(mongoDbFixture)
    {
        MongoClient mongoClient = new(mongoDbFixture.MongoDbContainer.GetConnectionString());
        CustomerRepository = new CustomerRepository(mongoClient.GetDatabase(base.DefaultConfiguration["MongoDbSettings:DatabaseName"]));

        base.Fixture.Customize<BirthDate>(a => a.FromFactory(() => new BirthDate(ValidBirthDate)));
        base.Fixture.Customize<Email>(a => a.FromFactory(() => new Email(ValidEmail)));
    }

    [Given("I choose a customer id: (.*)")]
    public async Task SetCustomerId(string id)
    {
        if (id != "valid")
        {
            CustomerId = id;
            return;
        }

        Customer existingCustomer = base.Fixture.Create<Customer>();
        existingCustomer.Id = string.Empty;

        await CustomerRepository.AddAsync(existingCustomer);

        CustomerId = existingCustomer.Id;
    }

    [And("The response contains the following value for the ErrorMessage field: (.*)")]
    public async Task ValidateResponseErrorMessage(string errorMessage)
    {
        ResponseData<string>? responseData = await base.GetResponseDataAsync<string>();
        responseData.Should().NotBeNull();

        responseData!.ErrorMessage.Should().Be(errorMessage);
    }

    [And("The response contains the following value for the Data field: (.*)")]
    public async Task ValidateResponseData(string data)
    {
        ResponseData<string>? responseData = await base.GetResponseDataAsync<string>();
        responseData.Should().NotBeNull();

        responseData!.Data.Should().Be(data);
    }
}