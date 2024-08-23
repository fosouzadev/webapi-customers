using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.Application.Services;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.WebApi.Controllers;
using FoSouzaDev.Customers.WebApi.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Moq;

namespace FoSouzaDev.Customers.UnitaryTests.WebApi.Controllers;

public sealed class CustomerControllerTest : BaseTest
{
    private readonly Mock<ICustomerApplicationService> _customerApplicationService;

    private readonly CustomerController _customerController;

    public CustomerControllerTest()
    {
        _customerApplicationService = new();
        _customerController = new(_customerApplicationService.Object);
    }

    [Fact]
    public async Task AddAsync_Success_ReturnHttpResponseCreatedWithExpectedData()
    {
        // Arrange
        AddCustomerDto request = base.Fixture.Create<AddCustomerDto>();
        string expectedId = base.Fixture.Create<string>();

        _customerApplicationService.Setup(a => a.AddAsync(request)).ReturnsAsync(expectedId);

        // Act
        IResult response = await _customerController.AddAsync(request);

        // Assert
        response.Should().BeOfType<Created<ResponseData<string>>>()
            .Subject.Value.Should().Match<ResponseData<string>>(a =>
                a.Data == expectedId &&
                a.ErrorMessage == null);
    }

    [Fact]
    public async Task GetByIdAsync_Success_ReturnHttpResponseOkWithExpectedData()
    {
        // Arrange
        string id = base.Fixture.Create<string>();
        CustomerDto expectedCustomer = base.Fixture.Create<CustomerDto>();

        _customerApplicationService.Setup(a => a.GetByIdAsync(id)).ReturnsAsync(expectedCustomer);

        // Act
        IResult response = await _customerController.GetByIdAsync(id);

        // Assert
        ResponseData<CustomerDto>? responseData = response.Should().BeOfType<Ok<ResponseData<CustomerDto>>>().Subject.Value;
        responseData!.ErrorMessage.Should().BeNull();
        responseData.Data.Should().BeEquivalentTo(expectedCustomer);
    }

    [Fact]
    public async Task EditAsync_Success_ReturnHttpResponseNoContent()
    {
        // Arrange
        string id = base.Fixture.Create<string>();
        JsonPatchDocument<EditCustomerDto> request = base.Fixture.Build<JsonPatchDocument<EditCustomerDto>>()
            .Without(a => a.ContractResolver)
            .Create();

        // Act
        IResult response = await _customerController.EditAsync(id, request);

        // Assert
        response.Should().BeOfType<NoContent>();
    }

    [Fact]
    public async Task DeleteAsync_Success_ReturnHttpResponseNoContent()
    {
        // Arrange
        string id = base.Fixture.Create<string>();

        // Act
        IResult response = await _customerController.DeleteAsync(id);

        // Assert
        response.Should().BeOfType<NoContent>();
    }
}