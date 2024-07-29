using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Moq;
using TheGentlemanLibrary.Application.Models.Users.Commands;
using TheGentlemanLibrary.Application.Models.Users.Handlers.TheGentlemanLibrary.Application.Models.Users.Handlers;
using TheGentlemanLibrary.Application.Models.Users.Interfaces;
using TheGentlemanLibrary.Application.Models.Users.JWT;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibraryTest.Users
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IOptions<JWTConfiguration>> _mockOptions;
        private readonly Mock<IValidator<RegisterCommand>> _mockValidator;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockOptions = new Mock<IOptions<JWTConfiguration>>();
            _mockValidator = new Mock<IValidator<RegisterCommand>>();
            _handler = new RegisterCommandHandler(_mockUserRepository.Object, _mockOptions.Object, null, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnJWTModel_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var command = new RegisterCommand("test@example.com", "Password123!");
            var user = new User { Id = 1, Email = "test@example.com" };

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockUserRepository.Setup(repo => repo.CreateUserAsync(command))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // Add more assertions here to check the JWT token
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenRegistrationFails()
        {
            // Arrange
            var command = new RegisterCommand("test@example.com", "Password123!");

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockUserRepository.Setup(repo => repo.CreateUserAsync(command))
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
            var command = new RegisterCommand("invalidemail", "InvalidPassword");

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Email", "Invalid email format") }));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
