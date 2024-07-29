using System.Security.Claims;

namespace TheGentlemanLibrary.API.Endpoints.Common
{
    public class UserHelper
    {
        public static int? GetUserId(HttpContext httpContext)
        {
            var userIdClaim = httpContext.User?.Claims?
                .FirstOrDefault(c => c.Type == "sub" || c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            return null;
        }
    }
}
