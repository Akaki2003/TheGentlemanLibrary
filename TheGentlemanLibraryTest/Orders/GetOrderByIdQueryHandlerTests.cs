using Moq;
using System.Net;
using TheGentlemanLibrary.Application.Models.Orders.Handlers;
using TheGentlemanLibrary.Application.Models.Orders.Interfaces;
using TheGentlemanLibrary.Application.Models.Orders.Queries;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibraryTest.Orders
{
    public class GetOrderByIdQueryHandlerTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly GetOrderByIdQueryHandler _handler;

        public GetOrderByIdQueryHandlerTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _handler = new GetOrderByIdQueryHandler(_mockOrderRepository.Object, null);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { UserId = 1, BookId = 1, Price = 19.99m };
            var query = new GetOrderByIdQuery(orderId);

            _mockOrderRepository.Setup(repo => repo.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.UserId);
            Assert.Equal(1, result.Data.BookId);
            Assert.Equal(19.99m, result.Data.Price);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResponse_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = 1;
            var query = new GetOrderByIdQuery(orderId);

            _mockOrderRepository.Setup(repo => repo.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)result.StatusCode);
            Assert.Null(result.Data);
            Assert.Single(result.ErrorMessages);
            Assert.Equal(RsStrings.OrderNotFound, result.ErrorMessages.First());
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResponse_WhenExceptionOccurs()
        {
            // Arrange
            var orderId = 1;
            var query = new GetOrderByIdQuery(orderId);

            _mockOrderRepository.Setup(repo => repo.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, (HttpStatusCode)result.StatusCode);
            Assert.Null(result.Data);
            Assert.Single(result.ErrorMessages);
            Assert.Equal(RsStrings.OrderFetchError, result.ErrorMessages.First());
        }
    }
}
