using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection;

namespace ZoeUserCounter;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlazorOnlineTracker(this IServiceCollection services)
    {
        services.AddSingleton<UserTracker>();
        services.AddSingleton<WhoIsOnline>();
        services.AddSingleton<CircuitHandler, UserTrackingCircuitHandler>();
        return services;
    }
}
