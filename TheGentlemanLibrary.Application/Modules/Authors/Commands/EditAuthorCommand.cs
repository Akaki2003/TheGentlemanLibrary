using MediatR;
using TheGentlemanLibrary.Application.Models.BaseModels;

namespace TheGentlemanLibrary.Application.Models.Authors.Commands
{
    public record EditAuthorCommand(int Id, DateTime? Born, string Country, string? Name, string? Biography, string? DateRange) : IRequest<ApiResponse<bool>>;
}
