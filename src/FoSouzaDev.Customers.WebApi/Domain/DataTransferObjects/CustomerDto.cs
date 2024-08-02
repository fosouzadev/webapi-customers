namespace FoSouzaDev.Customers.WebApi.Domain.DataTransferObjects;

public sealed class CustomerDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string LastName { get; init; }
    public required DateTime BirthDate { get; init; }
    public required string Email { get; init; }
    public string? Notes { get; init; }
}