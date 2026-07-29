using RestaurantReservation.UI;

namespace RestaurantReservation;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);
            await using var console = new RestaurantReservationConsole();
            await console.RunAsync();
        }
        catch (Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Application failed to start.");
            Console.ResetColor();
            Console.Error.WriteLine(exception.GetBaseException().Message);
        }
    }
}
