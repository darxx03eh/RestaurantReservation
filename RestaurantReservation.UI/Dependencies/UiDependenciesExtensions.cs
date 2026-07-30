using Microsoft.Extensions.DependencyInjection;

namespace RestaurantReservation.UI.Dependencies;

public static class UiDependenciesExtensions
{
    public static IServiceCollection AddUiDependencies(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<UiAssemblyMarker>()
            .AddClasses()
            .AsSelf()
            .WithScopedLifetime());
        return services;
    }
}