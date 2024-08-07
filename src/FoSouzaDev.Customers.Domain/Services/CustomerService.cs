using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Exceptions;
using FoSouzaDev.Customers.Domain.Repositories;
using FoSouzaDev.Customers.Domain.ValueObjects;

namespace FoSouzaDev.Customers.Domain.Services;

internal sealed class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    public async Task AddAsync(Customer customer) =>
        await customerRepository.AddAsync(customer);

    public async Task<Customer> GetByIdAsync(string id) =>
        (await customerRepository.GetByIdAsync(id)) ?? throw new NotFoundException(id);

    public async Task EditAsync(string id, FullName fullName, string? notes)
    {
        Customer? entity = await this.GetByIdAsync(id);

        entity!.FullName = fullName;
        entity.Notes = notes;

        await customerRepository.ReplaceAsync(entity);
    }

    public async Task DeleteAsync(string id)
    {
        _ = await this.GetByIdAsync(id);

        await customerRepository.DeleteAsync(id);
    }
}