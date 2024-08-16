using FoSouzaDev.Customers.Application.DataTransferObjects;
using Microsoft.AspNetCore.JsonPatch;

namespace FoSouzaDev.Customers.Application.Services;

public interface ICustomerApplicationService
{
    Task<string> AddAsync(AddCustomerDto customer);
    Task<CustomerDto> GetByIdAsync(string id);
    Task EditAsync(string id, JsonPatchDocument<EditCustomerDto> pathDocument);
    Task DeleteAsync(string id);
}