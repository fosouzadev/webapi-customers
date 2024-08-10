using AutoFixture;
using FluentAssertions;
using FoSouzaDev.Customers.WebApi;
using FoSouzaDev.Customers.Domain.Exceptions;
using HttpContextMoq;
using HttpContextMoq.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text;
using FoSouzaDev.Customers.CommonTests;

namespace FoSouzaDev.Customers.UnitaryTests.WebApi;

public sealed class ApplicationExceptionHandlerTest : BaseTest
{
    private readonly ApplicationExceptionHandler _applicationExceptionHandler;

    public ApplicationExceptionHandlerTest()
    {
        this._applicationExceptionHandler = new(new Mock<ILogger<ApplicationExceptionHandler>>().Object);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task TryHandleAsync_BadRequest_ReturnExpectedResponse(HttpStatusCode statusCode)
    {
        // Arrange
        byte[] bodyJson = Encoding.UTF8.GetBytes("{\"test\":\"test\"}");
        Stream stream = new MemoryStream();
        stream.Write(bodyJson, 0, bodyJson.Length);

        HttpContextMock httpContext = new HttpContextMock()
            .SetupResponseBody(stream)
            .SetupResponseContentType("application/json");

        Exception ex = statusCode switch
        {
            HttpStatusCode.BadRequest => base.Fixture.Create<ValidateException>(),
            HttpStatusCode.NotFound => base.Fixture.Create<NotFoundException>(),
            _ => base.Fixture.Create<Exception>()
        };
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        bool tryHandle = await this._applicationExceptionHandler.TryHandleAsync(httpContext, ex, cancellationToken);

        // Assert
        tryHandle.Should().BeTrue();
    }
}