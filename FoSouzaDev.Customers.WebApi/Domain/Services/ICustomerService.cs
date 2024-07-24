using FoSouzaDev.Customers.WebApi.Domain.DataTransferObjects;

namespace FoSouzaDev.Customers.WebApi.Domain.Services;

public interface ICustomerService
{
    Task<string> AddAsync(AddCustomerDto customer);
    Task<CustomerDto> GetByIdAsync(string id);
    Task EditAsync(string id, EditCustomerDto customer);
    Task DeleteAsync(string id);
}