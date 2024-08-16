using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.CommonTests;
using FoSouzaDev.Customers.Domain.Exceptions;
using FoSouzaDev.Customers.WebApi;
using HttpContextMoq;
using HttpContextMoq.Extensions;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace FoSouzaDev.Customers.UnitaryTests.WebApi;

public sealed class ApplicationExceptionHandlerTest : BaseTest
{
    private readonly ApplicationExceptionHandler _applicationExceptionHandler;

    public ApplicationExceptionHandlerTest()
    {
        this._applicationExceptionHandler = new(new Mock<ILogger<ApplicationExceptionHandler>>().Object);
    }

    [Theory]
    [InlineData("ValidateException")]
    [InlineData("JsonPatchException")]
    [InlineData("NotFoundException")]
    [InlineData("Exception")]
    public async Task TryHandleAsync_BadRequest_ReturnExpectedResponse(string exceptionType)
    {
        // Arrange
        byte[] bodyJson = Encoding.UTF8.GetBytes("{\"test\":\"test\"}");
        Stream stream = new MemoryStream();
        stream.Write(bodyJson, 0, bodyJson.Length);

        HttpContextMock httpContext = new HttpContextMock()
            .SetupResponseBody(stream)
            .SetupResponseContentType("application/json");

        Exception ex = exceptionType switch
        {
            nameof(ValidateException) => base.Fixture.Create<ValidateException>(),
            nameof(JsonPatchException) => base.Fixture.Create<JsonPatchException>(),
            nameof(NotFoundException) => base.Fixture.Create<NotFoundException>(),
            _ => base.Fixture.Create<Exception>()
        };
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        bool tryHandle = await this._applicationExceptionHandler.TryHandleAsync(httpContext, ex, cancellationToken);

        // Assert
        tryHandle.Should().BeTrue();
    }
}