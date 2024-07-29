using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TheGentlemanLibrary.Application.Models.Users.Handlers;
using TheGentlemanLibrary.Application.Models.Users.Interfaces;
using TheGentlemanLibrary.Application.Models.Users.Queries;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibraryTest.Users
{
    public class GetProfileQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly GetProfileQueryHandler _handler;

        public GetProfileQueryHandlerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _handler = new GetProfileQueryHandler(_mockUserRepository.Object, null);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Email = "test@example.com" };
            var query = new GetProfileQuery(userId);

            _mockUserRepository.Setup(repo => repo.GetProfile(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(userId, result.Data.Id);
            Assert.Equal("test@example.com", result.Data.Email);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResponse_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var query = new GetProfileQuery(userId);

            _mockUserRepository.Setup(repo => repo.GetProfile(userId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)result.StatusCode);
            Assert.Null(result.Data);
            Assert.Single(result.ErrorMessages);
            Assert.Equal(RsStrings.UserNotFound, result.ErrorMessages.First());
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResponse_WhenExceptionOccurs()
        {
            // Arrange
            var userId = 1;
            var query = new GetProfileQuery(userId);

            _mockUserRepository.Setup(repo => repo.GetProfile(userId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, (HttpStatusCode)result.StatusCode);
            Assert.Null(result.Data);
            Assert.Single(result.ErrorMessages);
            Assert.Equal(RsStrings.ProfileFetchError, result.ErrorMessages.First());
        }
    }
}
