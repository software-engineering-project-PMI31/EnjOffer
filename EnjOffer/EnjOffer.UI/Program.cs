using EnjOffer.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.Extensions.Configuration;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Services;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EnjOfferDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);
});

builder.Services.AddSingleton<IUsersRepository, UsersRepository>();
builder.Services.AddSingleton<IUserWordsRepository, UserWordsRepository>();
builder.Services.AddSingleton<IDefaultWordsRepository, DefaultWordsRepository>();
builder.Services.AddSingleton<IUserStatisticsRepository, UserStatisticsRepository>();
builder.Services.AddSingleton<IUsersDefaultWordsRepository, UsersDefaultWordsRepository>();

builder.Services.AddSingleton<IUsersService, UsersService>();
builder.Services.AddSingleton<IUserWordsService, UserWordsService>();
builder.Services.AddSingleton<IDefaultWordsService, DefaultWordsService>();
builder.Services.AddSingleton<IUserStatisticsService, UserStatisticsService>();
builder.Services.AddSingleton<IUsersDefaultWordsService, UsersDefaultWordsService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();