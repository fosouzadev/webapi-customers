namespace FoSouzaDev.Customers.Application.DataTransferObjects;

public sealed record EditCustomerDto
{
    public required string Name { get; init; }
    public required string LastName { get; init; }
    public string? Notes { get; init; }
}