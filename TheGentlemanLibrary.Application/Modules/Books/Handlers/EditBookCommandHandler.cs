using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Books.Commands;
using TheGentlemanLibrary.Application.Models.Books.Interfaces;
using TheGentlemanLibrary.Common.Resources;

namespace TheGentlemanLibrary.Application.Models.Books.Handlers
{
    public class EditBookCommandHandler(IBookRepository bookRepository, ILogger<EditBookCommandHandler> logger) : IRequestHandler<EditBookCommand, ApiResponse<bool>>
    {
        private readonly IBookRepository _bookRepository = bookRepository;
        private readonly ILogger<EditBookCommandHandler> _logger = logger;

        public async Task<ApiResponse<bool>> Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingBook = await _bookRepository.GetBookByIdAsync(request.Id, cancellationToken);
                if (existingBook == null)
                {
                    return ApiResponse<bool>.Fail(false, new List<string> { RsStrings.BookFetchError }, (int)HttpStatusCode.NotFound);
                }

                existingBook.Title = request.Title;
                existingBook.Pages = request.Pages;
                existingBook.AuthorId = request.AuthorId;
                existingBook.UserId = request.UserId;
                existingBook.DateRange = request.DateRange;

                var result = await _bookRepository.EditBook(existingBook, cancellationToken);
                if (result)
                {
                    return ApiResponse<bool>.Success(true);
                }
                else
                {
                    return ApiResponse<bool>.Fail(false, new List<string> { RsStrings.BookEditFailed }, (int)HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the book");
                return ApiResponse<bool>.Fail(false, new List<string> { RsStrings.BookEditError }, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
    public class EditAuthorCommandValidator : AbstractValidator<EditBookCommand>
    {
        public EditAuthorCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Book title must not be empty.");
        }
    }
}
