using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Services;
using FoSouzaDev.Customers.Domain.ValueObjects;

namespace FoSouzaDev.Customers.Application.Services
{
    internal sealed class CustomerApplicationService(ICustomerService customerService) : ICustomerApplicationService
    {
        public async Task<string> AddAsync(AddCustomerDto customer)
        {
            Customer entity = customer;
            await customerService.AddAsync(entity);

            return entity.Id;
        }

        public async Task<CustomerDto?> GetByIdAsync(string id)
        {
            Customer? entity = await customerService.GetByIdAsync(id);
            return (CustomerDto?)entity!;
        }

        public async Task EditAsync(string id, EditCustomerDto customer) =>
            await customerService.EditAsync(id, new FullName(customer.Name, customer.LastName), customer.Notes);

        public async Task DeleteAsync(string id) =>
            await customerService.DeleteAsync(id);
    }
}