using Microsoft.AspNetCore.Diagnostics;
using Npgsql;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using System.Net;
using EnjOffer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);
});

builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseRouting();
app.MapControllers();

app.Run();