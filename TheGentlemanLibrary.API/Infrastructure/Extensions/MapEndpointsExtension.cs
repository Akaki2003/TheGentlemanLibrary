using TheGentlemanLibrary.API.Endpoints;

namespace TheGentlemanLibrary.API.Infrastructure.Extensions
{
    public static class MapEndpointsExtension
    {
        public static void MapEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapBookEndpoints();
            app.MapOrderEndpoints();
            app.MapAuthorEndpoints();
            app.MapUserEndpoints();
        }
    }
}
