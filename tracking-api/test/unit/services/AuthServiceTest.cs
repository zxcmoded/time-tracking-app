using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using tracking_app.Dto;
using tracking_app.Models;
using tracking_app.Repository;
using tracking_app.Services;

namespace tracking_app.test.unit.services
{
    public class AuthServiceTest
    {
        private Mock<IUserRepository> _userRepositoryMock = null!;
        private IConfiguration _configuration = null!;
        private IAuthService _authService = null!;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "this_is_a_super_secret_key_12345" },
                { "Jwt:Issuer", "TestIssuer" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            _authService = new AuthService(_configuration, _userRepositoryMock.Object);
        }

        [Test]
        public async Task Authenticate_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var password = "password123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var testUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Password = hashedPassword,
                Name = "Test User"
            };

            _userRepositoryMock
                .Setup(repo => repo.GetByUserName("testuser"))
                .ReturnsAsync(testUser);

            // Act
            var result = await _authService.Authenticate("testuser", password);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message!.Length > 10); // simple token length check

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(result.Message);
            Assert.AreEqual("testuser", token.Claims.First(c => c.Type == "UserName").Value);
        }

        [Test]
        public async Task Authenticate_ShouldReturnFailure_WhenPasswordIsInvalid()
        {
            // Arrange
            var testUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Password = BCrypt.Net.BCrypt.HashPassword("correct_password"),
                Name = "Test User"
            };

            _userRepositoryMock
                .Setup(repo => repo.GetByUserName("testuser"))
                .ReturnsAsync(testUser);

            // Act
            var result = await _authService.Authenticate("testuser", "wrong_password");

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Invalid user name or password", result.Message);
        }
    }
}
