using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using TheGentlemanLibrary.Application.Models.Authors.Commands;
using TheGentlemanLibrary.Application.Models.Authors.Handlers;
using TheGentlemanLibrary.Application.Models.Authors.Interfaces;
using TheGentlemanLibrary.Common.Resources;

namespace TheGentlemanLibrary.Tests.Application.Authors
{
    public class EditAuthorCommandHandlerTests
    {
        private readonly Mock<IAuthorRepository> _mockAuthorRepository;
        private readonly Mock<ILogger<EditAuthorCommandHandler>> _mockLogger;
        private readonly EditAuthorCommandHandler _handler;
        private readonly EditAuthorCommandValidator _validator;

        public EditAuthorCommandHandlerTests()
        {
            _mockAuthorRepository = new Mock<IAuthorRepository>();
            _mockLogger = new Mock<ILogger<EditAuthorCommandHandler>>();
            _handler = new EditAuthorCommandHandler(_mockAuthorRepository.Object, _mockLogger.Object);
            _validator = new EditAuthorCommandValidator();
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenAuthorIsEditedSuccessfully()
        {
            // Arrange
            var command = new EditAuthorCommand(1, new DateTime(1980, 1, 1), "USA", "John Doe", "Updated biography", "1980-2024");
            _mockAuthorRepository.Setup(repo => repo.EditAuthor(command)).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenAuthorEditFails()
        {
            // Arrange
            var command = new EditAuthorCommand(1, new DateTime(1980, 1, 1), "USA", "John Doe", "Updated biography", "1980-2024");
            _mockAuthorRepository.Setup(repo => repo.EditAuthor(command)).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.False(result.Data);
            Assert.Contains(RsStrings.AuthorEditError, result.ErrorMessages);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var command = new EditAuthorCommand(1, new DateTime(1980, 1, 1), "USA", "John Doe", "Updated biography", "1980-2024");
            _mockAuthorRepository.Setup(repo => repo.EditAuthor(command)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.False(result.Data);
            Assert.Contains(RsStrings.AuthorEditError, result.ErrorMessages);
        }

        [Fact]
        public void Validator_ShouldHaveErrorForEmptyName()
        {
            // Arrange
            var command = new EditAuthorCommand(1, new DateTime(1980, 1, 1), "USA", "", "Updated biography", "1980-2024");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Author name must not be empty.");
        }

        [Fact]
        public void Validator_ShouldNotHaveErrorForValidName()
        {
            // Arrange
            var command = new EditAuthorCommand(1, new DateTime(1980, 1, 1), "USA", "John Doe", "Updated biography", "1980-2024");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validator_ShouldAllowNullBornDate()
        {
            // Arrange
            var command = new EditAuthorCommand(1, null, "USA", "John Doe", "Updated biography", "1980-2024");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Born);
        }
    }
}