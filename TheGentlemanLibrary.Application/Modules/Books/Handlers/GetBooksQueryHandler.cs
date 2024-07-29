using MediatR;
using System.Net;
using Microsoft.Extensions.Logging;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Application.Models.Books.Interfaces;
using TheGentlemanLibrary.Application.Models.Books.Queries;
using TheGentlemanLibrary.Application.Models.Books.Responses;
using TheGentlemanLibrary.Application.Models.BaseModels;
using Microsoft.Extensions.Caching.Hybrid;

namespace TheGentlemanLibrary.Application.Models.Books.Handlers
{
    public class GetBooksQueryHandler(
        IBookRepository bookRepository,
        ILogger<GetBooksQueryHandler> logger,
        HybridCache cache) : IRequestHandler<GetBooksQuery, ApiResponse<IEnumerable<BookResponseModel>>>
    {
        public async Task<ApiResponse<IEnumerable<BookResponseModel>>> Handle(
            GetBooksQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var value = await cache.GetOrCreateAsync(
                    "GetBooks",
                    async _ =>
                    {
                        var books = (await bookRepository.GetBooksAsync()).ToList();
                        var responseModels = books.Select(book => new BookResponseModel
                        {
                            Id = book.Id,
                            Title = book.Title,
                            Pages = book.Pages,
                            AuthorId = book.AuthorId,
                            UserId = book.UserId,
                            DateRange = book.DateRange
                        });
                        return responseModels;
                    },
                    new HybridCacheEntryOptions
                    {
                        Expiration = TimeSpan.FromMinutes(3),
                    }
                ,token:cancellationToken);

                return ApiResponse<IEnumerable<BookResponseModel>>.Success(value);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching books");
                return ApiResponse<IEnumerable<BookResponseModel>>.Fail(
                    null,
                    new List<string> { RsStrings.BooksFetchError },
                    (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
