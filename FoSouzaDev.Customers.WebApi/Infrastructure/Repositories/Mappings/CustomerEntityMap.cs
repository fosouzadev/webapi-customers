using FoSouzaDev.Customers.WebApi.Domain.Entities;
using FoSouzaDev.Customers.WebApi.Domain.ValueObjects;
using FoSouzaDev.Customers.WebApi.Infrastructure.Repositories.Entities;

namespace FoSouzaDev.Customers.WebApi.Infrastructure.Repositories.Mappings
{
    public static class CustomerEntityMap
    {
        public static CustomerEntity CustomerToCustomerEntity(Customer customer) => new()
        {
            Id = customer.Id,
            Name = customer.FullName.Name,
            LastName = customer.FullName.LastName,
            BirthDate = customer.BirthDate.Date,
            Email = customer.Email.Value,
            Notes = customer.Notes
        };

        public static Customer CustomerEntityToCustomer(CustomerEntity customerEntity) => new()
        {
            Id = customerEntity.Id,
            FullName = new FullName(customerEntity.Name, customerEntity.LastName),
            BirthDate = new BirthDate(customerEntity.BirthDate.Date),
            Email = new Email(customerEntity.Email),
            Notes = customerEntity.Notes
        };
    }
}