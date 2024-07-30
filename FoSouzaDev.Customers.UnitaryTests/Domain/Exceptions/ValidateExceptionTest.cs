using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.WebApi.Domain.Exceptions;

namespace FoSouzaDev.Customers.UnitaryTests.Domain.Exceptions
{
    public sealed class ValidateExceptionTest : BaseTest
    {
        [Fact]
        public void Constructor_Success_CreateAnException()
        {
            // Arrange
            string expectedMessage = base.Fixture.Create<string>();

            // Act
            ValidateException ex = new(expectedMessage);

            // Assert
            ex.Message.Should().Be(expectedMessage);
        }
    }
}