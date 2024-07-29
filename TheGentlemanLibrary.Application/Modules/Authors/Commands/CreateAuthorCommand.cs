using MediatR;
using TheGentlemanLibrary.Application.Models.Authors.Responses;
using TheGentlemanLibrary.Application.Models.BaseModels;

namespace TheGentlemanLibrary.Application.Models.Authors.Commands
{
    public record CreateAuthorCommand(int Id, string Country, string? Name, string? Biography, string? DateRange) : IRequest<ApiResponse<AuthorResponseModel>>;
}
