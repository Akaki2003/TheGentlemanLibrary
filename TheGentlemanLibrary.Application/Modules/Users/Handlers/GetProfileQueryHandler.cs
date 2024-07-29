using MediatR;
using System.Net;
using Microsoft.Extensions.Logging;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Application.Models.Users.Interfaces;
using TheGentlemanLibrary.Application.Models.Users.Queries;
using TheGentlemanLibrary.Application.Models.Users.Responses;
using TheGentlemanLibrary.Application.Models.BaseModels;

namespace TheGentlemanLibrary.Application.Models.Users.Handlers
{
    public class GetProfileQueryHandler(IUserRepository userRepo, ILogger<GetProfileQueryHandler> logger) : IRequestHandler<GetProfileQuery, ApiResponse<ProfileResponseModel>>
    {
        private readonly IUserRepository _userRepo = userRepo;
        private readonly ILogger<GetProfileQueryHandler> _logger = logger;

        public async Task<ApiResponse<ProfileResponseModel>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepo.GetProfile(request.ID);

                if (user == null)
                {
                    return ApiResponse<ProfileResponseModel>.Fail(null, new List<string> { RsStrings.UserNotFound }, (int)HttpStatusCode.NotFound);
                }

                return ApiResponse<ProfileResponseModel>.Success(new ProfileResponseModel { Email = user.Email, Id = user.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the profile");
                return ApiResponse<ProfileResponseModel>.Fail(null, new List<string> { RsStrings.ProfileFetchError }, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
