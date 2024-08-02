using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.Domain.Exceptions;

namespace FoSouzaDev.Customers.UnitaryTests.Domain.Exceptions
{
    public sealed class NotFoundExceptionTest : BaseTest
    {
        [Fact]
        public void Constructor_Success_CreateAnException()
        {
            // Arrange
            string expectedId = base.Fixture.Create<string>();

            // Act
            NotFoundException ex = new(expectedId);

            // Assert
            ex.Message.Should().Be("Not found.");
            ex.Id.Should().Be(expectedId);
        }
    }
}