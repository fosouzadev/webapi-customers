using FoSouzaDev.Customers.Domain.DataTransferObjects;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Exceptions;
using FoSouzaDev.Customers.Domain.Repositories;
using FoSouzaDev.Customers.Domain.ValueObjects;

namespace FoSouzaDev.Customers.Domain.Services;

public sealed class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    public async Task<string> AddAsync(AddCustomerDto customer)
    {
        Customer entity = customer;
        await customerRepository.AddAsync(entity);

        return entity.Id;
    }

    public async Task<Customer?> GetByIdAsync(string id) =>
        (await customerRepository.GetByIdAsync(id)) ?? throw new NotFoundException(id);

    public async Task EditAsync(string id, EditCustomerDto customer)
    {
        Customer? entity = await this.GetByIdAsync(id);

        entity!.FullName = new FullName(customer.Name, customer.LastName);
        entity.Notes = customer.Notes;

        await customerRepository.ReplaceAsync(entity);
    }

    public async Task DeleteAsync(string id)
    {
        _ = await this.GetByIdAsync(id);

        await customerRepository.DeleteAsync(id);
    }
}