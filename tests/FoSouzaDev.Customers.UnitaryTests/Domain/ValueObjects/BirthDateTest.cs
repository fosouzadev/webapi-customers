using FluentAssertions;
using FoSouzaDev.Customers.Domain.Exceptions;
using FoSouzaDev.Customers.Domain.ValueObjects;

namespace FoSouzaDev.Customers.UnitaryTests.Domain.ValueObjects
{
    public sealed class BirthDateTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Constructor_ValidBirthDate_NotThrowException(int month)
        {
            // Arrange
            DateTime date = DateTime.Now.AddYears(-new Random().Next(18, 101)).AddMonths(month);

            // Act
            BirthDate birthDate = new(date);

            // Assert
            birthDate.Date.Should().Be(date.Date);
        }

        [Theory]
        [InlineData(17)]
        [InlineData(101)]
        public void Constructor_InvalidAge_ThrowValidateException(int age)
        {
            // Arrange
            DateTime invalidDate = DateTime.Now.AddYears(-age);

            // Act
            Action act = () => _ = new BirthDate(invalidDate);

            // Assert
            act.Should().ThrowExactly<ValidateException>().WithMessage("Invalid age.");
        }

        [Fact]
        public void Constructor_ObjectComparison_ShowEquality()
        {
            // Arrange
            DateTime date = DateTime.Now.AddYears(-new Random().Next(18, 101));

            BirthDate birthDate1 = new(date);
            BirthDate birthDate2 = new(date);

            // Act
            bool equals = birthDate1 == birthDate2;

            // Assert
            equals.Should().BeTrue();
        }
    }
}