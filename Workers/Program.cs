using Microsoft.EntityFrameworkCore;
using TheGentlemanLibrary.Application.Models.Orders.Interfaces;
using TheGentlemanLibrary.Infrastructure.Data;
using TheGentlemanLibrary.Infrastructure.Repositories;
using Workers;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var connString = configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<ApplicationDbContext>((DbContextOptionsBuilder opt) => opt.UseNpgsql(connString));
services.AddScoped<IOrderRepository, OrderRepository>();

services.AddHostedService<OrderBackgroundService>();

var host = builder.Build();
host.Run();
