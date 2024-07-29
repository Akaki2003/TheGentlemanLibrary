using Moq;
using System.Net;
using TheGentlemanLibrary.Application.Models.Orders.Commands;
using TheGentlemanLibrary.Application.Models.Orders.Handlers;
using TheGentlemanLibrary.Application.Models.Orders.Interfaces;
using TheGentlemanLibrary.Common.Resources;

namespace TheGentlemanLibraryTest.Orders
{
    public class EditOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly EditOrderCommandHandler _handler;

        public EditOrderCommandHandlerTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _handler = new EditOrderCommandHandler(_mockOrderRepository.Object, null);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenOrderIsEdited()
        {
            // Arrange
            var command = new EditOrderCommand(1, 1, 1, 29.99m);

            _mockOrderRepository.Setup(repo => repo.EditOrder(It.IsAny<EditOrderCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResponse_WhenOrderEditFails()
        {
            // Arrange
            var command = new EditOrderCommand(1, 1, 1, 29.99m);

            _mockOrderRepository.Setup(repo => repo.EditOrder(It.IsAny<EditOrderCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)result.StatusCode);
            Assert.False(result.Data);
            Assert.Single(result.ErrorMessages);
            Assert.Equal(RsStrings.OrderEditError, result.ErrorMessages.First());
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResponse_WhenExceptionOccurs()
        {
            // Arrange
            var command = new EditOrderCommand(1, 1, 1, 29.99m);

            _mockOrderRepository.Setup(repo => repo.EditOrder(It.IsAny<EditOrderCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, (HttpStatusCode)result.StatusCode);
            Assert.False(result.Data);
            Assert.Single(result.ErrorMessages);
            Assert.Equal(RsStrings.OrderEditError, result.ErrorMessages.First());
        }
    }

    public class EditOrderCommandValidatorTests
    {
        private readonly EditOrderCommandValidator _validator;

        public EditOrderCommandValidatorTests()
        {
            _validator = new EditOrderCommandValidator();
        }

        [Theory]
        [InlineData(0, 1, 1, 29.99)]
        [InlineData(1, 0, 1, 29.99)]
        [InlineData(1, 1, 0, 29.99)]
        [InlineData(1, 1, 1, 0)]
        [InlineData(1, 1, 1, -1)]
        public void Validate_ShouldFailValidation_WhenInvalidDataProvided(int id, int userId, int bookId, decimal price)
        {
            // Arrange
            var command = new EditOrderCommand(id, userId, bookId, price);

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_ShouldPassValidation_WhenValidDataProvided()
        {
            // Arrange
            var command = new EditOrderCommand(1, 1, 1, 29.99m);

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
