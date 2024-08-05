using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Exceptions;
using FoSouzaDev.Customers.Domain.Repositories;
using FoSouzaDev.Customers.Domain.Services;
using FoSouzaDev.Customers.Domain.ValueObjects;
using Moq;

namespace FoSouzaDev.Customers.UnitaryTests.Domain.Services
{
    public sealed class CustomerServiceTest : BaseTest
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;

        private readonly ICustomerService _customerService;

        public CustomerServiceTest()
        {
            this._customerRepositoryMock = new();
            this._customerService = new CustomerService(this._customerRepositoryMock.Object);
        }

        [Fact]
        public async Task AddAsync_Success_ReturnNewId()
        {
            // Arrange
            AddCustomerDto customer = base.Fixture.Build<AddCustomerDto>()
                .With(a => a.BirthDate, ValidBirthDate)
                .With(a => a.Email, ValidEmail)
                .Create();

            Customer entity = customer;

            this._customerRepositoryMock
                .Setup(a => a.AddAsync(It.Is<Customer>(b =>
                    b.Id == entity.Id &&
                    b.FullName == entity.FullName &&
                    b.BirthDate == entity.BirthDate &&
                    b.Email == entity.Email &&
                    b.Notes == entity.Notes)))
                .Verifiable(Times.Once);

            // Act
            string id = await this._customerService.AddAsync(customer);

            // Arrange
            id.Should().Be(entity.Id);

            this._customerRepositoryMock.Verify();
        }

        [Fact]
        public async Task GetByIdAsync_Success_ReturnEntity()
        {
            // Arrange
            string id = base.Fixture.Create<string>();
            Customer expectedCustomer = this.MockGetById(id);

            // Act
            Customer? customer = await this._customerService.GetByIdAsync(id);

            // Assert
            customer.Should().BeEquivalentTo(expectedCustomer);
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ThrowNotFoundException()
        {
            // Arrange
            string id = base.Fixture.Create<string>();

            // Act
            Func<Task> act = () => this._customerService.GetByIdAsync(id);

            // Assert
            (await act.Should().ThrowExactlyAsync<NotFoundException>())
                .WithMessage("Not found.")
                .Which.Id.Should().Be(id);
        }

        [Fact]
        public async Task EditAsync_Success_NotThrowException()
        {
            // Arrange
            string id = base.Fixture.Create<string>();
            EditCustomerDto customer = base.Fixture.Create<EditCustomerDto>();
            
            Customer expectedCustomer = this.MockGetById(id);
            expectedCustomer.FullName = new FullName(customer.Name, customer.LastName);
            expectedCustomer.Notes = customer.Notes;

            // Act
            Func<Task> act = () => this._customerService.EditAsync(id, customer);

            // Assert
            await act.Should().NotThrowAsync();

            this._customerRepositoryMock.Verify(a => a.ReplaceAsync(It.Is<Customer>(b => 
                b.Id == expectedCustomer.Id &&
                b.FullName == expectedCustomer.FullName &&
                b.BirthDate == expectedCustomer.BirthDate &&
                b.Email == expectedCustomer.Email &&
                b.Notes == expectedCustomer.Notes)), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Success_NotThrowException()
        {
            // Arrange
            string id = base.Fixture.Create<string>();

            _ = this.MockGetById(id);

            // Act
            Func<Task> act = () => this._customerService.DeleteAsync(id);

            // Assert
            await act.Should().NotThrowAsync();

            this._customerRepositoryMock.Verify(a => a.DeleteAsync(id), Times.Once);
        }

        private Customer MockGetById(string id)
        {
            Customer customer = base.Fixture.Create<Customer>();

            this._customerRepositoryMock.Setup(a => a.GetByIdAsync(id)).ReturnsAsync(customer);

            return customer;
        }
    }
}