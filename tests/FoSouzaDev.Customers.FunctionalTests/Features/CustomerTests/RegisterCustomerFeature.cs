using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.WebApi.Responses;
using System.Text;
using System.Text.Json;
using Xunit.Gherkin.Quick;

namespace FoSouzaDev.Customers.FunctionalTests.Features.CustomerTests;

[FeatureFile("./Features/CustomerTests/RegisterCustomer.feature")]
public sealed class RegisterCustomerFeature(MongoDbFixture mongoDbFixture) : BaseCustomerFeature(mongoDbFixture)
{
    private AddCustomerDto? _customerDto;

    [Given("I choose valid random data for a new client")]
    public void GenerateValidCustomerData()
    {
        _customerDto = Fixture.Build<AddCustomerDto>()
            .With(a => a.BirthDate, ValidBirthDate)
            .With(a => a.Email, ValidEmail)
            .Create();
    }

    [Given("I choose the data for a new customer with an invalid (.*)")]
    public void GenerateInvalidCustomerData(string invalidData)
    {
        _customerDto = Fixture.Build<AddCustomerDto>()
            .With(a => a.Name, invalidData == "name" ? string.Empty : base.Fixture.Create<string>())
            .With(a => a.LastName, invalidData == "last name" ? string.Empty : base.Fixture.Create<string>())
            .With(a => a.BirthDate, invalidData == "date of birth" ? DateTime.Now.Date : ValidBirthDate)
            .With(a => a.Email, invalidData == "email" ? string.Empty : ValidEmail)
            .Create();
    }

    [When("I send a registration request")]
    public async Task SendRegistrationRequest()
    {
        StartApplication();

        using StringContent jsonContent = new(JsonSerializer.Serialize(_customerDto), Encoding.UTF8, "application/json");
        base.HttpResponse = await base.HttpClient!.PostAsync(Route, jsonContent);
    }

    [And("The response contais the inserted id")]
    public async Task ValidateResponseData()
    {
        ResponseData<string>? responseData = await base.GetResponseDataAsync<string>();

        responseData.Should().NotBeNull();
        responseData!.Data.Should().NotBeNull();
        responseData.ErrorMessage.Should().BeNull();

        base.CustomerId = responseData!.Data;
    }

    [And("The customer must exist in the database")]
    public async Task ValidateDatabase()
    {
        Customer? customer = await base.CustomerRepository.GetByIdAsync(base.CustomerId!);
        customer.Should().NotBeNull();

        Customer expectedCustomer = _customerDto!;
        expectedCustomer.Id = base.CustomerId!;
        customer.Should().BeEquivalentTo(expectedCustomer);
    }
}