using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.Implementations;
using RestaurantReservation.Infrastructure.Interfaces;
using RestaurantReservation.Infrastructure.Repositories;
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
    private readonly DashboardConsole _dashboardConsole;
    private readonly CustomerConsole _customerConsole;
    private readonly RestaurantConsole _restaurantConsole;
    private readonly TableConsole _tableConsole;
    private readonly EmployeeConsole _employeeConsole;
    private readonly MenuItemConsole _menuItemConsole;
    private readonly ReservationConsole _reservationConsole;
    private readonly OrderConsole _orderConsole;

    public RestaurantReservationConsole()
    {
        _context = new RestaurantReservationDbContext();
        _unitOfWork = CreateUnitOfWork(_context);

        var selector = new EntitySelector(_context, _unitOfWork);
        _dashboardConsole = new DashboardConsole(_context, _unitOfWork);
        _customerConsole = new CustomerConsole(_context, _unitOfWork, selector);
        _restaurantConsole = new RestaurantConsole(_context, _unitOfWork);
        _tableConsole = new TableConsole(_unitOfWork, selector);
        _employeeConsole = new EmployeeConsole(_context, _unitOfWork);
        _menuItemConsole = new MenuItemConsole(_unitOfWork, selector);
        _reservationConsole = new ReservationConsole(_context, selector);
        _orderConsole = new OrderConsole(_context, _unitOfWork, selector);
    }

    public async Task RunAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Reservations").Color(Color.Teal));

        while (true)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Choose an action[/]")
                    .PageSize(16)
                    .MoreChoicesText("[grey]Move up and down to see more actions[/]")
                    .AddChoices(
                        "Show dashboard",
                        "List restaurants",
                        "List customers",
                        "List tables",
                        "List employees",
                        "List menu items",
                        "List reservations",
                        "List orders with menu items",
                        "Show reservation details view",
                        "Show employee restaurant details view",
                        "Show revenue by restaurant function",
                        "Run customers by party size procedure",
                        "Create customer",
                        "Create restaurant",
                        "Create table",
                        "Create menu item",
                        "Create reservation",
                        "Create order",
                        "Update customer",
                        "Delete customer",
                        "Exit"));

            if (option == "Exit")
                break;

            await RunActionAsync(option);
        }
    }

    private async Task RunActionAsync(string option)
    {
        try
        {
            switch (option)
            {
                case "Show dashboard":
                    await _dashboardConsole.ShowAsync();
                    break;
                case "List restaurants":
                    await _restaurantConsole.ListAsync();
                    break;
                case "List customers":
                    await _customerConsole.ListAsync();
                    break;
                case "List tables":
                    await _tableConsole.ListAsync();
                    break;
                case "List employees":
                    await _employeeConsole.ListAsync();
                    break;
                case "List menu items":
                    await _menuItemConsole.ListAsync();
                    break;
                case "List reservations":
                    await _reservationConsole.ListAsync();
                    break;
                case "List orders with menu items":
                    await _orderConsole.ListWithMenuItemsAsync();
                    break;
                case "Show reservation details view":
                    await _reservationConsole.ShowDetailsViewAsync();
                    break;
                case "Show employee restaurant details view":
                    await _employeeConsole.ShowRestaurantDetailsViewAsync();
                    break;
                case "Show revenue by restaurant function":
                    await _restaurantConsole.ShowRevenueAsync();
                    break;
                case "Run customers by party size procedure":
                    await _customerConsole.ShowByPartySizeAsync();
                    break;
                case "Create customer":
                    await _customerConsole.CreateAsync();
                    break;
                case "Create restaurant":
                    await _restaurantConsole.CreateAsync();
                    break;
                case "Create table":
                    await _tableConsole.CreateAsync();
                    break;
                case "Create menu item":
                    await _menuItemConsole.CreateAsync();
                    break;
                case "Create reservation":
                    await _reservationConsole.CreateAsync();
                    break;
                case "Create order":
                    await _orderConsole.CreateAsync();
                    break;
                case "Update customer":
                    await _customerConsole.UpdateAsync();
                    break;
                case "Delete customer":
                    await _customerConsole.DeleteAsync();
                    break;
            }

            Ui.Pause();
        }
        catch (Exception exception)
        {
            AnsiConsole.MarkupLine("[red]Action failed.[/]");
            AnsiConsole.WriteException(exception, ExceptionFormats.ShortenEverything);
            Ui.Pause();
        }
    }

    private static IUnitOfWork CreateUnitOfWork(RestaurantReservationDbContext context)
        => new UnitOfWork(
            context,
            new CustomerRepository(context),
            new ReservationRepository(context),
            new OrderRepository(context),
            new EmployeeRepository(context),
            new MenuItemRepository(context),
            new OrderItemRepository(context),
            new RestaurantRepository(context),
            new TableRepository(context));

    public async ValueTask DisposeAsync() => await _context.DisposeAsync();
}
