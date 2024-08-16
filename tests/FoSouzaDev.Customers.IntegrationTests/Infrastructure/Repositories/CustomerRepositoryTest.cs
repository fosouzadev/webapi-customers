using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.Application.Infrastructure.Repositories;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.ValueObjects;
using FoSouzaDev.Customers.Infrastructure.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FoSouzaDev.Customers.IntegrationTests.Infrastructure.Repositories;

[Collection("MongoDbFixture")]
public sealed class CustomerRepositoryTest : BaseTest
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerRepositoryTest(MongoDbFixture mongoDbFixture)
    {
        IMongoDatabase mongoDatabase = new MongoClient(mongoDbFixture.MongoDbContainer.GetConnectionString()).GetDatabase("testDb");
        this._customerRepository = new CustomerRepository(mongoDatabase);
}

    [Fact]
    public async Task AddAsync_Success_SetNewId()
    {
        // Arrange
        Customer expectedCustomer  = base.Fixture.Create<Customer>();
        expectedCustomer.Id = string.Empty;

        // Act
        await this._customerRepository.AddAsync(expectedCustomer);

        // Assert
        Customer? customer = await this._customerRepository.GetByIdAsync(expectedCustomer.Id);
        customer.Should().BeEquivalentTo(expectedCustomer);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnNull()
    {
        // Arrange
        string id = $"{new ObjectId()}";

        // Act
        Customer? customer = await this._customerRepository.GetByIdAsync(id);

        // Assert
        customer.Should().BeNull();
    }

    [Fact]
    public async Task ReplaceAsync_Success_ReplacedObject()
    {
        // Arrange
        Customer expectedCustomer = base.Fixture.Create<Customer>();
        expectedCustomer.Id = string.Empty;

        await this._customerRepository.AddAsync(expectedCustomer);

        expectedCustomer.FullName = base.Fixture.Create<FullName>();
        expectedCustomer.Notes = base.Fixture.Create<string>();

        // Act
        await this._customerRepository.ReplaceAsync(expectedCustomer);

        // Assert
        Customer? customer = await this._customerRepository.GetByIdAsync(expectedCustomer.Id);
        customer.Should().BeEquivalentTo(expectedCustomer);
    }

    [Fact]
    public async Task ReplaceAsync_NotFound_ThrowInvalidOperationException()
    {
        // Arrange
        Customer expectedCustomer = base.Fixture.Create<Customer>();
        expectedCustomer.Id = $"{new ObjectId()}";

        // Act
        Func<Task> act = () => this._customerRepository.ReplaceAsync(expectedCustomer);

        // Assert
        await act.Should().ThrowExactlyAsync<InvalidOperationException>($"It was not possible to replace the customer with id: {expectedCustomer.Id}");
    }

    [Fact]
    public async Task DeleteAsync_Success_DeletedObject()
    {
        // Arrange
        Customer expectedCustomer = base.Fixture.Create<Customer>();
        expectedCustomer.Id = string.Empty;

        await this._customerRepository.AddAsync(expectedCustomer);

        // Act
        await this._customerRepository.DeleteAsync(expectedCustomer.Id);

        // Assert
        Customer? customer = await this._customerRepository.GetByIdAsync(expectedCustomer.Id);
        customer.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_NotFound_ThrowInvalidOperationException()
    {
        // Arrange
        string id = $"{new ObjectId()}";

        // Act
        Func<Task> act = () => this._customerRepository.DeleteAsync(id);

        // Assert
        await act.Should().ThrowExactlyAsync<InvalidOperationException>($"It was not possible to delete the customer with id: {id}");
    }
}