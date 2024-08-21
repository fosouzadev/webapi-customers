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

    [BsonRequired]
    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("lastName")]
    public required string LastName { get; set; }

    [BsonElement("birthDate")]
    public required DateTime BirthDate { get; set; }

    [BsonElement("email")]
    public required string Email { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("notes")]
    public string? Notes { get; set; }

    public static explicit operator CustomerEntity(Customer customer) =>
        CustomerEntityMap.CustomerToCustomerEntity(customer);

    public static explicit operator Customer?(CustomerEntity? customerEntity) =>
        CustomerEntityMap.CustomerEntityToCustomer(customerEntity);
}