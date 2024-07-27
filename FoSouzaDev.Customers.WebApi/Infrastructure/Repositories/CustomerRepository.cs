using FoSouzaDev.Customers.WebApi.Domain.Entities;
using FoSouzaDev.Customers.WebApi.Domain.Repositories;

namespace FoSouzaDev.Customers.WebApi.Infrastructure.Repositories
{
    public sealed class CustomerRepository : ICustomerRepository
    {
        private readonly IList<Customer> _customers;

        public CustomerRepository()
        {
            this._customers = [];
        }

        public async Task AddAsync(Customer customer)
        {
            customer.Id = $"{Guid.NewGuid()}";

            this._customers.Add(customer);
        }

        public async Task<Customer?> GetByIdAsync(string id)
        {
            return this._customers.FirstOrDefault(a => a.Id == id);
        }

        public async Task EditAsync(Customer customer)
        {
            Customer? currentCustomer = await this.GetByIdAsync(customer.Id);

            currentCustomer = customer;
        }

        public async Task DeleteAsync(string id)
        {
            Customer? customer = await this.GetByIdAsync(id);

            this._customers.Remove(customer!);
        }
    }
}