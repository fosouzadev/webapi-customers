using FluentAssertions;
using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.WebApi.Responses;
using System.Text.Json;
using Xunit.Gherkin.Quick;

namespace FoSouzaDev.Customers.FunctionalTests.Features.CustomerTests;

[FeatureFile("./Features/CustomerTests/QueryCustomer.feature")]
public sealed class QueryCustomerFeature(MongoDbFixture mongoDbFixture) : BaseCustomerFeature(mongoDbFixture)
{
    [When("I send a query request")]
    public async Task SendQueryRequest()
    {
        StartApplication();

        base.HttpResponse = await base.HttpClient!.GetAsync($"{Route}/{base.ExistingCustomerId}");
    }

    [And("The response contains the requested customer data")]
    public async Task ValidateResponseData()
    {
        Customer? customer = await base.CustomerRepository.GetByIdAsync(base.ExistingCustomerId!);
        customer.Should().NotBeNull();

        string jsonContent = await base.HttpResponse!.Content.ReadAsStringAsync();
        ResponseData<CustomerDto>? responseData = JsonSerializer.Deserialize<ResponseData<CustomerDto>>(jsonContent, base.JsonSerializerOptions);

        responseData.Should().NotBeNull();
        responseData!.Data.Should().NotBeNull();
        responseData.ErrorMessage.Should().BeNull();

        responseData.Data.Should().BeEquivalentTo((CustomerDto)customer!);
    }
}