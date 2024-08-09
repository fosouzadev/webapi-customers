using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Repositories;
using FoSouzaDev.Customers.Domain.ValueObjects;
using FoSouzaDev.Customers.Infrastructure.Repositories;
using FoSouzaDev.Customers.Infrastructure.Repositories.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FoSouzaDev.Customers.IntegrationTests.Infrastructure.Repositories
{
    public sealed class CustomerRepositoryTest : IClassFixture<MongoDbFixture>
    {
        private readonly Fixture _fixture = new();

        private readonly ICustomerRepository _customerRepository;

        public CustomerRepositoryTest(MongoDbFixture mongoDbFixture)
        {
            this._fixture = new();

            this._fixture.Customize<BirthDate>(a => a.FromFactory(() => new BirthDate(DateTime.Now.AddYears(-18).Date)));
            this._fixture.Customize<Email>(a => a.FromFactory(() => new Email("test@test.com")));

            IMongoDatabase mongoDatabase = new MongoClient(mongoDbFixture.MongoDbContainer.GetConnectionString()).GetDatabase("testDb");
            this._customerRepository = new CustomerRepository(mongoDatabase);
    }

        [Fact]
        public async Task AddAsync_Success_SetNewId()
        {
            // Arrange
            Customer expectedCustomer  = this._fixture.Create<Customer>();
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
            Customer expectedCustomer = this._fixture.Create<Customer>();
            expectedCustomer.Id = string.Empty;

            await this._customerRepository.AddAsync(expectedCustomer);

            expectedCustomer.FullName = this._fixture.Create<FullName>();
            expectedCustomer.Notes = this._fixture.Create<string>();

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
            Customer expectedCustomer = this._fixture.Create<Customer>();
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
            Customer expectedCustomer = this._fixture.Create<Customer>();
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
}