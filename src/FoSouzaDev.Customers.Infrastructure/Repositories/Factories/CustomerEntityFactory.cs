using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.ValueObjects;
using FoSouzaDev.Customers.Infrastructure.Repositories.Entities;

namespace FoSouzaDev.Customers.Infrastructure.Repositories.Mappings;

internal static class CustomerEntityFactory
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

    public static Customer? CustomerEntityToCustomer(CustomerEntity? customerEntity)
    {
        if (customerEntity == null)
            return default;

        return new()
        {
            Id = customerEntity.Id,
            FullName = new FullName(customerEntity.Name, customerEntity.LastName),
            BirthDate = new BirthDate(customerEntity.BirthDate.Date),
            Email = new Email(customerEntity.Email),
            Notes = customerEntity.Notes
        };
    }
}