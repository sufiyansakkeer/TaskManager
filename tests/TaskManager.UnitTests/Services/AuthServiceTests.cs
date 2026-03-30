using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TaskManager.Application.DTOs.Auth;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using Xunit;

namespace TaskManager.UnitTests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IJwtGenerator> _jwtMock;
        private readonly Mock<IPasswordHasher> _hasherMock;

        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _jwtMock = new Mock<IJwtGenerator>();
            _hasherMock = new Mock<IPasswordHasher>();
            _service = new AuthService(_userRepoMock.Object, _uowMock.Object, _jwtMock.Object, _hasherMock.Object);

        }

        [Fact]
        public async Task RegisterAsync_Should_Create_User_When_Email_Not_Exists()
        {
            // Given
            var request = new RegisterRequestDto
            {
                Email = "test@test.com",
                Password = "123456",
                FullName = "Test User"

            };

            _userRepoMock.Setup(x => x.IsEmailAlreadyExistAsync(request.Email)).ReturnsAsync(false);

            _hasherMock.Setup(x => x.Hash(request.Password)).Returns("hashed");

            _jwtMock.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("token");

            // When

            var result = await _service.RegisterAsync(request);

            // Then
            result.Token.Should().Be("token");

            _userRepoMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            _uowMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_Should_Throw_When_Email_Exists()
        {
            // Arrange
            var request = new RegisterRequestDto { Email = "test@test.com" };

            _userRepoMock
                .Setup(x => x.IsEmailAlreadyExistAsync(request.Email))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _service.RegisterAsync(request);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task LoginAsync_Should_Throw_When_Invalid_Password()
        {
            // Arrange
            var user = new User("test@test.com", "hashed", "Test");

            _userRepoMock
                .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _hasherMock
                .Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            // Act
            Func<Task> act = async () =>
                await _service.LoginAsync(new LoginRequestDto());

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}