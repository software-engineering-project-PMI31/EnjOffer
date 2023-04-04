using EnjOffer.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.Extensions.Configuration;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EnjOfferDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);
});

builder.Services.AddScoped<IUserWordsService, UserWordsService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IDefaultWordsService, DefaultWordsService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();