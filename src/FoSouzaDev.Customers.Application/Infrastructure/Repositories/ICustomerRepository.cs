using FoSouzaDev.Customers.Domain.Entities;

namespace FoSouzaDev.Customers.Application.Infrastructure.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer?> GetByIdAsync(string id);
    Task ReplaceAsync(Customer customer);
    Task DeleteAsync(string id);
}