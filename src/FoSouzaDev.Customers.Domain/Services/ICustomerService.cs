using FoSouzaDev.Customers.Domain.DataTransferObjects;
using FoSouzaDev.Customers.Domain.Entities;

namespace FoSouzaDev.Customers.Domain.Services;

public interface ICustomerService
{
    Task<string> AddAsync(AddCustomerDto customer);
    Task<Customer?> GetByIdAsync(string id);
    Task EditAsync(string id, EditCustomerDto customer);
    Task DeleteAsync(string id);
}