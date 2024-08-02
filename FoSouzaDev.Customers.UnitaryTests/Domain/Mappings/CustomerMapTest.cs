using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.WebApi.Domain.DataTransferObjects;
using FoSouzaDev.Customers.WebApi.Domain.Entities;
using FoSouzaDev.Customers.WebApi.Domain.Mappings;
using FoSouzaDev.Customers.WebApi.Domain.ValueObjects;

namespace FoSouzaDev.Customers.UnitaryTests.Domain.Mappings
{
    public sealed class CustomerMapTest : BaseTest
    {
        public CustomerMapTest()
        {
            DateTime validBirthDate = DateTime.Now.AddYears(-18).Date;
            string validEmail = "test@test.com";

            base.Fixture.Customize<CustomerDto>(a => a
                .With(b => b.BirthDate, validBirthDate)
                .With(b => b.Email, validEmail)
            );

            base.Fixture.Customize<AddCustomerDto>(a => a
                .With(b => b.BirthDate, validBirthDate)
                .With(b => b.Email, validEmail)
            );

            base.Fixture.Customize<Customer>(a => a
                .With(b => b.BirthDate, new BirthDate(validBirthDate))
                .With(b => b.Email, new Email(validEmail))
            );
        }

        [Fact]
        public void CustomerDtoToCustomer_Success_ReturnExpectedObject()
        {
            // Arrange
            CustomerDto customerDto = base.Fixture.Create<CustomerDto>();
            Customer expectedCustomer = new()
            {
                Id = customerDto.Id,
                FullName = new FullName(customerDto.Name, customerDto.LastName),
                BirthDate = new BirthDate(customerDto.BirthDate),
                Email = new Email(customerDto.Email),
                Notes = customerDto.Notes
            };

            // Act
            Customer customer = CustomerMap.CustomerDtoToCustomer(customerDto);

            // Assert
            customer.Should().BeEquivalentTo(expectedCustomer);
        }

        [Fact]
        public void CustomerToCustomerDto_Success_ReturnExpectedObject()
        {
            // Arrange
            Customer customer = base.Fixture.Create<Customer>();
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
            CustomerDto customerDto = CustomerMap.CustomerToCustomerDto(customer);

            // Assert
            customerDto.Should().BeEquivalentTo(expectedCustomerDto);
        }

        [Fact]
        public void AddCustomerDtoToCustomer_Success_ReturnExpectedObject()
        {
            // Arrange
            AddCustomerDto addCustomerDto = base.Fixture.Create<AddCustomerDto>();
            Customer expectedCustomer = new()
            {
                Id = string.Empty,
                FullName = new FullName(addCustomerDto.Name, addCustomerDto.LastName),
                BirthDate = new BirthDate(addCustomerDto.BirthDate),
                Email = new Email(addCustomerDto.Email),
                Notes = addCustomerDto.Notes
            };

            // Act
            Customer customer = CustomerMap.AddCustomerDtoToCustomer(addCustomerDto);

            // Assert
            customer.Should().BeEquivalentTo(expectedCustomer);
        }
    }
}