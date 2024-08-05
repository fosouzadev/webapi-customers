using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.ValueObjects;

namespace FoSouzaDev.Customers.Application.Mappings
{
    public static class CustomerMap
    {
        public static Customer CustomerDtoToCustomer(CustomerDto customerDto) => new()
        {
            Id = customerDto.Id,
            FullName = new FullName(customerDto.Name, customerDto.LastName),
            BirthDate = new BirthDate(customerDto.BirthDate),
            Email = new Email(customerDto.Email),
            Notes = customerDto.Notes
        };

        public static CustomerDto CustomerToCustomerDto(Customer customer) => new()
        {
            Id = customer.Id,
            Name = customer.FullName.Name,
            LastName = customer.FullName.LastName,
            BirthDate = customer.BirthDate.Date,
            Email = customer.Email.Value,
            Notes = customer.Notes
        };

        public static Customer AddCustomerDtoToCustomer(AddCustomerDto addCustomerDto) => new()
        {
            Id = string.Empty,
            FullName = new FullName(addCustomerDto.Name, addCustomerDto.LastName),
            BirthDate = new BirthDate(addCustomerDto.BirthDate),
            Email = new Email(addCustomerDto.Email),
            Notes = addCustomerDto.Notes
        };
    }
}