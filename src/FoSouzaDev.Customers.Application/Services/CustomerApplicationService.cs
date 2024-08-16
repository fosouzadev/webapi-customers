using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.Application.Infrastructure.Repositories;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Exceptions;
using FoSouzaDev.Customers.Domain.ValueObjects;

namespace FoSouzaDev.Customers.Application.Services;

internal sealed class CustomerApplicationService(ICustomerRepository customerRepository) : ICustomerApplicationService
{
    public async Task<string> AddAsync(AddCustomerDto customer)
    {
        Customer entity = (Customer)customer;
        await customerRepository.AddAsync(entity);

        return entity.Id;
    }

    public async Task<CustomerDto> GetByIdAsync(string id)
    {
        Customer entity = await GetByIdOrThrowAsync(id);
        return (CustomerDto)entity;
    }

    public async Task EditAsync(string id, EditCustomerDto customer)
    {
        Customer entity = await GetByIdOrThrowAsync(id);

        entity.FullName = new FullName(customer.Name, customer.LastName);
        entity.Notes = customer.Notes;

        await customerRepository.ReplaceAsync(entity);
    }

    public async Task DeleteAsync(string id)
    {
        _ = await GetByIdOrThrowAsync(id);

        await customerRepository.DeleteAsync(id);
    }

    private async Task<Customer> GetByIdOrThrowAsync(string id) =>
        (await customerRepository.GetByIdAsync(id)) ?? throw new NotFoundException(id);
}