using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using TheGentlemanLibrary.Application.Models.Authors.Handlers;
using TheGentlemanLibrary.Application.Models.Authors.Interfaces;
using TheGentlemanLibrary.Application.Models.Authors.Queries;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibrary.Tests.Application.Authors
{
    public class GetAuthorByIdQueryHandlerTests
    {
        private readonly Mock<IAuthorRepository> _mockAuthorRepository;
        private readonly Mock<ILogger<GetAuthorByIdQueryHandler>> _mockLogger;
        private readonly GetAuthorByIdQueryHandler _handler;

        public GetAuthorByIdQueryHandlerTests()
        {
            _mockAuthorRepository = new Mock<IAuthorRepository>();
            _mockLogger = new Mock<ILogger<GetAuthorByIdQueryHandler>>();
            _handler = new GetAuthorByIdQueryHandler(_mockAuthorRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenAuthorExists()
        {
            // Arrange
            var authorId = 1;
            var query = new GetAuthorByIdQuery(authorId);
            var author = new Author
            {
                Id = authorId,
                Name = "John Doe",
                Biography = "A great author",
                DateRange = "1980-2024"
            };

            _mockAuthorRepository.Setup(repo => repo.GetAuthorByIdAsync(authorId))
                .ReturnsAsync(author);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(author.Id, result.Data.Id);
            Assert.Equal(author.Name, result.Data.Name);
            Assert.Equal(author.Biography, result.Data.Biography);
            Assert.Equal(author.DateRange, result.Data.DateRange);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenAuthorDoesNotExist()
        {
            // Arrange
            var authorId = 1;
            var query = new GetAuthorByIdQuery(authorId);

            _mockAuthorRepository.Setup(repo => repo.GetAuthorByIdAsync(authorId))
                .ReturnsAsync((Author)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Null(result.Data);
            Assert.Contains(RsStrings.AuthorFetchError, result.ErrorMessages);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var authorId = 1;
            var query = new GetAuthorByIdQuery(authorId);

            _mockAuthorRepository.Setup(repo => repo.GetAuthorByIdAsync(authorId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Null(result.Data);
            Assert.Contains(RsStrings.AuthorFetchError, result.ErrorMessages);
        }
    }
}