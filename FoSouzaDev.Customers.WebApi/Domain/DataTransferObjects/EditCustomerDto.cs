namespace FoSouzaDev.Customers.WebApi.Domain.DataTransferObjects;

public sealed class EditCustomerDto
{
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public string? Notes { get; set; }
}