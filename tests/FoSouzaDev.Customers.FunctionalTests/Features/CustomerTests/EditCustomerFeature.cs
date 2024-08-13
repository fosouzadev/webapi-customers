using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using System.Text;
using System.Text.Json;
using Xunit.Gherkin.Quick;

namespace FoSouzaDev.Customers.FunctionalTests.Features.CustomerTests;

[FeatureFile("./Features/CustomerTests/EditCustomer.feature")]
public sealed class EditCustomerFeature(MongoDbFixture mongoDbFixture) : BaseCustomerFeature(mongoDbFixture)
{
    private EditCustomerDto? _customerDto;

    [And("I choose random valid values ​​for editing")]
    public void EditCustomerData()
    {
        _customerDto = base.Fixture.Create<EditCustomerDto>();
    }

    [When("I send the edit request")]
    public async Task SendEditRequest()
    {
        StartApplication();

        using StringContent jsonContent = new(JsonSerializer.Serialize(_customerDto), Encoding.UTF8, "application/json");
        base.HttpResponse = await base.HttpClient!.PatchAsync($"{Route}/{base.ExistingCustomerId}", jsonContent);
    }

    [And("The customer must be edited in the database")]
    public async Task ValidateDatabase()
    {
        Customer? customer = await base.CustomerRepository.GetByIdAsync(base.ExistingCustomerId!);
        customer.Should().NotBeNull();

        customer!.FullName.Name.Should().Be(_customerDto!.Name);
        customer.FullName.LastName.Should().Be(_customerDto.LastName);
        customer.Notes.Should().Be(_customerDto.Notes);
    }
}