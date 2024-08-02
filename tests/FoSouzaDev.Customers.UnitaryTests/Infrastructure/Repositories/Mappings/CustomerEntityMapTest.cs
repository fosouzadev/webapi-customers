using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.WebApi.Domain.Entities;
using FoSouzaDev.Customers.WebApi.Domain.ValueObjects;
using FoSouzaDev.Customers.WebApi.Infrastructure.Repositories.Entities;
using FoSouzaDev.Customers.WebApi.Infrastructure.Repositories.Mappings;

namespace FoSouzaDev.Customers.UnitaryTests.Infrastructure.Repositories.Mappings
{
    public sealed class CustomerEntityMapTest : BaseTest
    {
        public CustomerEntityMapTest()
        {
            base.Fixture.Customize<CustomerEntity>(a => a
                .With(b => b.BirthDate, ValidBirthDate)
                .With(b => b.Email, ValidEmail)
            );
        }

        [Fact]
        public void CustomerToCustomerEntity_Success_ReturnExpectedObject()
        {
            // Arrange
            Customer customer = base.Fixture.Create<Customer>();
            CustomerEntity expectedCustomerEntity = new()
            {
                Id = customer.Id,
                Name = customer.FullName.Name,
                LastName = customer.FullName.LastName,
                BirthDate = customer.BirthDate.Date,
                Email = customer.Email.Value,
                Notes = customer.Notes
            };

            // Act
            CustomerEntity customerEntity = CustomerEntityMap.CustomerToCustomerEntity(customer);

            // Assert
            customerEntity.Should().BeEquivalentTo(expectedCustomerEntity);
        }

        [Fact]
        public void CustomerEntityToCustomer_Success_ReturnExpectedObject()
        {
            // Arrange
            CustomerEntity customerEntity = base.Fixture.Create<CustomerEntity>();
            Customer expectedCustomer = new()
            {
                Id = customerEntity.Id,
                FullName = new FullName(customerEntity.Name, customerEntity.LastName),
                BirthDate = new BirthDate(customerEntity.BirthDate.Date),
                Email = new Email(customerEntity.Email),
                Notes = customerEntity.Notes
            };

            // Act
            Customer customer = CustomerEntityMap.CustomerToCustomerEntity(customerEntity);

            // Assert
            customer.Should().BeEquivalentTo(expectedCustomer);
        }
    }
}