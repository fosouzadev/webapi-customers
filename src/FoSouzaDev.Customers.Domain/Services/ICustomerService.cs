using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.ValueObjects;

namespace FoSouzaDev.Customers.Domain.Services;

public interface ICustomerService
{
    Task AddAsync(Customer customer);
    Task<Customer> GetByIdAsync(string id);
    Task EditAsync(string id, FullName fullName, string? notes);
    Task DeleteAsync(string id);
}