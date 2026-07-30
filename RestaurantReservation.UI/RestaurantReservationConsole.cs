using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.Implementations;
using RestaurantReservation.Infrastructure.Interfaces;
using RestaurantReservation.Infrastructure.Repositories;
using RestaurantReservation.Infrastructure.Seeders;
using RestaurantReservation.UI.Common;
using RestaurantReservation.UI.Features;
using RestaurantReservation.UI.Features.Customers;
using RestaurantReservation.UI.Features.Employees;
using RestaurantReservation.UI.Features.MenuItems;
using RestaurantReservation.UI.Features.Orders;
using RestaurantReservation.UI.Features.Reservations;
using RestaurantReservation.UI.Features.Restaurants;
using RestaurantReservation.UI.Features.Tables;
using Spectre.Console;

namespace RestaurantReservation.UI;

public sealed class RestaurantReservationConsole : IAsyncDisposable
{
    private readonly RestaurantReservationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SeederRunner _seederRunner;
    private readonly DashboardConsole _dashboardConsole;
    private readonly CustomerConsole _customerConsole;
    private readonly RestaurantConsole _restaurantConsole;
    private readonly TableConsole _tableConsole;
    private readonly EmployeeConsole _employeeConsole;
    private readonly MenuItemConsole _menuItemConsole;
    private readonly ReservationConsole _reservationConsole;
    private readonly OrderConsole _orderConsole;

    public RestaurantReservationConsole(RestaurantReservationDbContext context, DashboardConsole dashboardConsole,
        CustomerConsole customerConsole, RestaurantConsole restaurantConsole, TableConsole tableConsole,
        EmployeeConsole employeeConsole, MenuItemConsole menuItemConsole, ReservationConsole reservationConsole,
        OrderConsole orderConsole, IUnitOfWork unitOfWork, SeederRunner seederRunner )
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _seederRunner = seederRunner;

        _dashboardConsole = dashboardConsole;
        _customerConsole = customerConsole;
        _restaurantConsole = restaurantConsole;
        _tableConsole = tableConsole;
        _employeeConsole = employeeConsole;
        _menuItemConsole = menuItemConsole;
        _reservationConsole = reservationConsole;
        _orderConsole = orderConsole;
    }

    public async Task RunAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Reservations").Color(Color.Teal));

        var shouldSeed = AnsiConsole.Confirm("Do you want to seed the database?", true);
        if (shouldSeed)
        {
            var alreadySeeded = await _seederRunner.IsAlreadySeededAsync();
            if(alreadySeeded)
                AnsiConsole.MarkupLine("[yellow]The database has already been seeded.[/]");
            else
            {
                AnsiConsole.MarkupLine("[cyan]Seeding database, please wait...[/]");
                await _seederRunner.RunAsync(_context);
                AnsiConsole.MarkupLine("[green]Database seeding completed successfully.[/]");
            }
        }
        AnsiConsole.MarkupLine("[green]Application is ready.[/]");
        while (true)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Choose a menu[/]")
                    .PageSize(10)
                    .AddChoices(
                        "Show dashboard",
                        "Customers",
                        "Restaurants",
                        "Tables",
                        "Employees",
                        "Menu items",
                        "Reservations",
                        "Orders",
                        "Exit"));

            if (option == "Exit")
                break;

            await RunMenuAsync(option);
        }
    }

    private async Task RunMenuAsync(string option)
    {
        switch (option)
        {
            case "Show dashboard":
                await RunActionAsync(_dashboardConsole.ShowAsync);
                break;
            case "Customers":
                await RunCustomersMenuAsync();
                break;
            case "Restaurants":
                await RunRestaurantsMenuAsync();
                break;
            case "Tables":
                await RunTablesMenuAsync();
                break;
            case "Employees":
                await RunEmployeesMenuAsync();
                break;
            case "Menu items":
                await RunMenuItemsMenuAsync();
                break;
            case "Reservations":
                await RunReservationsMenuAsync();
                break;
            case "Orders":
                await RunOrdersMenuAsync();
                break;
        }
    }

    private async Task RunCustomersMenuAsync()
    {
        while (true)
        {
            var option = PromptSubMenu("Customers", "List customers", "Create customer", "Update customer", "Delete customer", "Customers by party size", "Back");
            if (option == "Back")
                return;

            await RunActionAsync(option switch
            {
                "List customers" => _customerConsole.ListAsync,
                "Create customer" => _customerConsole.CreateAsync,
                "Update customer" => _customerConsole.UpdateAsync,
                "Delete customer" => _customerConsole.DeleteAsync,
                "Customers by party size" => _customerConsole.ShowByPartySizeAsync,
                _ => throw new InvalidOperationException("Unknown customer action.")
            });
        }
    }

    private async Task RunRestaurantsMenuAsync()
    {
        while (true)
        {
            var option = PromptSubMenu("Restaurants", "List restaurants", "Create restaurant", "Show revenue", "Back");
            if (option == "Back")
                return;

            await RunActionAsync(option switch
            {
                "List restaurants" => _restaurantConsole.ListAsync,
                "Create restaurant" => _restaurantConsole.CreateAsync,
                "Show revenue" => _restaurantConsole.ShowRevenueAsync,
                _ => throw new InvalidOperationException("Unknown restaurant action.")
            });
        }
    }

    private async Task RunTablesMenuAsync()
    {
        while (true)
        {
            var option = PromptSubMenu("Tables", "List tables", "Create table", "Back");
            if (option == "Back")
                return;

            await RunActionAsync(option switch
            {
                "List tables" => _tableConsole.ListAsync,
                "Create table" => _tableConsole.CreateAsync,
                _ => throw new InvalidOperationException("Unknown table action.")
            });
        }
    }

    private async Task RunEmployeesMenuAsync()
    {
        while (true)
        {
            var option = PromptSubMenu("Employees", "List employees", "Employee restaurant details", "Back");
            if (option == "Back")
                return;

            await RunActionAsync(option switch
            {
                "List employees" => _employeeConsole.ListAsync,
                "Employee restaurant details" => _employeeConsole.ShowRestaurantDetailsViewAsync,
                _ => throw new InvalidOperationException("Unknown employee action.")
            });
        }
    }

    private async Task RunMenuItemsMenuAsync()
    {
        while (true)
        {
            var option = PromptSubMenu("Menu items", "List menu items", "Create menu item", "Back");
            if (option == "Back")
                return;

            await RunActionAsync(option switch
            {
                "List menu items" => _menuItemConsole.ListAsync,
                "Create menu item" => _menuItemConsole.CreateAsync,
                _ => throw new InvalidOperationException("Unknown menu item action.")
            });
        }
    }

    private async Task RunReservationsMenuAsync()
    {
        while (true)
        {
            var option = PromptSubMenu("Reservations", "List reservations", "Create reservation", "Reservation details", "Back");
            if (option == "Back")
                return;

            await RunActionAsync(option switch
            {
                "List reservations" => _reservationConsole.ListAsync,
                "Create reservation" => _reservationConsole.CreateAsync,
                "Reservation details" => _reservationConsole.ShowDetailsViewAsync,
                _ => throw new InvalidOperationException("Unknown reservation action.")
            });
        }
    }

    private async Task RunOrdersMenuAsync()
    {
        while (true)
        {
            var option = PromptSubMenu("Orders", "List orders with menu items", "Create order", "Back");
            if (option == "Back")
                return;

            await RunActionAsync(option switch
            {
                "List orders with menu items" => _orderConsole.ListWithMenuItemsAsync,
                "Create order" => _orderConsole.CreateAsync,
                _ => throw new InvalidOperationException("Unknown order action.")
            });
        }
    }

    private static string PromptSubMenu(string title, params string[] options)
        => AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[bold]{title}[/]")
                .PageSize(10)
                .AddChoices(options));

    private async Task RunActionAsync(Func<Task> action)
    {
        try
        {
            await action();
            Ui.Pause();
        }
        catch (Exception exception)
        {
            Ui.WriteError(exception);
            Ui.Pause();
        }
    }
    
    public async ValueTask DisposeAsync() => await _context.DisposeAsync();
}
