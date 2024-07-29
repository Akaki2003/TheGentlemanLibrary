using MediatR;
using System.Net;
using Microsoft.Extensions.Logging;
using TheGentlemanLibrary.Common.Resources;
using FluentValidation;
using TheGentlemanLibrary.Application.Models.Authors.Commands;
using TheGentlemanLibrary.Application.Models.Authors.Interfaces;
using TheGentlemanLibrary.Application.Models.BaseModels;

namespace TheGentlemanLibrary.Application.Models.Authors.Handlers
{
    public class EditAuthorCommandHandler(IAuthorRepository authorRepository, ILogger<EditAuthorCommandHandler> logger) : IRequestHandler<EditAuthorCommand, ApiResponse<bool>>
    {
        public async Task<ApiResponse<bool>> Handle(EditAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await authorRepository.EditAuthor(request);
                if (result)
                {
                    return ApiResponse<bool>.Success(true, (int)HttpStatusCode.OK);
                }
                else
                {
                    return ApiResponse<bool>.Fail(false, new List<string> { RsStrings.AuthorEditError }, (int)HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while editing the author");
                return ApiResponse<bool>.Fail(false, new List<string> { RsStrings.AuthorEditError }, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
    public class EditAuthorCommandValidator : AbstractValidator<EditAuthorCommand>
    {
        public EditAuthorCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Author name must not be empty.");
        }
    }
}
