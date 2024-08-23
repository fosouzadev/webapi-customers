using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Infrastructure.Repositories.Entities;
using FoSouzaDev.Customers.Infrastructure.Repositories.Mappings;

namespace FoSouzaDev.Customers.UnitaryTests.Infrastructure.Repositories.Mappings;

public sealed class CustomerEntityMapTest : BaseTest
{
    public CustomerEntityMapTest()
    {
        base.Fixture.Customize<CustomerEntity>(a => a
            .With(b => b.BirthDate, ValidDataGenerator.ValidBirthDate)
            .With(b => b.Email, ValidDataGenerator.ValidEmail)
        );
    }

    [Fact]
    public void CustomerToCustomerEntity_Success_ReturnExpectedObject()
    {
        // Arrange
        Customer customer = base.Fixture.Create<Customer>();
        CustomerEntity expectedCustomerEntity = (CustomerEntity)customer;

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
        Customer expectedCustomer = (Customer)customerEntity!;

        // Act
        Customer customer = CustomerEntityMap.CustomerEntityToCustomer(customerEntity)!;

        // Assert
        customer.Should().BeEquivalentTo(expectedCustomer);
    }

    [Fact]
    public void CustomerEntityToCustomer_Success_ReturnNull()
    {
        // Arrange
        CustomerEntity? customerEntity = default;
        Customer? expectedCustomer = default;

        // Act
        Customer? customer = CustomerEntityMap.CustomerEntityToCustomer(customerEntity);

        // Assert
        customer.Should().BeEquivalentTo(expectedCustomer);
    }
}