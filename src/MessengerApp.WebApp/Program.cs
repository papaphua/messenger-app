using System.IdentityModel.Tokens.Jwt;
using DotNetEnv;
using MessengerApp.Application;
using MessengerApp.Application.Abstractions.Data;
using MessengerApp.Application.Services.ChannelService;
using MessengerApp.Application.Services.DirectService;
using MessengerApp.Application.Services.GroupService;
using MessengerApp.Application.Services.ProfileService;
using MessengerApp.Application.Services.UserService;
using MessengerApp.Domain.Constants;
using MessengerApp.Domain.Entities;
using MessengerApp.Infrastructure.Data;
using MessengerApp.Infrastructure.Options;
using MessengerApp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllersWithViews();

// Options
builder.Services.Configure<EmailOptions>(configuration.GetSection(Sections.EmailOptions).Bind);

// Application services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IDirectService, DirectService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IChannelService, ChannelService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services
    .AddIdentityCore<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender, CustomEmailSender>();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:5001";

        options.ClientId = "mvc";
        options.ClientSecret = Environment.GetEnvironmentVariable(Envs.MvcClientSecret);
        options.ResponseType = "code";

        options.SaveTokens = true;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("offline_access");
        options.Scope.Add("scope1");

        options.GetClaimsFromUserInfoEndpoint = true;
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment()) app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}")
    .RequireAuthorization();

app.Run();