using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Exceptions;
using FoSouzaDev.Customers.Domain.Repositories;
using FoSouzaDev.Customers.Domain.ValueObjects;
using FoSouzaDev.Customers.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace FoSouzaDev.Customers.IntegrationTests.Infrastructure.Repositories;

[Collection("MongoDbFixture")]
public sealed class CustomerRepositoryTest(MongoDbFixture mongoDbFixture) : BaseTest
{
    private readonly ICustomerRepository _customerRepository = new CustomerRepository(
        mongoDbFixture.MongoDatabase!,
        new LoggerFactory().CreateLogger<CustomerRepository>());

    [Fact]
    public async Task AddAsync_Success_SetNewId()
    {
        // Arrange
        Customer expectedCustomer  = base.Fixture.Create<Customer>();
        expectedCustomer.Id = string.Empty;

        // Act
        await _customerRepository.AddAsync(expectedCustomer);

        // Assert
        Customer? customer = await _customerRepository.GetByIdAsync(expectedCustomer.Id);
        customer.Should().BeEquivalentTo(expectedCustomer);
    }

    [Fact]
    public async Task AddAsync_EmailConflict_ThrowConflictException()
    {
        // Arrange
        Customer currentCustomer = base.Fixture.Create<Customer>();
        currentCustomer.Id = string.Empty;
        await _customerRepository.AddAsync(currentCustomer);

        Customer expectedCustomer = new()
        {
            Id = string.Empty,
            FullName = base.Fixture.Create<FullName>(),
            BirthDate = base.Fixture.Create<BirthDate>(),
            Email = currentCustomer.Email,
            Notes = base.Fixture.Create<string>()
        };

        // Act
        Func<Task> act = () => _customerRepository.AddAsync(expectedCustomer);

        // Assert
        ConflictException ex = (await act.Should().ThrowExactlyAsync<ConflictException>()).Which;
        ex.Message.Should().Be("Already registered.");
        ex.Email.Should().Be(expectedCustomer.Email.Value);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnNull()
    {
        // Arrange
        string id = $"{new ObjectId()}";

        // Act
        Customer? customer = await _customerRepository.GetByIdAsync(id);

        // Assert
        customer.Should().BeNull();
    }

    [Fact]
    public async Task ReplaceAsync_Success_ReplacedObject()
    {
        // Arrange
        Customer expectedCustomer = base.Fixture.Create<Customer>();
        expectedCustomer.Id = string.Empty;

        await _customerRepository.AddAsync(expectedCustomer);

        expectedCustomer.FullName = base.Fixture.Create<FullName>();
        expectedCustomer.Notes = base.Fixture.Create<string>();

        // Act
        await _customerRepository.ReplaceAsync(expectedCustomer);

        // Assert
        Customer? customer = await _customerRepository.GetByIdAsync(expectedCustomer.Id);
        customer.Should().BeEquivalentTo(expectedCustomer);
    }

    [Fact]
    public async Task ReplaceAsync_NotFound_ThrowInvalidOperationException()
    {
        // Arrange
        Customer expectedCustomer = base.Fixture.Create<Customer>();
        expectedCustomer.Id = $"{new ObjectId()}";

        // Act
        Func<Task> act = () => _customerRepository.ReplaceAsync(expectedCustomer);

        // Assert
        (await act.Should().ThrowExactlyAsync<InvalidOperationException>())
            .WithMessage($"It was not possible to replace the customer with id: {expectedCustomer.Id}");
    }

    [Fact]
    public async Task DeleteAsync_Success_DeletedObject()
    {
        // Arrange
        Customer expectedCustomer = base.Fixture.Create<Customer>();
        expectedCustomer.Id = string.Empty;

        await _customerRepository.AddAsync(expectedCustomer);

        // Act
        await _customerRepository.DeleteAsync(expectedCustomer.Id);

        // Assert
        Customer? customer = await _customerRepository.GetByIdAsync(expectedCustomer.Id);
        customer.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_NotFound_ThrowInvalidOperationException()
    {
        // Arrange
        string id = $"{new ObjectId()}";

        // Act
        Func<Task> act = () => _customerRepository.DeleteAsync(id);

        // Assert
        (await act.Should().ThrowExactlyAsync<InvalidOperationException>())
            .WithMessage($"It was not possible to delete the customer with id: {id}");
    }
}