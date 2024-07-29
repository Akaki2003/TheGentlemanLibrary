using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Books.Commands;
using TheGentlemanLibrary.Application.Models.Books.Handlers;
using TheGentlemanLibrary.Application.Models.Books.Interfaces;
using TheGentlemanLibrary.Application.Models.Books.Responses;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Domain.Entities;
using Xunit;

namespace TheGentlemanLibrary.Application.Tests.Models.Books.Handlers
{
    public class CreateBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<ILogger<CreateBookCommandHandler>> _loggerMock;
        private readonly CreateBookCommandHandler _handler;

        public CreateBookCommandHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _loggerMock = new Mock<ILogger<CreateBookCommandHandler>>();
            _handler = new CreateBookCommandHandler(_bookRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenBookIsCreated()
        {
            // Arrange
            var command = new CreateBookCommand(1, "Title", 200, 1, "2000-2020") { UserId = 1 };
            var book = new Book { Id = 1, Title = "Title", Pages = 200, AuthorId = 1, UserId = 1, DateRange = "2000-2020" };

            _bookRepositoryMock.Setup(repo => repo.CreateBookAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResponse_WhenBookCreationFails()
        {
            // Arrange
            var command = new CreateBookCommand(1, "Title", 200, 1, "2000-2020") { UserId = 1 };

            _bookRepositoryMock.Setup(repo => repo.CreateBookAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>())).ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResponse_WhenExceptionIsThrown()
        {
            // Arrange
            var command = new CreateBookCommand(1, "Title", 200, 1, "2000-2020") { UserId = 1 };

            _bookRepositoryMock.Setup(repo => repo.CreateBookAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenTitleIsEmpty()
        {
            var validator = new CreateAuthorCommandValidator();
            var command = new CreateBookCommand(1, "", 200, 1, "2000-2020");
            var result = validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Title);
        }

        [Fact]
        public void Validate_ShouldNotHaveError_WhenTitleIsNotEmpty()
        {
            var validator = new CreateAuthorCommandValidator();
            var command = new CreateBookCommand(1, "Title", 200, 1, "2000-2020");
            var result = validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(c => c.Title);
        }
    }
}
