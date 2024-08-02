using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.WebApi.Responses;

namespace FoSouzaDev.Customers.UnitaryTests.WebApi.Responses
{
    public sealed class ResponseDataTest : BaseTest
    {
        [Fact]
        public void Constructor_SuccessWithData_CreateAnObject()
        {
            // Arrange
            string expectedData = base.Fixture.Create<string>();

            // Act
            ResponseData<string> responseData = new(data: expectedData);

            // Assert
            responseData.Data.Should().Be(expectedData);
            responseData.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public void Constructor_SuccessWithErrorMessage_CreateAnObject()
        {
            // Arrange
            string expectedErrorMessage = base.Fixture.Create<string>();

            // Act
            ResponseData<string> responseData = new(errorMessage: expectedErrorMessage);

            // Assert
            responseData.Data.Should().BeNull();
            responseData.ErrorMessage.Should().Be(expectedErrorMessage);
        }

        [Fact]
        public void Constructor_SuccessWithDataAndErrorMessage_CreateAnObject()
        {
            // Arrange
            string expectedData = base.Fixture.Create<string>();
            string expectedErrorMessage = base.Fixture.Create<string>();

            // Act
            ResponseData<string> responseData = new(expectedData, expectedErrorMessage);

            // Assert
            responseData.Data.Should().Be(expectedData);
            responseData.ErrorMessage.Should().Be(expectedErrorMessage);
        }
    }
}