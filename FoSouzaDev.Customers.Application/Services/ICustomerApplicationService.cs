using FoSouzaDev.Customers.Application.DataTransferObjects;

namespace FoSouzaDev.Customers.Application.Services;

public interface ICustomerApplicationService
{
    Task<string> AddAsync(AddCustomerDto customer);
    Task<CustomerDto?> GetByIdAsync(string id);
    Task EditAsync(string id, EditCustomerDto customer);
    Task DeleteAsync(string id);
}