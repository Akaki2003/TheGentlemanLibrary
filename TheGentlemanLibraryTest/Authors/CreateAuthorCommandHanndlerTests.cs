using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using TheGentlemanLibrary.Application.Models.Authors.Commands;
using TheGentlemanLibrary.Application.Models.Authors.Handlers;
using TheGentlemanLibrary.Application.Models.Authors.Interfaces;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibrary.Tests.Application.Authors
{
    public class CreateAuthorCommandHanndlerTests
    {
        private readonly Mock<IAuthorRepository> _mockAuthorRepository;
        private readonly Mock<ILogger<CreateAuthorCommandHandler>> _mockLogger;
        private readonly CreateAuthorCommandHandler _handler;
        private readonly CreateAuthorCommandValidator _validator;

        public CreateAuthorCommandHanndlerTests()
        {
            _mockAuthorRepository = new Mock<IAuthorRepository>();
            _mockLogger = new Mock<ILogger<CreateAuthorCommandHandler>>();
            _handler = new CreateAuthorCommandHandler(_mockAuthorRepository.Object, _mockLogger.Object);
            _validator = new CreateAuthorCommandValidator();
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenAuthorIsCreatedSuccessfully()
        {
            // Arrange
            var command = new CreateAuthorCommand(1, "USA", "John Doe", "A great author", "1980-2024");

            var createdAuthor = new Author
            {
                Id = command.Id,
                Country = command.Country,
                Name = command.Name,
                Biography = command.Biography,
                DateRange = command.DateRange,
                CreatedAt = DateTime.UtcNow
            };

            _mockAuthorRepository.Setup(repo => repo.CreateAuthorAsync(command))
                .ReturnsAsync(createdAuthor);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);
            Assert.Equal(createdAuthor.Id, result.Data.Id);
            Assert.Equal(createdAuthor.Country, result.Data.Country);
            Assert.Equal(createdAuthor.Name, result.Data.Name);
            Assert.Equal(createdAuthor.Biography, result.Data.Biography);
            Assert.Equal(createdAuthor.DateRange, result.Data.DateRange);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenAuthorCreationFails()
        {
            // Arrange
            var command = new CreateAuthorCommand(1, "USA", "John Doe", "A great author", "1980-2024");

            _mockAuthorRepository.Setup(repo => repo.CreateAuthorAsync(command))
                .ReturnsAsync((Author)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains(RsStrings.AuthorNotFound, result.ErrorMessages);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var command = new CreateAuthorCommand(1, "USA", "John Doe", "A great author", "1980-2024");

            _mockAuthorRepository.Setup(repo => repo.CreateAuthorAsync(command))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains(RsStrings.AuthorCreationError, result.ErrorMessages);
        }

        [Fact]
        public void Validator_ShouldHaveErrorForEmptyName()
        {
            // Arrange
            var command = new CreateAuthorCommand(1, "USA", "", "A great author", "1980-2024");

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
            var command = new CreateAuthorCommand(1, "USA", "John Doe", "A great author", "1980-2024");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }
    }
}