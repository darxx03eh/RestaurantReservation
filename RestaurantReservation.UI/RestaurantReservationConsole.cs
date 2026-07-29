using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Entities.Procedures;
using RestaurantReservation.Domain.Entities.Views;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.Implementations;
using RestaurantReservation.Infrastructure.Interfaces;
using RestaurantReservation.Infrastructure.Repositories;
using RestaurantReservation.Infrastructure.Seeders;
using Spectre.Console;
using RestaurantTable = RestaurantReservation.Domain.Entities.Table;
using SpectreTable = Spectre.Console.Table;

namespace RestaurantReservation.UI;

public sealed class RestaurantReservationConsole : IAsyncDisposable
{
    private readonly RestaurantReservationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public RestaurantReservationConsole()
    {
        _context = new RestaurantReservationDbContext();
        _unitOfWork = CreateUnitOfWork(_context);
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
                        "Apply migrations",
                        "Seed sample data",
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
                case "Apply migrations":
                    await ApplyMigrationsAsync();
                    break;
                case "Seed sample data":
                    await SeedAsync();
                    break;
                case "Show dashboard":
                    await ShowDashboardAsync();
                    break;
                case "List restaurants":
                    ShowRestaurants(await _unitOfWork.Restaurants.GetAllAsync());
                    break;
                case "List customers":
                    ShowCustomers(await _unitOfWork.Customers.GetAllAsync());
                    break;
                case "List tables":
                    ShowTables(await _unitOfWork.Tables.GetAllAsync());
                    break;
                case "List employees":
                    ShowEmployees(await _unitOfWork.Employees.GetAllAsync());
                    break;
                case "List menu items":
                    ShowMenuItems(await _unitOfWork.MenuItems.GetAllAsync());
                    break;
                case "List reservations":
                    await ShowReservationsAsync();
                    break;
                case "List orders with menu items":
                    await ShowOrdersWithMenuItemsAsync();
                    break;
                case "Show reservation details view":
                    await ShowReservationDetailsViewAsync();
                    break;
                case "Show employee restaurant details view":
                    await ShowEmployeeRestaurantDetailsViewAsync();
                    break;
                case "Show revenue by restaurant function":
                    await ShowRevenueByRestaurantAsync();
                    break;
                case "Run customers by party size procedure":
                    await ShowCustomersByPartySizeAsync();
                    break;
                case "Create customer":
                    await CreateCustomerAsync();
                    break;
                case "Create restaurant":
                    await CreateRestaurantAsync();
                    break;
                case "Create table":
                    await CreateTableAsync();
                    break;
                case "Create menu item":
                    await CreateMenuItemAsync();
                    break;
                case "Create reservation":
                    await CreateReservationAsync();
                    break;
                case "Create order":
                    await CreateOrderAsync();
                    break;
                case "Update customer":
                    await UpdateCustomerAsync();
                    break;
                case "Delete customer":
                    await DeleteCustomerAsync();
                    break;
            }

            Pause();
        }
        catch (Exception exception)
        {
            AnsiConsole.MarkupLine("[red]Action failed.[/]");
            AnsiConsole.WriteException(exception, ExceptionFormats.ShortenEverything);
            Pause();
        }
    }

    private async Task ApplyMigrationsAsync()
    {
        await _context.Database.MigrateAsync();
        AnsiConsole.MarkupLine("[green]Database migrations applied.[/]");
    }

    private async Task SeedAsync()
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await new SeederRunner().RunAsync(_context);
            await transaction.CommitAsync();
            AnsiConsole.MarkupLine("[green]Sample data seeded.[/]");
        }
        catch
        {
            await transaction.RollbackAsync();
            AnsiConsole.MarkupLine("[yellow]The infrastructure seeders were rolled back. Their reservation/order dates can conflict with the SQL date constraints depending on the current date.[/]");
            throw;
        }
    }

    private async Task ShowDashboardAsync()
    {
        var grid = new Grid().AddColumn().AddColumn();
        grid.AddRow("Restaurants", (await _unitOfWork.Restaurants.GetAllAsync()).Count.ToString());
        grid.AddRow("Customers", (await _unitOfWork.Customers.GetAllAsync()).Count.ToString());
        grid.AddRow("Tables", (await _unitOfWork.Tables.GetAllAsync()).Count.ToString());
        grid.AddRow("Employees", (await _unitOfWork.Employees.GetAllAsync()).Count.ToString());
        grid.AddRow("Menu items", (await _unitOfWork.MenuItems.GetAllAsync()).Count.ToString());
        grid.AddRow("Reservations", (await _context.Reservations.CountAsync()).ToString());
        grid.AddRow("Orders", (await _unitOfWork.Orders.GetAllAsync()).Count.ToString());

        AnsiConsole.Write(new Panel(grid).Header("Current Data").Border(BoxBorder.Rounded));
    }

    private async Task ShowReservationsAsync()
    {
        var reservations = await _context.Reservations
            .AsNoTracking()
            .Include(reservation => reservation.Customer)
            .Include(reservation => reservation.Restaurant)
            .Include(reservation => reservation.Table)
            .OrderBy(reservation => reservation.ReservationDate)
            .ToListAsync();

        var table = CreateTable("Reservations", "Id", "Date", "Customer", "Restaurant", "Table", "Party");
        foreach (var reservation in reservations)
        {
            table.AddRow(
                reservation.ReservationId.ToString(),
                FormatDate(reservation.ReservationDate),
                FullName(reservation.Customer),
                reservation.Restaurant.Name,
                reservation.TableId.ToString(),
                reservation.PartySize.ToString());
        }

        AnsiConsole.Write(table);
    }

    private async Task ShowOrdersWithMenuItemsAsync()
    {
        var reservationId = AnsiConsole.Ask<int>("Reservation id:");
        var orders = await _unitOfWork.Orders.ListOrdersAndMenuItemsAsync(reservationId);
        var table = CreateTable("Orders", "Order", "Date", "Total", "Items");

        foreach (var order in orders)
        {
            var items = order.OrderItems.Any()
                ? string.Join(", ", order.OrderItems.Select(item => $"{item.Item.Name} x{item.Quantity}"))
                : "No items";

            table.AddRow(order.OrderId.ToString(), FormatDate(order.OrderDate), FormatMoney(order.TotalAmount), items);
        }

        AnsiConsole.Write(table);
    }

    private async Task ShowReservationDetailsViewAsync()
    {
        var rows = await _context.Set<ReservationDetailsView>()
            .AsNoTracking()
            .OrderBy(row => row.ReservationDate)
            .ToListAsync();

        var table = CreateTable("Reservation Details View", "Id", "Date", "Customer", "Phone", "Restaurant", "Party");
        foreach (var row in rows)
        {
            table.AddRow(
                row.ReservationId.ToString(),
                FormatDate(row.ReservationDate),
                row.CustomerName,
                row.CustomerPhoneNumber,
                row.RestaurantName,
                row.PartySize.ToString());
        }

        AnsiConsole.Write(table);
    }

    private async Task ShowEmployeeRestaurantDetailsViewAsync()
    {
        var rows = await _context.Set<EmployeeRestaurantDetailsView>()
            .AsNoTracking()
            .OrderBy(row => row.RestauranName)
            .ThenBy(row => row.EmployeeName)
            .ToListAsync();

        var table = CreateTable("Employee Restaurant Details View", "Employee", "Position", "Restaurant", "Phone", "Hours");
        foreach (var row in rows)
            table.AddRow(row.EmployeeName, row.Position, row.RestauranName, row.RestauranPhoneNumber, row.OpeningHours);

        AnsiConsole.Write(table);
    }

    private async Task ShowRevenueByRestaurantAsync()
    {
        var rows = await _context.Restaurants
            .AsNoTracking()
            .Select(restaurant => new
            {
                restaurant.RestaurantId,
                restaurant.Name,
                Revenue = _context.GetRestaurantTotalRevenue(restaurant.RestaurantId)
            })
            .OrderBy(row => row.RestaurantId)
            .ToListAsync();

        var table = CreateTable("Revenue Function", "Restaurant", "Revenue");
        foreach (var row in rows)
            table.AddRow($"{row.RestaurantId} - {row.Name}", FormatMoney(row.Revenue));

        AnsiConsole.Write(table);
    }

    private async Task ShowCustomersByPartySizeAsync()
    {
        var partySize = AnsiConsole.Prompt(
            new TextPrompt<int>("Minimum party size:")
                .Validate(value => value >= 0 ? ValidationResult.Success() : ValidationResult.Error("Use zero or more.")));

        var rows = await _context.Set<CustomerReservationProcedure>()
            .FromSqlInterpolated($"EXEC dbo.sp_GetCustomersByPartySize {partySize}")
            .AsNoTracking()
            .ToListAsync();

        var table = CreateTable("Stored Procedure Result", "Id", "Name", "Email", "Phone");
        foreach (var row in rows)
            table.AddRow(row.CustomerId.ToString(), row.FullName, row.Email, row.PhoneNumber);

        AnsiConsole.Write(table);
    }

    private async Task CreateCustomerAsync()
    {
        var customer = new Customer
        {
            FirstName = PromptRequired("First name:", 50),
            LastName = PromptRequired("Last name:", 50),
            Email = PromptRequired("Email:", 100),
            PhoneNumber = PromptRequired("Phone number:", 20)
        };

        await _unitOfWork.Customers.AddAsync(customer);
        AnsiConsole.MarkupLine($"[green]Customer #{customer.CustomerId} created.[/]");
    }

    private async Task CreateRestaurantAsync()
    {
        var restaurant = new Restaurant
        {
            Name = PromptRequired("Name:", 100),
            Address = PromptRequired("Address:", 100),
            PhoneNumber = PromptOptional("Phone number:", 20) ?? string.Empty,
            OpeningHours = PromptOptional("Opening hours:", 100) ?? string.Empty
        };

        await _unitOfWork.Restaurants.AddAsync(restaurant);
        AnsiConsole.MarkupLine($"[green]Restaurant #{restaurant.RestaurantId} created.[/]");
    }

    private async Task CreateTableAsync()
    {
        var restaurant = await SelectRestaurantAsync();
        if (restaurant is null)
            return;

        var restaurantTable = new RestaurantTable
        {
            RestaurantId = restaurant.RestaurantId,
            Capacity = AnsiConsole.Prompt(
                new TextPrompt<int>("Capacity:")
                    .Validate(value => value is > 0 and <= 20
                        ? ValidationResult.Success()
                        : ValidationResult.Error("Capacity must be between 1 and 20.")))
        };

        await _unitOfWork.Tables.AddAsync(restaurantTable);
        AnsiConsole.MarkupLine($"[green]Table #{restaurantTable.TableId} created.[/]");
    }

    private async Task CreateMenuItemAsync()
    {
        var restaurant = await SelectRestaurantAsync();
        if (restaurant is null)
            return;

        var item = new MenuItem
        {
            RestaurantId = restaurant.RestaurantId,
            Name = PromptRequired("Name:", 100),
            Description = PromptOptional("Description:", 500) ?? string.Empty,
            Price = AnsiConsole.Prompt(
                new TextPrompt<decimal>("Price:")
                    .Validate(value => value > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("Price must be greater than zero.")))
        };

        await _unitOfWork.MenuItems.AddAsync(item);
        AnsiConsole.MarkupLine($"[green]Menu item #{item.ItemId} created.[/]");
    }

    private async Task CreateReservationAsync()
    {
        var customer = await SelectCustomerAsync();
        var restaurant = await SelectRestaurantAsync();
        if (customer is null || restaurant is null)
            return;

        var tables = await _context.Tables
            .AsNoTracking()
            .Where(table => table.RestaurantId == restaurant.RestaurantId)
            .OrderBy(table => table.TableId)
            .ToListAsync();

        if (tables.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]This restaurant has no tables yet.[/]");
            return;
        }

        var selectedTable = SelectEntity(tables, "Table:", table => $"#{table.TableId} - capacity {table.Capacity}");
        var partySize = AnsiConsole.Prompt(
            new TextPrompt<int>("Party size:")
                .Validate(value => value > 0 && value <= selectedTable.Capacity
                    ? ValidationResult.Success()
                    : ValidationResult.Error($"Party size must be between 1 and {selectedTable.Capacity}.")));

        var reservation = new Reservation
        {
            CustomerId = customer.CustomerId,
            RestaurantId = restaurant.RestaurantId,
            TableId = selectedTable.TableId,
            PartySize = partySize,
            ReservationDate = PromptDate("Reservation date (yyyy-MM-dd HH:mm):", DateTime.UtcNow.AddHours(1), mustBeFuture: true)
        };

        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
        AnsiConsole.MarkupLine($"[green]Reservation #{reservation.ReservationId} created.[/]");
    }

    private async Task CreateOrderAsync()
    {
        var reservation = await SelectReservationAsync();
        if (reservation is null)
            return;

        var employees = await _context.Employees
            .AsNoTracking()
            .Where(employee => employee.RestaurantId == reservation.RestaurantId)
            .OrderBy(employee => employee.EmployeeId)
            .ToListAsync();

        var menuItems = await _context.MenuItems
            .AsNoTracking()
            .Where(item => item.RestaurantId == reservation.RestaurantId)
            .OrderBy(item => item.Name)
            .ToListAsync();

        if (employees.Count == 0 || menuItems.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]The reservation restaurant needs employees and menu items before an order can be created.[/]");
            return;
        }

        var employee = SelectEntity(employees, "Employee:", item => $"#{item.EmployeeId} - {FullName(item)} ({item.Position})");
        var selectedItems = AnsiConsole.Prompt(
            new MultiSelectionPrompt<MenuItem>()
                .Title("Order items:")
                .NotRequired()
                .UseConverter(item => $"#{item.ItemId} - {item.Name} ({FormatMoney(item.Price)})")
                .AddChoices(menuItems));

        if (selectedItems.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No order was created because no items were selected.[/]");
            return;
        }

        var quantities = new Dictionary<int, int>();
        foreach (var item in selectedItems)
        {
            quantities[item.ItemId] = AnsiConsole.Prompt(
                new TextPrompt<int>($"{item.Name} quantity:")
                    .Validate(value => value > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("Quantity must be greater than zero.")));
        }

        var total = selectedItems.Sum(item => item.Price * quantities[item.ItemId]);
        var order = new Order
        {
            ReservationId = reservation.ReservationId,
            EmployeeId = employee.EmployeeId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = total
        };

        await _unitOfWork.Orders.AddAsync(order);

        foreach (var item in selectedItems)
        {
            await _unitOfWork.OrderItems.AddAsync(new OrderItem
            {
                OrderId = order.OrderId,
                ItemId = item.ItemId,
                Quantity = quantities[item.ItemId]
            });
        }

        AnsiConsole.MarkupLine($"[green]Order #{order.OrderId} created with total {FormatMoney(total)}.[/]");
    }

    private async Task UpdateCustomerAsync()
    {
        var customer = await SelectCustomerAsync();
        if (customer is null)
            return;

        customer.FirstName = PromptRequired("First name:", 50, customer.FirstName);
        customer.LastName = PromptRequired("Last name:", 50, customer.LastName);
        customer.Email = PromptRequired("Email:", 100, customer.Email);
        customer.PhoneNumber = PromptRequired("Phone number:", 20, customer.PhoneNumber);

        await _unitOfWork.Customers.UpdateAsync(customer);
        AnsiConsole.MarkupLine("[green]Customer updated.[/]");
    }

    private async Task DeleteCustomerAsync()
    {
        var customer = await SelectCustomerAsync();
        if (customer is null)
            return;

        if (!AnsiConsole.Confirm($"Delete {FullName(customer)} and related reservations?"))
            return;

        await _unitOfWork.Customers.DeleteAsync(customer);
        AnsiConsole.MarkupLine("[green]Customer deleted.[/]");
    }

    private async Task<Customer?> SelectCustomerAsync()
    {
        var customers = (await _unitOfWork.Customers.GetAllAsync()).OrderBy(customer => customer.CustomerId).ToList();
        if (customers.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No customers found.[/]");
            return null;
        }

        return SelectEntity(customers, "Customer:", customer => $"#{customer.CustomerId} - {FullName(customer)}");
    }

    private async Task<Restaurant?> SelectRestaurantAsync()
    {
        var restaurants = (await _unitOfWork.Restaurants.GetAllAsync()).OrderBy(restaurant => restaurant.RestaurantId).ToList();
        if (restaurants.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No restaurants found.[/]");
            return null;
        }

        return SelectEntity(restaurants, "Restaurant:", restaurant => $"#{restaurant.RestaurantId} - {restaurant.Name}");
    }

    private async Task<Reservation?> SelectReservationAsync()
    {
        var reservations = await _context.Reservations
            .AsNoTracking()
            .Include(reservation => reservation.Customer)
            .Include(reservation => reservation.Restaurant)
            .OrderBy(reservation => reservation.ReservationId)
            .ToListAsync();

        if (reservations.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No reservations found.[/]");
            return null;
        }

        return SelectEntity(
            reservations,
            "Reservation:",
            reservation => $"#{reservation.ReservationId} - {FullName(reservation.Customer)} at {reservation.Restaurant.Name} on {FormatDate(reservation.ReservationDate)}");
    }

    private static TEntity SelectEntity<TEntity>(IEnumerable<TEntity> entities, string title, Func<TEntity, string> display)
        where TEntity : class
        => AnsiConsole.Prompt(
            new SelectionPrompt<TEntity>()
                .Title(title)
                .PageSize(12)
                .UseConverter(display)
                .AddChoices(entities));

    private static void ShowRestaurants(IEnumerable<Restaurant> restaurants)
    {
        var table = CreateTable("Restaurants", "Id", "Name", "Address", "Phone", "Hours");
        foreach (var restaurant in restaurants.OrderBy(restaurant => restaurant.RestaurantId))
        {
            table.AddRow(
                restaurant.RestaurantId.ToString(),
                restaurant.Name,
                restaurant.Address,
                restaurant.PhoneNumber ?? "",
                restaurant.OpeningHours ?? "");
        }

        AnsiConsole.Write(table);
    }

    private static void ShowCustomers(IEnumerable<Customer> customers)
    {
        var table = CreateTable("Customers", "Id", "Name", "Email", "Phone");
        foreach (var customer in customers.OrderBy(customer => customer.CustomerId))
            table.AddRow(customer.CustomerId.ToString(), FullName(customer), customer.Email, customer.PhoneNumber);

        AnsiConsole.Write(table);
    }

    private static void ShowTables(IEnumerable<RestaurantTable> tables)
    {
        var table = CreateTable("Tables", "Id", "Restaurant", "Capacity");
        foreach (var restaurantTable in tables.OrderBy(item => item.TableId))
            table.AddRow(restaurantTable.TableId.ToString(), restaurantTable.RestaurantId.ToString(), restaurantTable.Capacity.ToString());

        AnsiConsole.Write(table);
    }

    private static void ShowEmployees(IEnumerable<Employee> employees)
    {
        var table = CreateTable("Employees", "Id", "Name", "Position", "Restaurant");
        foreach (var employee in employees.OrderBy(employee => employee.EmployeeId))
            table.AddRow(employee.EmployeeId.ToString(), FullName(employee), employee.Position.ToString(), employee.RestaurantId.ToString());

        AnsiConsole.Write(table);
    }

    private static void ShowMenuItems(IEnumerable<MenuItem> menuItems)
    {
        var table = CreateTable("Menu Items", "Id", "Restaurant", "Name", "Price", "Description");
        foreach (var item in menuItems.OrderBy(item => item.ItemId))
            table.AddRow(item.ItemId.ToString(), item.RestaurantId.ToString(), item.Name, FormatMoney(item.Price), item.Description ?? "");

        AnsiConsole.Write(table);
    }

    private static SpectreTable CreateTable(string title, params string[] columns)
    {
        var table = new SpectreTable()
            .Title(title)
            .Border(TableBorder.Rounded)
            .Expand();

        foreach (var column in columns)
            table.AddColumn(new TableColumn(column).NoWrap());

        return table;
    }

    private static string PromptRequired(string prompt, int maxLength, string? defaultValue = null)
    {
        var textPrompt = new TextPrompt<string>(prompt)
            .Validate(value =>
            {
                if (string.IsNullOrWhiteSpace(value))
                    return ValidationResult.Error("Value is required.");

                return value.Length <= maxLength
                    ? ValidationResult.Success()
                    : ValidationResult.Error($"Maximum length is {maxLength}.");
            });

        if (defaultValue is not null)
            textPrompt.DefaultValue(defaultValue);

        return AnsiConsole.Prompt(textPrompt);
    }

    private static string? PromptOptional(string prompt, int maxLength)
    {
        var value = AnsiConsole.Ask<string?>($"{prompt} [grey](leave empty for none)[/]", null);
        return string.IsNullOrWhiteSpace(value) ? null : value.Length <= maxLength ? value : value[..maxLength];
    }

    private static DateTime PromptDate(string prompt, DateTime defaultValue, bool mustBeFuture)
        => AnsiConsole.Prompt(
            new TextPrompt<DateTime>(prompt)
                .DefaultValue(defaultValue)
                .Validate(value =>
                {
                    if (mustBeFuture && value < DateTime.UtcNow)
                        return ValidationResult.Error("Use a future UTC date and time.");

                    return ValidationResult.Success();
                }));

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

    private static string FullName(Customer customer) => $"{customer.FirstName} {customer.LastName}";

    private static string FullName(Employee employee) => $"{employee.FirstName} {employee.LastName}";

    private static string FormatDate(DateTime value) => value.ToString("yyyy-MM-dd HH:mm");

    private static string FormatMoney(decimal value) => value.ToString("C");

    private static void Pause()
    {
        AnsiConsole.MarkupLine("[grey]Press any key to continue.[/]");
        Console.ReadKey(intercept: true);
        AnsiConsole.Clear();
    }

    public async ValueTask DisposeAsync() => await _context.DisposeAsync();
}
