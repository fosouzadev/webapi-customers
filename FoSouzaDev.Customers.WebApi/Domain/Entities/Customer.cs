using FoSouzaDev.Customers.WebApi.Domain.ValueObjects;

namespace FoSouzaDev.Customers.WebApi.Domain.Entities
{
    public sealed class Customer
    {
        public required string Id { get; set; }
        public required FullName FullName { get; set; }
        public required BirthDate BirthDate { get; init; }
        public required Email Email { get; init; }
        public string? Notes { get; set; }
    }
}