using Moq;
using System.Net;
using TheGentlemanLibrary.Application.Models.Books.Handlers;
using TheGentlemanLibrary.Application.Models.Books.Interfaces;
using TheGentlemanLibrary.Application.Models.Books.Queries;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibraryTest.Books
{
    public class GetBooksQueryHandlerTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly GetBooksQueryHandler _handler;

        public GetBooksQueryHandlerTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _handler = new GetBooksQueryHandler(_mockBookRepository.Object, null, null);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenBooksAreRetrieved()
        {
            // Arrange
            var books = new List<Book>
            {
                new () { Id = 1, Title = "Book 1", Pages = 100, AuthorId = 1, UserId = 1, DateRange = "2023" },
                new () { Id = 2, Title = "Book 2", Pages = 200, AuthorId = 2, UserId = 2, DateRange = "2024" }
            };

            _mockBookRepository.Setup(repo => repo.GetBooksAsync())
                .ReturnsAsync(books);

            var query = new GetBooksQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count());

            var booksList = result.Data.ToList();
            Assert.Equal(1, booksList[0].Id);
            Assert.Equal("Book 1", booksList[0].Title);
            Assert.Equal(100, booksList[0].Pages);
            Assert.Equal(1, booksList[0].AuthorId);
            Assert.Equal(1, booksList[0].UserId);
            Assert.Equal("2023", booksList[0].DateRange);

            Assert.Equal(2, booksList[1].Id);
            Assert.Equal("Book 2", booksList[1].Title);
            Assert.Equal(200, booksList[1].Pages);
            Assert.Equal(2, booksList[1].AuthorId);
            Assert.Equal(2, booksList[1].UserId);
            Assert.Equal("2024", booksList[1].DateRange);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResponse_WhenExceptionOccurs()
        {
            // Arrange
            _mockBookRepository.Setup(repo => repo.GetBooksAsync())
                .ThrowsAsync(new Exception("Test exception"));

            var query = new GetBooksQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, (HttpStatusCode)result.StatusCode);
            Assert.Null(result.Data);
            Assert.Single(result.ErrorMessages);
            Assert.Contains(RsStrings.BooksFetchError, result.ErrorMessages);
        }
    }
}