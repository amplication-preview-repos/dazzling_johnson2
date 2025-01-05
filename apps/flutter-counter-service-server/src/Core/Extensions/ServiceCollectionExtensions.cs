using FlutterCounterService.APIs;

namespace FlutterCounterService;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add services to the container.
    /// </summary>
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ICountersService, CountersService>();
        services.AddScoped<IUsersService, UsersService>();
    }
}
