using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.ValueObjects;
using FoSouzaDev.Customers.Infrastructure.Repositories.Entities;
using FoSouzaDev.Customers.Infrastructure.Repositories.Factories;

namespace FoSouzaDev.Customers.UnitaryTests.Infrastructure.Repositories.Mappings;

public sealed class CustomerEntityFactoryTest : BaseTest
{
    private readonly ICustomerEntityFactory _factory;

    public CustomerEntityFactoryTest()
    {
        _factory = new CustomerEntityFactory();

        Fixture.Customize<CustomerEntity>(a => a
            .With(b => b.BirthDate, ValidDataGenerator.ValidBirthDate)
            .With(b => b.Email, ValidDataGenerator.ValidEmail)
        );
    }

    [Fact]
    public void CustomerToCustomerEntity_Success_ReturnExpectedObject()
    {
        // Arrange
        Customer customer = Fixture.Create<Customer>();
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
        CustomerEntity customerEntity = _factory.CustomerToCustomerEntity(customer);

        // Assert
        customerEntity.Should().BeEquivalentTo(expectedCustomerEntity);
    }

    [Fact]
    public void CustomerEntityToCustomer_Success_ReturnExpectedObject()
    {
        // Arrange
        CustomerEntity customerEntity = Fixture.Create<CustomerEntity>();
        Customer expectedCustomer = new()
        {
            Id = customerEntity.Id,
            FullName = new FullName(customerEntity.Name, customerEntity.LastName),
            BirthDate = new BirthDate(customerEntity.BirthDate.Date),
            Email = new Email(customerEntity.Email),
            Notes = customerEntity.Notes
        };

        // Act
        Customer customer = _factory.CustomerEntityToCustomer(customerEntity);

        // Assert
        customer.Should().BeEquivalentTo(expectedCustomer);
    }

    [Fact]
    public void CustomerEntityToCustomer_Success_ReturnNull()
    {
        // Act
        Customer customer = _factory.CustomerEntityToCustomer(null);

        // Assert
        customer.Should().BeNull();
    }
}