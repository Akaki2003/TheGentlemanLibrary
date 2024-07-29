using TGL.API.Signalr;
using TheGentlemanLibrary.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var env = builder.Environment;
var migrationsAssembly = "TheGentlemanLibrary.Infrastructure";
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


// Add services to the container.
services.AddMemoryCache();


builder.Services.AddDefaultIdentity<User>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddRoles<UserRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSignalR();
services.AddCors();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(builder =>
{
    builder
    .WithOrigins(["http://localhost:5173", "https://localhost:7234/"])
    .AllowAnyMethod().AllowCredentials().AllowAnyHeader();
});

app.UseHttpsRedirection();

app.MapHub<ChatHub>("/chatHub");
app.Run();
