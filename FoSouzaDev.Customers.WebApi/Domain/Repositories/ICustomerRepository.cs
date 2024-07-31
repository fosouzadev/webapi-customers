using FoSouzaDev.Customers.WebApi.Domain.Entities;

namespace FoSouzaDev.Customers.WebApi.Domain.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer?> GetByIdAsync(string id);
    Task ReplaceAsync(Customer customer);
    Task DeleteAsync(string id);
}