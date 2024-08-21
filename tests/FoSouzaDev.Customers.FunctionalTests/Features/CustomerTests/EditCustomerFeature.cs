using FluentAssertions;
using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using Gherkin.Ast;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System.Text;
using System.Text.Json;
using Xunit.Gherkin.Quick;

namespace FoSouzaDev.Customers.FunctionalTests.Features.CustomerTests;

[FeatureFile("./Features/CustomerTests/EditCustomer.feature")]
public sealed class EditCustomerFeature(MongoDbFixture mongoDbFixture) : BaseCustomerFeature(mongoDbFixture)
{
    private JsonPatchDocument<EditCustomerDto> _pathDocument = new();

    [And("I choose the following data to edit:")]
    public void EditCustomerData(DataTable operations)
    {
        foreach (TableRow? row in operations.Rows.Skip(1))
        {
            string operationType = row.Cells.ElementAt(0).Value;
            string fieldName = row.Cells.ElementAt(1).Value;
            string? value = row.Cells.ElementAtOrDefault(2)?.Value;

            _pathDocument.Operations.Add(new Operation<EditCustomerDto>
            {
                op = operationType,
                path = $"/{fieldName}",
                value = value
            });
        }
    }

    [When("I send the edit request")]
    public async Task SendEditRequest()
    {
        StartApplication();

        using StringContent jsonContent = new(JsonSerializer.Serialize(_pathDocument.Operations), Encoding.UTF8, "application/json");
        base.HttpResponse = await base.HttpClient!.PatchAsync($"{Route}/{base.CustomerId}", jsonContent);
    }

    [And("The customer must be edited in the database")]
    public async Task ValidateDatabase()
    {
        Customer? customer = await base.CustomerRepository.GetByIdAsync(base.CustomerId!);
        customer.Should().NotBeNull();

        customer!.FullName.Name.Should().Be(_pathDocument!.Operations.First(a => a.path == "/name").value.ToString());
        customer.FullName.LastName.Should().Be(_pathDocument!.Operations.First(a => a.path == "/lastName").value.ToString());
        customer.Notes.Should().Be(_pathDocument!.Operations.First(a => a.path == "/notes").value.ToString());
    }
}