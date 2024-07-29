using Moq;
using System.Net;
using TheGentlemanLibrary.Application.Models.Orders.Commands;
using TheGentlemanLibrary.Application.Models.Orders.Handlers;
using TheGentlemanLibrary.Application.Models.Orders.Interfaces;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibraryTest.Orders
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _handler = new CreateOrderCommandHandler(_mockOrderRepository.Object, null);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenOrderIsCreated()
        {
            // Arrange
            var command = new CreateOrderCommand(1, 1, 1, 19.99m);
            var createdOrder = new Order { UserId = 1, BookId = 1, Price = 19.99m };

            _mockOrderRepository.Setup(repo => repo.CreateOrderAsync(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdOrder);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.Created, (HttpStatusCode)result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.UserId);
            Assert.Equal(1, result.Data.BookId);
            Assert.Equal(19.99m, result.Data.Price);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResponse_WhenOrderCreationFails()
        {
            // Arrange
            var command = new CreateOrderCommand(1, 1, 1, 19.99m);

            _mockOrderRepository.Setup(repo => repo.CreateOrderAsync(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)result.StatusCode);
            Assert.Null(result.Data);
            Assert.Single(result.ErrorMessages);
            Assert.Contains(RsStrings.OrderCreationError, result.ErrorMessages);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResponse_WhenExceptionOccurs()
        {
            // Arrange
            var command = new CreateOrderCommand(1, 1, 1, 19.99m);

            _mockOrderRepository.Setup(repo => repo.CreateOrderAsync(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, (HttpStatusCode)result.StatusCode);
            Assert.Null(result.Data);
            Assert.Single(result.ErrorMessages);
            Assert.Contains(RsStrings.OrderCreationError, result.ErrorMessages);
        }
    }

    public class CreateOrderCommandValidatorTests
    {
        private readonly CreateOrderCommandValidator _validator;

        public CreateOrderCommandValidatorTests()
        {
            _validator = new CreateOrderCommandValidator();
        }

        [Theory]
        [InlineData(0, 1, 1, 19.99)]
        [InlineData(1, 0, 1, 19.99)]
        [InlineData(1, 1, 0, 19.99)]
        [InlineData(1, 1, 1, 0)]
        [InlineData(1, 1, 1, -1)]
        public void Validate_ShouldFailValidation_WhenInvalidDataProvided(int id, int userId, int bookId, decimal price)
        {
            // Arrange
            var command = new CreateOrderCommand(id, userId, bookId, price);

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_ShouldPassValidation_WhenValidDataProvided()
        {
            // Arrange
            var command = new CreateOrderCommand(1, 1, 1, 19.99m);

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
