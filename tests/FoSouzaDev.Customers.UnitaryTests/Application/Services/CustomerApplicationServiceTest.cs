using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.Application.Factories;
using FoSouzaDev.Customers.Application.Services;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Exceptions;
using FoSouzaDev.Customers.Domain.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Moq;

namespace FoSouzaDev.Customers.UnitaryTests.Application.Services;

public sealed class CustomerApplicationServiceTest : BaseTest
{
    private readonly Mock<ICustomerRepository> _repositoryMock;
    private readonly Mock<ICustomerFactory> _factoryMock;

    private readonly ICustomerApplicationService _applicationService;

    public CustomerApplicationServiceTest()
    {
        _repositoryMock = new Mock<ICustomerRepository>();
        _factoryMock = new Mock<ICustomerFactory>();

        _applicationService = new CustomerApplicationService(
            _repositoryMock.Object,
            _factoryMock.Object);
    }

    private Customer MockGetById(string id)
    {
        Customer expectedCustomer = Fixture.Create<Customer>();

        _repositoryMock.Setup(a => a.GetByIdAsync(id)).ReturnsAsync(expectedCustomer);

        return expectedCustomer;
    }

    [Fact]
    public async Task AddAsync_Success_ReturnId()
    {
        // Arrange
        AddCustomerDto customer = Fixture.Create<AddCustomerDto>();
        Customer expectedEntity = Fixture.Create<Customer>();

        _factoryMock.Setup(a => a.AddCustomerDtoToCustomer(customer))
            .Returns(expectedEntity);

        // Act
        string id = await _applicationService.AddAsync(customer);

        // Assert
        id.Should().Be(expectedEntity.Id);

        _repositoryMock.Verify(a => a.AddAsync(expectedEntity), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Success_ReturnObject()
    {
        // Arrange
        string id = Fixture.Create<string>();
        Customer expectedCustomer = MockGetById(id);
        CustomerDto expectedCustomerDto = Fixture.Create<CustomerDto>();

        _factoryMock.Setup(a => a.CustomerToCustomerDto(expectedCustomer))
            .Returns(expectedCustomerDto);

        // Act
        CustomerDto customer = await _applicationService.GetByIdAsync(id);

        // Assert
        customer.Should().Be(expectedCustomerDto);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ThrowNotFoundException()
    {
        // Arrange
        string id = Fixture.Create<string>();

        // Act
        Func<Task> act = () => _applicationService.GetByIdAsync(id);

        // Assert
        await act.Should().ThrowExactlyAsync<NotFoundException>();
    }

    [Theory]
    [InlineData(OperationType.Replace, "/name", "testName")]
    [InlineData(OperationType.Replace, "/lastName", "testLastName")]
    [InlineData(OperationType.Replace, "/notes", "testNotes")]
    [InlineData(OperationType.Remove, "/notes", null)]
    [InlineData(OperationType.Add, "/notes", "testNotes")]
    public async Task EditAsync_Success_NotThrowException(OperationType operationType, string path, string value)
    {
        // Arrange
        string id = Fixture.Create<string>();
        Customer expectedCustomer = MockGetById(id);
        EditCustomerDto editCustomerDto = Fixture.Create<EditCustomerDto>();

        _factoryMock.Setup(a => a.CustomerToEditCustomerDto(expectedCustomer))
            .Returns(editCustomerDto);

        JsonPatchDocument<EditCustomerDto> pathDocument = new();
        pathDocument.Operations.Add(new Operation<EditCustomerDto>
        {
            op = operationType.ToString().ToLower(),
            path = path,
            value = value
        });

        // Act
        Func<Task> act = () => _applicationService.EditAsync(id, pathDocument);

        // Assert
        await act.Should().NotThrowAsync();

        expectedCustomer.FullName.Name.Should().Be(editCustomerDto.Name);
        expectedCustomer.FullName.LastName.Should().Be(editCustomerDto.LastName);
        expectedCustomer.Notes.Should().Be(editCustomerDto.Notes);

        _repositoryMock.Verify(a => a.ReplaceAsync(expectedCustomer), Times.Once);
    }

    [Fact]
    public async Task EditAsync_NotFound_ThrowNotFoundException()
    {
        // Arrange
        string id = Fixture.Create<string>();

        // Act
        Func<Task> act = () => _applicationService.EditAsync(id, null);

        // Assert
        await act.Should().ThrowExactlyAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_Success_NotThrowException()
    {
        // Arrange
        string id = Fixture.Create<string>();
        _ = MockGetById(id);

        // Act
        Func<Task> act = () => _applicationService.DeleteAsync(id);

        // Assert
        await act.Should().NotThrowAsync();

        _repositoryMock.Verify(a => a.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NotFound_ThrowNotFoundException()
    {
        // Arrange
        string id = Fixture.Create<string>();

        // Act
        Func<Task> act = () => _applicationService.DeleteAsync(id);

        // Assert
        await act.Should().ThrowExactlyAsync<NotFoundException>();
    }
}