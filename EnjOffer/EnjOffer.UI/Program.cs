using EnjOffer.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.Extensions.Configuration;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Services;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Infrastructure.Repositories;
using EnjOffer.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using EnjOffer.UI.EmailConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EnjOfferDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));

builder.Services.AddControllers();

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);
});

builder.Services.AddScoped<IUserWordsService, UserWordsService>();
builder.Services.AddScoped<IDefaultWordsService, DefaultWordsService>();
builder.Services.AddScoped<IUserStatisticsService, UserStatisticsService>();
builder.Services.AddScoped<IUsersDefaultWordsService, UsersDefaultWordsService>();
builder.Services.AddScoped<IWordsService, WordsService>();
builder.Services.AddScoped<IAdviceService, AdviceService>();
builder.Services.AddScoped<IBooksService, BooksService>();

builder.Services.AddScoped<IUserWordsRepository, UserWordsRepository>();
builder.Services.AddScoped<IDefaultWordsRepository, DefaultWordsRepository>();
builder.Services.AddScoped<IUserStatisticsRepository, UserStatisticsRepository>();
builder.Services.AddScoped<IUsersDefaultWordsRepository, UsersDefaultWordsRepository>();
builder.Services.AddScoped<IAdviceRepository, AdviceRepository>();
builder.Services.AddScoped<IBooksRepository, BooksRepository>();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<EnjOfferDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, EnjOfferDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, EnjOfferDbContext, Guid>>();

builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddAuthorization(options => options.FallbackPolicy =
    new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/login");

builder.Services.AddControllersWithViews().AddNewtonsoftJson();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();