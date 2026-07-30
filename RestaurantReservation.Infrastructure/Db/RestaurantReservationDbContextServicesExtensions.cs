using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RestaurantReservation.Infrastructure.Db;

public static class RestaurantReservationDbContextServicesExtensions
{
    public static IServiceCollection AddRestaurantReservationDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<RestaurantReservationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("RestaurantReservationDbLocalConnection"));
        });
        
        return services;
    }
    public static IServiceCollection AddRestaurantReservationDbContextFactory(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContextFactory<RestaurantReservationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("RestaurantReservationDbLocalConnection"));
        });
        return services;
    }
}