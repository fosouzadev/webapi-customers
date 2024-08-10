using FluentAssertions;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Exceptions;
using FoSouzaDev.Customers.Domain.ValueObjects;

namespace FoSouzaDev.Customers.UnitaryTests.Domain.ValueObjects;

public sealed class EmailTest : BaseTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("a.")]
    [InlineData("#.")]
    [InlineData("`^.")]
    [InlineData("a@a")]
    [InlineData("a@a.")]
    [InlineData("abc-abc.com")]
    public void Constructor_InvalidEmail_ThrowValidateException(string? email)
    {
        // Act
        Action act = () => _ = new Email(email!);

        // Assert
        act.Should().ThrowExactly<ValidateException>().WithMessage("Invalid email.");
    }

    [Theory]
    [InlineData("a@a.a")]
    [InlineData("ab@ab.ab")]
    [InlineData("a@a.a.a")]
    [InlineData("ab@ab.ab.ab")]
    public void Constructor_ValidEmail_NotThrowException(string email)
    {
        // Act
        Email emailVo = new(email);

        // Assert
        emailVo.Value.Should().Be(email);
    }

    [Fact]
    public void Constructor_ObjectComparison_ShowEquality()
    {
        // Arrange
        string email = ValidEmail;

        Email email1 = new(email);
        Email email2 = new(email);

        // Act
        bool equals = email1 == email2;

        // Assert
        equals.Should().BeTrue();
    }
}