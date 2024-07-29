using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using TheGentlemanLibrary.Application.Models.Books.Commands;
using TheGentlemanLibrary.Application.Models.Books.Handlers;
using TheGentlemanLibrary.Application.Models.Books.Interfaces;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibrary.Application.Tests.Models.Books.Handlers
{
    public class EditBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<ILogger<EditBookCommandHandler>> _loggerMock;
        private readonly EditBookCommandHandler _handler;

        public EditBookCommandHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _loggerMock = new Mock<ILogger<EditBookCommandHandler>>();
            _handler = new EditBookCommandHandler(_bookRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenBookIsEdited()
        {
            // Arrange
            var command = new EditBookCommand(1, "New Title", 200, 1, "2000-2020") { UserId = 1 };
            var book = new Book { Id = 1, Title = "Old Title", Pages = 100, AuthorId = 1, UserId = 1, DateRange = "1980-2000" };

            _bookRepositoryMock.Setup(repo => repo.GetBookByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(book);
            _bookRepositoryMock.Setup(repo => repo.EditBook(It.IsAny<Book>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResponse_WhenBookNotFound()
        {
            // Arrange
            var command = new EditBookCommand(1, "New Title", 200, 1, "2000-2020") { UserId = 1 };

            _bookRepositoryMock.Setup(repo => repo.GetBookByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResponse_WhenEditFails()
        {
            // Arrange
            var command = new EditBookCommand(1, "New Title", 200, 1, "2000-2020") { UserId = 1 };
            var book = new Book { Id = 1, Title = "Old Title", Pages = 100, AuthorId = 1, UserId = 1, DateRange = "1980-2000" };

            _bookRepositoryMock.Setup(repo => repo.GetBookByIdAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(book);
            _bookRepositoryMock.Setup(repo => repo.EditBook(It.IsAny<Book>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResponse_WhenExceptionIsThrown()
        {
            // Arrange
            var command = new EditBookCommand(1, "New Title", 200, 1, "2000-2020") { UserId = 1 };

            _bookRepositoryMock.Setup(repo => repo.GetBookByIdAsync(command.Id, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenTitleIsEmpty()
        {
            var validator = new EditAuthorCommandValidator();
            var command = new EditBookCommand(1, "", 200, 1, "2000-2020");
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Title);
        }

        [Fact]
        public void Validate_ShouldNotHaveError_WhenTitleIsNotEmpty()
        {
            var validator = new EditAuthorCommandValidator();
            var command = new EditBookCommand(1, "Title", 200, 1, "2000-2020");
            var result = validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(c => c.Title);
        }
    }
}
