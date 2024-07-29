using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Books.Commands;
using TheGentlemanLibrary.Application.Models.Books.Interfaces;
using TheGentlemanLibrary.Application.Models.Books.Responses;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibrary.Application.Models.Books.Handlers
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, ApiResponse<BookResponseModel>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<CreateBookCommandHandler> _logger;

        public CreateBookCommandHandler(IBookRepository bookRepository, ILogger<CreateBookCommandHandler> logger)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResponse<BookResponseModel>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var book = new Book
                {
                    Title = request.Title,
                    Pages = request.Pages,
                    AuthorId = request.AuthorId,
                    UserId = request.UserId,
                    DateRange = request.DateRange
                };

                var createdBook = await _bookRepository.CreateBookAsync(book, cancellationToken);

                if (createdBook == null)
                {
                    return ApiResponse<BookResponseModel>.Fail(null, new List<string> { RsStrings.BookCreationError }, (int)HttpStatusCode.BadRequest);
                }

                var responseModel = new BookResponseModel
                {
                    Id = createdBook.Id,
                    Title = createdBook.Title,
                    Pages = createdBook.Pages,
                    AuthorId = createdBook.AuthorId,
                    UserId = createdBook.UserId,
                    DateRange = createdBook.DateRange
                };

                return ApiResponse<BookResponseModel>.Success(responseModel, (int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the book");
                return ApiResponse<BookResponseModel>.Fail(null, new List<string> { RsStrings.BookCreationFailed }, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
    public class CreateAuthorCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Book title must not be empty.");
        }
    }
}
