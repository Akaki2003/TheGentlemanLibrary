using Moq;
using System.Net;
using TheGentlemanLibrary.Application.Models.Orders.Handlers;
using TheGentlemanLibrary.Application.Models.Orders.Interfaces;
using TheGentlemanLibrary.Application.Models.Orders.Queries;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibraryTest.Orders
{
    public class GetOrdersQueryHandlerTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly GetOrdersQueryHandler _handler;

        public GetOrdersQueryHandlerTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _handler = new GetOrdersQueryHandler(_mockOrderRepository.Object, null);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenOrdersExist()
        {
            // Arrange
            var orders = new List<Order>
            {
                new () { UserId = 1, BookId = 1, Price = 19.99m },
                new () { UserId = 2, BookId = 2, Price = 29.99m }
            };

            _mockOrderRepository.Setup(repo => repo.GetOrdersAsync())
                .ReturnsAsync(orders);

            var query = new GetOrdersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count());

            var ordersList = result.Data.ToList();
            Assert.Equal(1, ordersList[0].UserId);
            Assert.Equal(1, ordersList[0].BookId);
            Assert.Equal(19.99m, ordersList[0].Price);

            Assert.Equal(2, ordersList[1].UserId);
            Assert.Equal(2, ordersList[1].BookId);
            Assert.Equal(29.99m, ordersList[1].Price);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenNoOrdersExist()
        {
            // Arrange
            var orders = new List<Order>();

            _mockOrderRepository.Setup(repo => repo.GetOrdersAsync())
                .ReturnsAsync(orders);

            var query = new GetOrdersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResponse_WhenExceptionOccurs()
        {
            // Arrange
            _mockOrderRepository.Setup(repo => repo.GetOrdersAsync())
                .ThrowsAsync(new Exception("Test exception"));

            var query = new GetOrdersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, (HttpStatusCode)result.StatusCode);
            Assert.Null(result.Data);
            Assert.Single(result.ErrorMessages);
            Assert.Equal(RsStrings.OrdersFetchError, result.ErrorMessages.First());
        }
    }
}
