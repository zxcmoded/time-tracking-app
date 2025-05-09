using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using tracking_app.Controllers;
using tracking_app.Dto;
using tracking_app.Services;

namespace tracking_app.test.unit.Controllers
{
    public class AuthControllerTests
    {
        [Test]
        public async Task Authenticate_ValidCredentials_ReturnsJwtToken()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var mockLogger = new Mock<ILogger<AuthController>>();

            var expectedToken = "fake-jwt-token-123";
            var request = new AuthRequestDto("john.doe", "Password123!");

            mockAuthService
                .Setup(s => s.Authenticate(request.Username, request.Password))
                .ReturnsAsync(new AuthResponseDto(true, expectedToken));

            var controller = new AuthController(mockLogger.Object, mockAuthService.Object);

            // Act
            var result = await controller.Authenticate(request);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as AuthResponseDto;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(expectedToken, response.Message);
        }
    }
}
