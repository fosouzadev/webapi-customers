using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.Application.Mappings;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.ValueObjects;

namespace FoSouzaDev.Customers.UnitaryTests.Application.Mappings;

public sealed class CustomerFactoryTest : BaseTest
{
    public CustomerFactoryTest()
    {
        Fixture.Customize<CustomerDto>(a => a
            .With(b => b.BirthDate, ValidDataGenerator.ValidBirthDate)
            .With(b => b.Email, ValidDataGenerator.ValidEmail)
        );

        Fixture.Customize<AddCustomerDto>(a => a
            .With(b => b.BirthDate, ValidDataGenerator.ValidBirthDate)
            .With(b => b.Email, ValidDataGenerator.ValidEmail)
        );
    }

    [Fact]
    public void CustomerToCustomerDto_Success_ReturnExpectedObject()
    {
        // Arrange
        Customer customer = Fixture.Create<Customer>();
        CustomerDto expectedCustomerDto = new()
        {
            Id = customer.Id,
            Name = customer.FullName.Name,
            LastName = customer.FullName.LastName,
            BirthDate = customer.BirthDate.Date,
            Email = customer.Email.Value,
            Notes = customer.Notes
        };

        // Act
        CustomerDto customerDto = CustomerFactory.CustomerToCustomerDto(customer);

        // Assert
        customerDto.Should().BeEquivalentTo(expectedCustomerDto);
    }

    [Fact]
    public void AddCustomerDtoToCustomer_Success_ReturnExpectedObject()
    {
        // Arrange
        AddCustomerDto addCustomerDto = Fixture.Create<AddCustomerDto>();
        Customer expectedCustomer = new()
        {
            Id = string.Empty,
            FullName = new FullName(addCustomerDto.Name, addCustomerDto.LastName),
            BirthDate = new BirthDate(addCustomerDto.BirthDate),
            Email = new Email(addCustomerDto.Email),
            Notes = addCustomerDto.Notes
        };

        // Act
        Customer customer = CustomerFactory.AddCustomerDtoToCustomer(addCustomerDto);

        // Assert
        customer.Should().BeEquivalentTo(expectedCustomer);
    }

    [Fact]
    public void CustomerToEditCustomerDto_Success_ReturnExpectedObject()
    {
        // Arrange
        Customer customer = Fixture.Create<Customer>();
        EditCustomerDto expectedEditCustomerDto = new()
        {
            Name = customer.FullName.Name,
            LastName = customer.FullName.LastName,
            Notes = customer.Notes
        };

        // Act
        EditCustomerDto editCustomerDto = CustomerFactory.CustomerToEditCustomerDto(customer);

        // Assert
        editCustomerDto.Should().BeEquivalentTo(expectedEditCustomerDto);
    }
}