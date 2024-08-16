using AutoFixture;
using Castle.Core.Resource;
using FluentAssertions;
using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.Application.Infrastructure.Repositories;
using FoSouzaDev.Customers.Application.Services;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Exceptions;
using FoSouzaDev.Customers.Domain.ValueObjects;
using Moq;

namespace FoSouzaDev.Customers.UnitaryTests.Application.Services;

public sealed class CustomerApplicationServiceTest : BaseTest
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;

    private readonly ICustomerApplicationService _customerApplicationService;

    public CustomerApplicationServiceTest()
    {
        this._customerRepositoryMock = new();

        this._customerApplicationService = new CustomerApplicationService(this._customerRepositoryMock.Object);
    }

    [Fact]
    public async Task AddAsync_Success_ReturnId()
    {
        // Arrange
        AddCustomerDto customer = base.Fixture.Build<AddCustomerDto>()
            .With(a => a.BirthDate, ValidBirthDate)
            .With(a => a.Email, ValidEmail)
            .Create();

        // Act
        string id = await this._customerApplicationService.AddAsync(customer);

        // Assert
        id.Should().Be(string.Empty);

        this._customerRepositoryMock.Verify(a => a.AddAsync(It.Is<Customer>(b =>
            b.FullName.Name == customer.Name &&
            b.FullName.LastName == customer.LastName &&
            b.BirthDate.Date == customer.BirthDate &&
            b.Email.Value == customer.Email &&
            b.Notes == customer.Notes)), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Success_ReturnObject()
    {
        // Arrange
        string id = base.Fixture.Create<string>();
        Customer expectedCustomer = MockGetById(id);

        // Act
        CustomerDto customer = await this._customerApplicationService.GetByIdAsync(id);

        // Assert
        customer.Should().Be((CustomerDto)expectedCustomer);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ThrowNotFoundException()
    {
        // Arrange
        string id = base.Fixture.Create<string>();

        // Act
        Func<Task> act = () => this._customerApplicationService.GetByIdAsync(id);

        // Assert
        await act.Should().ThrowExactlyAsync<NotFoundException>();
    }

    [Fact]
    public async Task EditAsync_Success_NotThrowException()
    {
        // Arrange
        string id = base.Fixture.Create<string>();
        EditCustomerDto customer = base.Fixture.Create<EditCustomerDto>();
        
        Customer expectedCustomer = MockGetById(id);
        expectedCustomer.FullName = new FullName(customer.Name, customer.LastName);
        expectedCustomer.Notes = customer.Notes;

        // Act
        Func<Task> act = () => this._customerApplicationService.EditAsync(id, customer);

        // Assert
        await act.Should().NotThrowAsync();

        this._customerRepositoryMock.Verify(a => a.ReplaceAsync(It.Is<Customer>(b =>
            b.FullName == expectedCustomer.FullName &&
            b.BirthDate == expectedCustomer.BirthDate &&
            b.Email == expectedCustomer.Email &&
            b.Notes == expectedCustomer.Notes)), Times.Once);
    }

    [Fact]
    public async Task EditAsync_NotFound_ThrowNotFoundException()
    {
        // Arrange
        string id = base.Fixture.Create<string>();
        EditCustomerDto customer = base.Fixture.Create<EditCustomerDto>();

        // Act
        Func<Task> act = () => this._customerApplicationService.EditAsync(id, customer);

        // Assert
        await act.Should().ThrowExactlyAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_Success_NotThrowException()
    {
        // Arrange
        string id = base.Fixture.Create<string>();
        _ = MockGetById(id);

        // Act
        Func<Task> act = () => this._customerApplicationService.DeleteAsync(id);

        // Assert
        await act.Should().NotThrowAsync();

        this._customerRepositoryMock.Verify(a => a.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NotFound_ThrowNotFoundException()
    {
        // Arrange
        string id = base.Fixture.Create<string>();

        // Act
        Func<Task> act = () => this._customerApplicationService.DeleteAsync(id);

        // Assert
        await act.Should().ThrowExactlyAsync<NotFoundException>();
    }

    private Customer MockGetById(string id)
    {
        Customer expectedCustomer = base.Fixture.Create<Customer>();

        this._customerRepositoryMock.Setup(a => a.GetByIdAsync(id)).ReturnsAsync(expectedCustomer);

        return expectedCustomer;
    }
}