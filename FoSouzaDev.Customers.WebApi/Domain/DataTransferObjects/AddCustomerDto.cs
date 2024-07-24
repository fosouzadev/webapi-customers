namespace FoSouzaDev.Customers.WebApi.Domain.DataTransferObjects;

public sealed class AddCustomerDto
{
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required DateTime BirthDate { get; set; }
    public required string Email { get; set; }
    public string? Notes { get; set; }
}