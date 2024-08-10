using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.Application.Services;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Services;
using FoSouzaDev.Customers.Domain.ValueObjects;
using Moq;

namespace FoSouzaDev.Customers.UnitaryTests.Application.Services;

public sealed class CustomerApplicationServiceTest : BaseTest
{
    private readonly Mock<ICustomerService> _customerServiceMock;

    private readonly ICustomerApplicationService _customerApplicationService;

    public CustomerApplicationServiceTest()
    {
        this._customerServiceMock = new();

        this._customerApplicationService = new CustomerApplicationService(this._customerServiceMock.Object);
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

        this._customerServiceMock.Verify(a => a.AddAsync(It.Is<Customer>(b =>
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

        CustomerDto expectedCustomer = base.Fixture.Build<CustomerDto>()
            .With(a => a.BirthDate, ValidBirthDate)
            .With(a => a.Email, ValidEmail)
            .Create();
        this._customerServiceMock.Setup(a => a.GetByIdAsync(id)).ReturnsAsync(expectedCustomer);

        // Act
        CustomerDto customer = await this._customerApplicationService.GetByIdAsync(id);

        // Assert
        customer.Should().Be(expectedCustomer);
    }

    [Fact]
    public async Task EditAsync_Success_NotThrowException()
    {
        // Arrange
        string id = base.Fixture.Create<string>();
        EditCustomerDto customer = base.Fixture.Create<EditCustomerDto>();

        // Act
        Func<Task> act = () => this._customerApplicationService.EditAsync(id, customer);

        // Assert
        await act.Should().NotThrowAsync();

        this._customerServiceMock.Verify(a =>
            a.EditAsync(id, new FullName(customer.Name, customer.LastName), customer.Notes), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Success_NotThrowException()
    {
        // Arrange
        string id = base.Fixture.Create<string>();

        // Act
        Func<Task> act = () => this._customerApplicationService.DeleteAsync(id);

        // Assert
        await act.Should().NotThrowAsync();

        this._customerServiceMock.Verify(a => a.DeleteAsync(id), Times.Once);
    }
}