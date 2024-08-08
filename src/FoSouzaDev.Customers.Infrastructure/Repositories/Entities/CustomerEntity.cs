using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Infrastructure.Repositories.Mappings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FoSouzaDev.Customers.Infrastructure.Repositories.Entities;

internal sealed class CustomerEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    public required string Name { get; set; }

    public required string LastName { get; set; }

    public required DateTime BirthDate { get; set; }

    public required string Email { get; set; }

    [BsonIgnoreIfNull]
    public string? Notes { get; set; }

    public static implicit operator CustomerEntity(Customer customer) =>
        CustomerEntityMap.CustomerToCustomerEntity(customer);

    public static implicit operator Customer?(CustomerEntity? customerEntity) =>
        CustomerEntityMap.CustomerEntityToCustomer(customerEntity);
}