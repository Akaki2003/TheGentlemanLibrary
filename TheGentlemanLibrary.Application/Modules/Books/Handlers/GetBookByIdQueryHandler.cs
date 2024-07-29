using MediatR;
using System.Net;
using Microsoft.Extensions.Logging;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Application.Models.Books.Interfaces;
using TheGentlemanLibrary.Application.Models.Books.Queries;
using TheGentlemanLibrary.Application.Models.Books.Responses;
using TheGentlemanLibrary.Application.Models.BaseModels;

namespace TheGentlemanLibrary.Application.Models.Books.Handlers
{
    public class GetBookByIdQueryHandler(IBookRepository bookRepository, ILogger<GetBookByIdQueryHandler> logger) : IRequestHandler<GetBookByIdQuery, ApiResponse<BookResponseModel>>
    {
        private readonly IBookRepository _bookRepository = bookRepository;
        private readonly ILogger<GetBookByIdQueryHandler> _logger = logger;

        public async Task<ApiResponse<BookResponseModel>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(request.ID, cancellationToken);
                if (book == null)
                {
                    return ApiResponse<BookResponseModel>.Fail(null, new List<string> { RsStrings.BookFetchError }, (int)HttpStatusCode.NotFound);
                }

                var responseModel = new BookResponseModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Pages = book.Pages,
                    AuthorId = book.AuthorId,
                    UserId = book.UserId,
                    DateRange = book.DateRange
                };

                return ApiResponse<BookResponseModel>.Success(responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the book");
                return ApiResponse<BookResponseModel>.Fail(null, new List<string> { RsStrings.BookFetchError }, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
