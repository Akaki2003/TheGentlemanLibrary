using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using TheGentlemanLibrary.Application.Models.Books.Handlers;
using TheGentlemanLibrary.Application.Models.Books.Interfaces;
using TheGentlemanLibrary.Application.Models.Books.Queries;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibrary.Application.Tests.Models.Books.Handlers
{
    public class GetBookByIdQueryHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<ILogger<GetBookByIdQueryHandler>> _loggerMock;
        private readonly GetBookByIdQueryHandler _handler;

        public GetBookByIdQueryHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _loggerMock = new Mock<ILogger<GetBookByIdQueryHandler>>();
            _handler = new GetBookByIdQueryHandler(_bookRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenBookIsFound()
        {
            // Arrange
            var query = new GetBookByIdQuery(1);
            var book = new Book { Id = 1, Title = "Title", Pages = 200, AuthorId = 1, UserId = 1, DateRange = "2000-2020" };

            _bookRepositoryMock.Setup(repo => repo.GetBookByIdAsync(query.ID, It.IsAny<CancellationToken>())).ReturnsAsync(book);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResponse_WhenBookIsNotFound()
        {
            // Arrange
            var query = new GetBookByIdQuery(1);

            _bookRepositoryMock.Setup(repo => repo.GetBookByIdAsync(query.ID, It.IsAny<CancellationToken>())).ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResponse_WhenExceptionIsThrown()
        {
            // Arrange
            var query = new GetBookByIdQuery(1);

            _bookRepositoryMock.Setup(repo => repo.GetBookByIdAsync(query.ID, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
        }
    }
}
