using FoSouzaDev.Customers.Application.Infrastructure.Repositories;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Exceptions;
using FoSouzaDev.Customers.Infrastructure.Repositories.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FoSouzaDev.Customers.Infrastructure.Repositories;

internal sealed class CustomerRepository(IMongoDatabase mongoDatabase, ILogger<CustomerRepository> logger) : ICustomerRepository
{
    private const string CollectionName = "customers";
    private readonly IMongoCollection<CustomerEntity> _collection = mongoDatabase.GetCollection<CustomerEntity>(CollectionName);

    public async Task AddAsync(Customer customer)
    {
        CustomerEntity customerEntity = (CustomerEntity)customer;

        try
        {
            await this._collection.InsertOneAsync(customerEntity);
        }
        catch (MongoWriteException ex) when (ex.Message.Contains("E11000 duplicate key error collection"))
        {
            logger.LogError(ex, message: "Exception message: {ExceptionMessage}", ex.Message);
            throw new ConflictException(customer.Email.Value);
        }

        customer.Id = customerEntity.Id;
    }

    public async Task<Customer?> GetByIdAsync(string id)
    {
        var filter = Builders<CustomerEntity>.Filter.Eq(a => a.Id, id);
        CustomerEntity? customerEntity = (await this._collection.FindAsync(filter)).FirstOrDefault();

        return (Customer?)customerEntity;
    }

    public async Task ReplaceAsync(Customer customer)
    {
        CustomerEntity customerEntity = (CustomerEntity)customer;

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