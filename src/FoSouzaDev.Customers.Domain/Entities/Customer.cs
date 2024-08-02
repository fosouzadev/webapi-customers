using FoSouzaDev.Customers.Domain.DataTransferObjects;
using FoSouzaDev.Customers.Domain.Mappings;
using FoSouzaDev.Customers.Domain.ValueObjects;

namespace FoSouzaDev.Customers.Domain.Entities
{
    public sealed class Customer
    {
        public required string Id { get; set; }
        public required FullName FullName { get; set; }
        public required BirthDate BirthDate { get; init; }
        public required Email Email { get; init; }
        public string? Notes { get; set; }

        public static implicit operator Customer(CustomerDto customerDto) =>
            CustomerMap.CustomerDtoToCustomer(customerDto);

        public static implicit operator CustomerDto(Customer customer) =>
            CustomerMap.CustomerToCustomerDto(customer);

        public static implicit operator Customer(AddCustomerDto addCustomerDto) =>
            CustomerMap.AddCustomerDtoToCustomer(addCustomerDto);
    }
}