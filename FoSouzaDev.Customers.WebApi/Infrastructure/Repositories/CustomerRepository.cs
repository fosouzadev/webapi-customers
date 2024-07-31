using FoSouzaDev.Customers.WebApi.Domain.Entities;
using FoSouzaDev.Customers.WebApi.Domain.Repositories;
using FoSouzaDev.Customers.WebApi.Infrastructure.Repositories.Entities;
using MongoDB.Driver;

namespace FoSouzaDev.Customers.WebApi.Infrastructure.Repositories
{
    public sealed class CustomerRepository(IMongoDatabase mongoDatabase) : ICustomerRepository
    {
        private const string CollectionName = "Customers";
        private readonly IMongoCollection<CustomerEntity> _collection = mongoDatabase.GetCollection<CustomerEntity>(CollectionName);

        public async Task AddAsync(Customer customer)
        {
            CustomerEntity customerEntity = customer;
            await this._collection.InsertOneAsync(customerEntity);

            customer.Id = customerEntity.Id;
        }

        public async Task<Customer?> GetByIdAsync(string id)
        {
            var filter = Builders<CustomerEntity>.Filter.Eq(a => a.Id, id);
            CustomerEntity? customerEntity = (await this._collection.FindAsync(filter)).FirstOrDefault();

            if (customerEntity == null)
                return default;

            return (Customer?)customerEntity;
        }

        public async Task ReplaceAsync(Customer customer)
        {
            CustomerEntity customerEntity = customer;

            var filter = Builders<CustomerEntity>.Filter.Eq(a => a.Id, customerEntity.Id);
            ReplaceOneResult result = await this._collection.ReplaceOneAsync(filter, customerEntity);

            if (result.ModifiedCount != 1)
                throw new InvalidOperationException($"It was not possible to replace the customer with id: {customerEntity.Id}");
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<CustomerEntity>.Filter.Eq(a => a.Id, id);

            DeleteResult result = await this._collection.DeleteOneAsync(filter);

            if (result.DeletedCount != 1)
                throw new InvalidOperationException($"It was not possible to delete the customer with id: {id}");
        }
    }
}