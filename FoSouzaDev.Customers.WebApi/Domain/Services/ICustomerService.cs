using FoSouzaDev.Customers.WebApi.Domain.DataTransferObjects;
using FoSouzaDev.Customers.WebApi.Domain.Entities;

namespace FoSouzaDev.Customers.WebApi.Domain.Services;

public interface ICustomerService
{
    Task<string> AddAsync(AddCustomerDto customer);
    Task<Customer?> GetByIdAsync(string id);
    Task EditAsync(string id, EditCustomerDto customer);
    Task DeleteAsync(string id);
}