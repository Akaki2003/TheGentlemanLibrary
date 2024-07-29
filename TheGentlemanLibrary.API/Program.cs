using FluentValidation;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using TheGentlemanLibrary.API.Infrastructure.Extensions;
using TheGentlemanLibrary.API.Middlewares;
using TheGentlemanLibrary.Application.Dependency;
using TheGentlemanLibrary.Application.Models.Authors.Handlers;
using TheGentlemanLibrary.Application.Models.Users.JWT;
using TheGentlemanLibrary.Domain.Entities;
using TheGentlemanLibrary.Infrastructure.Data;
using TheGentlemanLibrary.Infrastructure.Health;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var env = builder.Environment;
var migrationsAssembly = "TheGentlemanLibrary.Infrastructure";
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var supportedCultures = new[] { "en", "ka" };

services.AddLogging();
services.AddCustomHealthChecks(configuration);
services.AddExceptionHandler<GlobalExceptionHandler>();
services.AddProblemDetails();


services.AddTGLDb(configuration);
services.AddStackExchangeRedisCache(opt =>
    opt.Configuration = configuration.GetConnectionString("Cache"));
services.AddHybridCache();

services.AddLocalization(options => options.ResourcesPath = "TheGentlemanLibrary.Common/Resources");
services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

services.AddDefaultIdentity<User>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddRoles<UserRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

services.AddValidatorsFromAssembly(typeof(DependencyMarker).Assembly);
services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetAuthorsQueryHandler).Assembly)
        .AddMyBehaviors()
        );
services.AddControllers();
services.AddCors();

builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));
services.Configure<JWTConfiguration>(builder.Configuration.GetSection(nameof(JWTConfiguration)));
services.AddMyAuthentication(configuration);

services.AddServices();

services.AddEndpointsApiExplorer();
services.AddSwaggerDocumentation(configuration);
services.AddHttpClient();
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => c.DisplayRequestDuration());

app.UseCors(builder =>
{
    builder
    .WithOrigins(["http://localhost:5173", "https://localhost:7123/"])
    .AllowAnyMethod().AllowCredentials().AllowAnyHeader();
});
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapHealthChecks("/_health");
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestLocalization();
app.MapEndpoints();
app.MapControllers();

app.Run();

