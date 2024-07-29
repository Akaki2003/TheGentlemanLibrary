using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using TheGentlemanLibrary.Application.Models.Authors.Commands;
using TheGentlemanLibrary.Application.Models.Authors.Interfaces;
using TheGentlemanLibrary.Application.Models.Authors.Responses;
using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Common.Resources;

namespace TheGentlemanLibrary.Application.Models.Authors.Handlers
{
    public class CreateAuthorCommandHandler(IAuthorRepository authorRepository, ILogger<CreateAuthorCommandHandler> logger) : IRequestHandler<CreateAuthorCommand, ApiResponse<AuthorResponseModel>>
    {
        private readonly IAuthorRepository _authorRepository = authorRepository;
        private readonly ILogger<CreateAuthorCommandHandler> _logger = logger;

        public async Task<ApiResponse<AuthorResponseModel>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var createdAuthor = await _authorRepository.CreateAuthorAsync(request);
                if (createdAuthor == null)
                {
                    return ApiResponse<AuthorResponseModel>.Fail(null, new List<string> { RsStrings.AuthorNotFound }, (int)HttpStatusCode.NotFound);
                }

                var result = new AuthorResponseModel
                {
                    Id = createdAuthor.Id,
                    Biography = createdAuthor.Biography,
                    Name = createdAuthor.Name,
                    DateRange = createdAuthor.DateRange,
                    Country = createdAuthor.Country
                };

                return ApiResponse<AuthorResponseModel>.Success(result, (int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the author");
                return ApiResponse<AuthorResponseModel>.Fail(null, new List<string> { RsStrings.AuthorCreationError }, (int)HttpStatusCode.InternalServerError);
            }
        }
    }

    public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Author name must not be empty.");
        }
    }
}
