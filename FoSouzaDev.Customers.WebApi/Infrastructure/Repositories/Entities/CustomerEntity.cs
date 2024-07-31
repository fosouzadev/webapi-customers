using FoSouzaDev.Customers.WebApi.Domain.Entities;
using FoSouzaDev.Customers.WebApi.Infrastructure.Repositories.Mappings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FoSouzaDev.Customers.WebApi.Infrastructure.Repositories.Entities
{
    public sealed class CustomerEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        public required string Name { get; set; }

        public required string LastName { get; set; }

        public required DateTime BirthDate { get; set; }

        public required string Email { get; set; }

        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public string? Notes { get; set; }

        public static implicit operator CustomerEntity(Customer customer) =>
            CustomerEntityMap.CustomerToCustomerEntity(customer);

        public static implicit operator Customer(CustomerEntity customerEntity) =>
            CustomerEntityMap.CustomerEntityToCustomer(customerEntity);
    }
}