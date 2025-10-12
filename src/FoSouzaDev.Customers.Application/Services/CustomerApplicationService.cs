using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.Application.Factories;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Exceptions;
using FoSouzaDev.Customers.Domain.Repositories;
using FoSouzaDev.Customers.Domain.ValueObjects;
using Microsoft.AspNetCore.JsonPatch;

namespace FoSouzaDev.Customers.Application.Services;

internal sealed class CustomerApplicationService(
    ICustomerRepository repository,
    ICustomerFactory factory) : ICustomerApplicationService
{
    private async Task<Customer> GetByIdOrThrowAsync(string id) =>
        (await repository.GetByIdAsync(id)) ?? throw new NotFoundException(id);

    public async Task<string> AddAsync(AddCustomerDto customer)
    {
        Customer entity = factory.AddCustomerDtoToCustomer(customer);
        await repository.AddAsync(entity);

        return entity.Id;
    }

    public async Task<CustomerDto> GetByIdAsync(string id)
    {
        Customer entity = await GetByIdOrThrowAsync(id);
        return factory.CustomerToCustomerDto(entity);
    }

    public async Task EditAsync(string id, JsonPatchDocument<EditCustomerDto> pathDocument)
    {
        Customer entity = await GetByIdOrThrowAsync(id);

        EditCustomerDto editCustomer = factory.CustomerToEditCustomerDto(entity);
        pathDocument.ApplyTo(editCustomer);

        entity.FullName = new FullName(editCustomer.Name, editCustomer.LastName);
        entity.Notes = editCustomer.Notes;

        await repository.ReplaceAsync(entity);
    }

    public async Task DeleteAsync(string id)
    {
        _ = await GetByIdOrThrowAsync(id);

        await repository.DeleteAsync(id);
    }
}