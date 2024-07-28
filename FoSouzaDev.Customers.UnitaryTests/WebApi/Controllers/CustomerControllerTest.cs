using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.WebApi.Controllers;
using FoSouzaDev.Customers.WebApi.Domain.DataTransferObjects;
using FoSouzaDev.Customers.WebApi.Domain.Entities;
using FoSouzaDev.Customers.WebApi.Domain.Services;
using FoSouzaDev.Customers.WebApi.Domain.ValueObjects;
using FoSouzaDev.Customers.WebApi.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace FoSouzaDev.Customers.UnitaryTests.WebApi.Controllers
{
    public sealed class CustomerControllerTest : BaseTest
    {
        private readonly Mock<ICustomerService> _customerService;

        private readonly CustomerController _customerController;

        public CustomerControllerTest()
        {
            this._customerService = new();
            this._customerController = new(this._customerService.Object);
        }

        [Fact]
        public async Task AddAsync_Success_ReturnHttpResponseCreatedWithExpectedData()
        {
            // Arrange
            AddCustomerDto request = base.Fixture.Create<AddCustomerDto>();
            string expectedId = base.Fixture.Create<string>();

            this._customerService.Setup(a => a.AddAsync(request)).ReturnsAsync(expectedId);

            // Act
            var response = await this._customerController.AddAsync(request);

            // Assert
            response.Result.Should().BeOfType<Created<ResponseData<string>>>()
                .Subject.Value.Should().Match<ResponseData<string>>(a =>
                    a.Data == expectedId &&
                    a.ErrorMessage == null);
        }

        [Fact]
        public async Task GetByIdAsync_Success_ReturnHttpResponseOkWithExpectedData()
        {
            // Arrange
            string id = base.Fixture.Create<string>();

            base.Fixture.Customize<BirthDate>(a => a.FromFactory(() => new BirthDate(DateTime.Now.AddYears(-18))));
            base.Fixture.Customize<Email>(a => a.FromFactory(() => new Email("test@test.com")));
            Customer customer = base.Fixture.Create<Customer>();

            CustomerDto expectedCustomer = new()
            {
                Id = customer!.Id,
                Name = customer.FullName.Name,
                LastName = customer.FullName.LastName,
                BirthDate = customer.BirthDate.Date,
                Email = customer.Email.Value,
                Notes = customer.Notes
            };

            this._customerService.Setup(a => a.GetByIdAsync(id)).ReturnsAsync(customer);

            // Act
            var response = await this._customerController.GetByIdAsync(id);

            // Assert
            ResponseData<CustomerDto>? responseData = response.Result.Should().BeOfType<Ok<ResponseData<CustomerDto>>>().Subject.Value;
            responseData!.ErrorMessage.Should().BeNull();
            responseData.Data.Should().BeEquivalentTo(expectedCustomer);
        }

        [Fact]
        public async Task EditAsync_Success_ReturnHttpResponseNoContent()
        {
            // Arrange
            string id = base.Fixture.Create<string>();
            EditCustomerDto request = base.Fixture.Create<EditCustomerDto>();

            // Act
            var response = await this._customerController.EditAsync(id, request);

            // Assert
            response.Result.Should().BeOfType<NoContent>();
        }

        [Fact]
        public async Task DeleteAsync_Success_ReturnHttpResponseNoContent()
        {
            // Arrange
            string id = base.Fixture.Create<string>();

            // Act
            var response = await this._customerController.DeleteAsync(id);

            // Assert
            response.Result.Should().BeOfType<NoContent>();
        }
    }
}