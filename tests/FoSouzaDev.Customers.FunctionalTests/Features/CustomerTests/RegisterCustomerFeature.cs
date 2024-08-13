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
    private string? _insertedId;

    [Given("I choose valid random data for a new client")]
    public void GenerateCustomerData()
    {
        _customerDto = Fixture.Build<AddCustomerDto>()
            .With(a => a.BirthDate, ValidBirthDate)
            .With(a => a.Email, ValidEmail)
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
        string jsonContent = await base.HttpResponse!.Content.ReadAsStringAsync();
        ResponseData<string>? responseData = JsonSerializer.Deserialize<ResponseData<string>>(jsonContent, base.JsonSerializerOptions);

        responseData.Should().NotBeNull();
        responseData!.Data.Should().NotBeNull();
        responseData.ErrorMessage.Should().BeNull();

        _insertedId = responseData!.Data!;
    }

    [And("The customer must exist in the database")]
    public async Task ValidateDatabase()
    {
        Customer? customer = await base.CustomerRepository.GetByIdAsync(_insertedId!);
        customer.Should().NotBeNull();

        Customer expectedCustomer = _customerDto!;
        expectedCustomer.Id = _insertedId!;
        customer.Should().BeEquivalentTo(expectedCustomer);
    }
}