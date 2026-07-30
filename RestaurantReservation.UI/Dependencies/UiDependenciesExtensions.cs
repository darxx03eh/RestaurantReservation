using Microsoft.Extensions.DependencyInjection;

namespace RestaurantReservation.UI.Dependencies;

public static class AddUiDependenciesExtensions
{
    public static IServiceCollection AddUiDependencies(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<RestaurantReservationConsole>()
            .AddClasses()
            .AsSelf()
            .WithSingletonLifetime());
        return services;
    }
}