using MediatR;
using System.Net;
using Microsoft.Extensions.Logging;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Application.Models.Authors.Interfaces;
using TheGentlemanLibrary.Application.Models.Authors.Queries;
using TheGentlemanLibrary.Application.Models.Authors.Responses;
using TheGentlemanLibrary.Application.Models.BaseModels;
using Microsoft.Extensions.Caching.Hybrid;

namespace TheGentlemanLibrary.Application.Models.Authors.Handlers
{
    public class GetAuthorsQueryHandler(
        IAuthorRepository authorRepository,
        ILogger<GetAuthorsQueryHandler> logger,
        HybridCache cache) : IRequestHandler<GetAuthorsQuery, ApiResponse<List<AuthorResponseModel>>>
    {
        public async Task<ApiResponse<List<AuthorResponseModel>>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var authors = await cache.GetOrCreateAsync(
                    "GetAuthors",
                    async _ =>
                    {
                        var authorsList = (await authorRepository.GetAuthorsAsync()).ToList();
                        return authorsList.Select(x => new AuthorResponseModel
                        {
                            Id = x.Id,
                            Biography = x.Biography,
                            Name = x.Name,
                            DateRange = x.DateRange
                        }).ToList();
                    },
                    new HybridCacheEntryOptions
                    {
                        Expiration = TimeSpan.FromMinutes(5),
                    },
                    token: cancellationToken);

                return ApiResponse<List<AuthorResponseModel>>.Success(authors);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching authors");
                return ApiResponse<List<AuthorResponseModel>>.Fail(null, new List<string> { RsStrings.AuthorsFetchError }, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}