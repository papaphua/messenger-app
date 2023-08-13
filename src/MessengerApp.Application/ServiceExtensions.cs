using MessengerApp.Application.Services.UserService;
using Microsoft.Extensions.DependencyInjection;

namespace MessengerApp.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}