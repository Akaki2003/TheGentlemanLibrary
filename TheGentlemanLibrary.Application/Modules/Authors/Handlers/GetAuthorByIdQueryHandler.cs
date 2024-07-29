using MediatR;
using System.Net;
using Microsoft.Extensions.Logging;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Application.Models.Authors.Interfaces;
using TheGentlemanLibrary.Application.Models.Authors.Queries;
using TheGentlemanLibrary.Application.Models.Authors.Responses;
using TheGentlemanLibrary.Application.Models.BaseModels;

namespace TheGentlemanLibrary.Application.Models.Authors.Handlers
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, ApiResponse<AuthorResponseModel>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<GetAuthorByIdQueryHandler> _logger;

        public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository, ILogger<GetAuthorByIdQueryHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<AuthorResponseModel>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var author = await _authorRepository.GetAuthorByIdAsync(request.ID);
                if (author == null)
                {
                    return ApiResponse<AuthorResponseModel>.Fail(null, new List<string> { RsStrings.AuthorFetchError }, (int)HttpStatusCode.NotFound);
                }

                var result = new AuthorResponseModel
                {
                    Id = author.Id,
                    Biography = author.Biography,
                    Name = author.Name,
                    DateRange = author.DateRange
                };

                return ApiResponse<AuthorResponseModel>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the author");
                return ApiResponse<AuthorResponseModel>.Fail(null, new List<string> { RsStrings.AuthorFetchError }, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
