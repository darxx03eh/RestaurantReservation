using RestaurantReservation.UI;

namespace RestaurantReservation;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);
        await using var console = new RestaurantReservationConsole();
        await console.RunAsync();
    }
}
