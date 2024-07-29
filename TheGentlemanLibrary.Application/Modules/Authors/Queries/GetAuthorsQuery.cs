using MediatR;
using TheGentlemanLibrary.Application.Models.Authors.Responses;
using TheGentlemanLibrary.Application.Models.BaseModels;

namespace TheGentlemanLibrary.Application.Models.Authors.Queries
{
    public record GetAuthorsQuery : IRequest<ApiResponse<List<AuthorResponseModel>>>;
}
