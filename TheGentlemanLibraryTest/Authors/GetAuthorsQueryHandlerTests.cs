using Microsoft.Extensions.Caching.Hybrid;
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
    public class GetAuthorsQueryHandlerTests
    {
        private readonly Mock<IAuthorRepository> _mockAuthorRepository;
        private readonly Mock<ILogger<GetAuthorsQueryHandler>> _mockLogger;
        private readonly Mock<HybridCache> _mockCache;
        private readonly GetAuthorsQueryHandler _handler;

        public GetAuthorsQueryHandlerTests()
        {
            _mockAuthorRepository = new Mock<IAuthorRepository>();
            _mockLogger = new Mock<ILogger<GetAuthorsQueryHandler>>();
            _mockCache = new Mock<HybridCache>();
            _handler = new GetAuthorsQueryHandler(_mockAuthorRepository.Object, _mockLogger.Object, _mockCache.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenAuthorsAreRetrieved()
        {
            // Arrange
            var query = new GetAuthorsQuery();
            var authors = new List<Author>
            {
                new Author { Id = 1, Name = "John Doe", Biography = "A great author", DateRange = "1980-2024" },
                new Author { Id = 2, Name = "Jane Smith", Biography = "Another great author", DateRange = "1985-2023" }
            };

            _mockAuthorRepository.Setup(repo => repo.GetAuthorsAsync())
                .ReturnsAsync(authors);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal(1, result.Data[0].Id);
            Assert.Equal("John Doe", result.Data[0].Name);
            Assert.Equal("A great author", result.Data[0].Biography);
            Assert.Equal("1980-2024", result.Data[0].DateRange);
            Assert.Equal(2, result.Data[1].Id);
            Assert.Equal("Jane Smith", result.Data[1].Name);
            Assert.Equal("Another great author", result.Data[1].Biography);
            Assert.Equal("1985-2023", result.Data[1].DateRange);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoAuthorsExist()
        {
            // Arrange
            var query = new GetAuthorsQuery();

            _mockAuthorRepository.Setup(repo => repo.GetAuthorsAsync())
                .ReturnsAsync(new List<Author>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var query = new GetAuthorsQuery();

            _mockAuthorRepository.Setup(repo => repo.GetAuthorsAsync())
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Null(result.Data);
            Assert.Contains(RsStrings.AuthorsFetchError, result.ErrorMessages);
        }
    }
}