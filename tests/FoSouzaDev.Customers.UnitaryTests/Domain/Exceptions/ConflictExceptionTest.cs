using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Exceptions;

namespace FoSouzaDev.Customers.UnitaryTests.Domain.Exceptions;

public sealed class ConflictExceptionTest : BaseTest
{
    [Fact]
    public void Constructor_Success_CreateAnException()
    {
        // Arrange
        string expectedEmail = base.Fixture.Create<string>();

        // Act
        ConflictException ex = new(expectedEmail);

        // Assert
        ex.Message.Should().Be("Already registered.");
        ex.Email.Should().Be(expectedEmail);
    }
}