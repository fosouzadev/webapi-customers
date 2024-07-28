using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.WebApi.Domain.DataTransferObjects;
using FoSouzaDev.Customers.WebApi.Domain.Entities;
using FoSouzaDev.Customers.WebApi.Domain.Repositories;
using FoSouzaDev.Customers.WebApi.Domain.Services;
using FoSouzaDev.Customers.WebApi.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

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
                .With(a => a.BirthDate, DateTime.Now.AddYears(-18))
                .With(a => a.Email, "test@test.com")
                .Create();

            string expectedId = base.Fixture.Create<string>();
            Customer entity = new()
            {
                Id = string.Empty,
                FullName = new FullName(customer.Name, customer.LastName),
                BirthDate = new BirthDate(customer.BirthDate),
                Email = new Email(customer.Email),
                Notes = customer.Notes
            };

            this._customerRepositoryMock
                .Setup(a => a.AddAsync(It.Is<Customer>(b =>
                    b.Id == entity.Id &&
                    b.FullName == entity.FullName &&
                    b.BirthDate == entity.BirthDate &&
                    b.Email == entity.Email &&
                    b.Notes == entity.Notes)))
                .Callback(() => entity.Id = expectedId);

            // Act
            string id = await this._customerService.AddAsync(customer);

            // Arrange
            id.Should().Be(expectedId);
        }
    }
}