using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.WebApi.Domain.Exceptions;
using FoSouzaDev.Customers.WebApi.Domain.ValueObjects;

namespace FoSouzaDev.Customers.UnitaryTests.Domain.ValueObjects
{
    public sealed class FullNameTest : BaseTest
    {
        [Fact]
        public void Constructor_ValidFullName_NotThrowException()
        {
            // Arrange
            string name = base.Fixture.Create<string>();
            string lastName = base.Fixture.Create<string>();

            // Act
            FullName fullName = new(name, lastName);

            // Assert
            fullName.Name.Should().Be(name);
            fullName.LastName.Should().Be(lastName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("aa")]
        public void Constructor_InvalidName_ThrowValidateException(string? name)
        {
            // Arrange
            string lastName = base.Fixture.Create<string>();

            // Act
            Action act = () => _ = new FullName(name!, lastName);

            // Assert
            act.Should().ThrowExactly<ValidateException>().WithMessage("Invalid name.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("aa")]
        public void Constructor_InvalidLastName_ThrowValidateException(string? lastName)
        {
            // Arrange
            string name = base.Fixture.Create<string>();

            // Act
            Action act = () => _ = new FullName(name, lastName!);

            // Assert
            act.Should().ThrowExactly<ValidateException>().WithMessage("Invalid last name.");
        }

        [Fact]
        public void Constructor_ObjectComparison_ShowEquality()
        {
            // Arrange
            string name = base.Fixture.Create<string>();
            string lastName = base.Fixture.Create<string>();

            FullName fullName1 = new(name, lastName);
            FullName fullName2 = new(name, lastName);

            // Act
            bool equals = fullName1 == fullName2;
            
            // Assert
            equals.Should().BeTrue();
        }
    }
}