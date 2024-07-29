using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Moq;
using TheGentlemanLibrary.Application.Models.Users.Commands;
using TheGentlemanLibrary.Application.Models.Users.Handlers;
using TheGentlemanLibrary.Application.Models.Users.Interfaces;
using TheGentlemanLibrary.Application.Models.Users.JWT;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibraryTest.Users
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IOptions<JWTConfiguration>> _mockOptions;
        private readonly Mock<IValidator<LoginCommand>> _mockValidator;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockOptions = new Mock<IOptions<JWTConfiguration>>();
            _mockValidator = new Mock<IValidator<LoginCommand>>();
            _handler = new LoginCommandHandler(_mockUserRepository.Object, _mockOptions.Object, null, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnJWTModel_WhenLoginIsSuccessful()
        {
            // Arrange
            var command = new LoginCommand("test@example.com", "Password123!");
            var user = new User { Id = 1, Email = "test@example.com" };

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockUserRepository.Setup(repo => repo.Login(command))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // Add more assertions here to check the JWT token
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenLoginFails()
        {
            // Arrange
            var command = new LoginCommand("test@example.com", "WrongPassword");

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockUserRepository.Setup(repo => repo.Login(command))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var command = new LoginCommand("invalidemail", "InvalidPassword");

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Email", "Invalid email format") }));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
