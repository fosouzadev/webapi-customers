using FoSouzaDev.Customers.Application.Mappings;
using FoSouzaDev.Customers.Domain.Entities;

namespace FoSouzaDev.Customers.Application.DataTransferObjects;

public sealed record EditCustomerDto
{
    public string? Name { get; init; }
    public string? LastName { get; init; }
    public string? Notes { get; init; }

    public static explicit operator EditCustomerDto(Customer customer) =>
        CustomerFactory.CustomerToEditCustomerDto(customer);
}