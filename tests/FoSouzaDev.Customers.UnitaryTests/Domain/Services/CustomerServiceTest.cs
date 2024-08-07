using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Exceptions;
using FoSouzaDev.Customers.Domain.Repositories;
using FoSouzaDev.Customers.Domain.Services;
using FoSouzaDev.Customers.Domain.ValueObjects;
using Moq;

namespace FoSouzaDev.Customers.UnitaryTests.Domain.Services;

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
    public async Task AddAsync_Success_NotThrowException()
    {
        // Arrange
        Customer customer = base.Fixture.Create<Customer>();

        // Act
        Func<Task> act = () => this._customerService.AddAsync(customer);

        // Arrange
        await act.Should().NotThrowAsync();

        this._customerRepositoryMock.Verify(a => a.AddAsync(customer), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Success_ReturnEntity()
    {
        // Arrange
        string id = base.Fixture.Create<string>();
        Customer expectedCustomer = this.MockGetById(id);

        // Act
        Customer customer = await this._customerService.GetByIdAsync(id);

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

        Customer expectedCustomer = this.MockGetById(id);
        expectedCustomer.FullName = base.Fixture.Create<FullName>();
        expectedCustomer.Notes = base.Fixture.Create<string>();

        // Act
        Func<Task> act = () => this._customerService.EditAsync(id, expectedCustomer.FullName, expectedCustomer.Notes);

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