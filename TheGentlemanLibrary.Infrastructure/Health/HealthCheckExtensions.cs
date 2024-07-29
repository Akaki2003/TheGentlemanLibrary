using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TheGentlemanLibrary.Infrastructure.Health
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddHealthChecks().AddNpgSql(configuration["ConnectionStrings:DefaultConnection"]!);
            return services;
        }

        public static IEndpointConventionBuilder UseCustomHealthChecks(this IEndpointRouteBuilder erb)
        {
            return erb
                .MapHealthChecks("/_health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
        }   
    }
}
