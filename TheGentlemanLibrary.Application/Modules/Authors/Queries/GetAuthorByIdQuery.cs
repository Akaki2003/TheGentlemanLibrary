using MediatR;
using TheGentlemanLibrary.Application.Models.Authors.Responses;
using TheGentlemanLibrary.Application.Models.BaseModels;

namespace TheGentlemanLibrary.Application.Models.Authors.Queries
{
    public record GetAuthorByIdQuery(int ID) : IRequest<ApiResponse<AuthorResponseModel>>;
}
