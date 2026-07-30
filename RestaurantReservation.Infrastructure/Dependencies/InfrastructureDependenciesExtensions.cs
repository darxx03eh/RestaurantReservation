using Microsoft.Extensions.DependencyInjection;
using RestaurantReservation.Infrastructure.Seeders;

namespace RestaurantReservation.Infrastructure.Dependencies;

public static class InfrastructureDependenciesExtensions
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<InfrastructureAssemblyMarker>()
            .AddClasses(classes => classes.Where(type => !typeof(ISeeder).IsAssignableTo(type)))
            .AsSelf()
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.Scan(scan => scan
            .FromAssemblyOf<InfrastructureAssemblyMarker>()
            .AddClasses(classes => classes.AssignableTo<ISeeder>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}