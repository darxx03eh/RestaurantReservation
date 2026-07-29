using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities.Views;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.Interfaces;
using RestaurantReservation.UI.Common;
using Spectre.Console;

namespace RestaurantReservation.UI.Features.Employees;

internal sealed class EmployeeConsole(RestaurantReservationDbContext context, IUnitOfWork unitOfWork)
{
    public async Task ListAsync()
    {
        var employees = await unitOfWork.Employees.GetAllAsync();
        var table = Ui.CreateTable("Employees", "Id", "Name", "Position", "Restaurant");

        foreach (var employee in employees.OrderBy(employee => employee.EmployeeId))
            table.AddRow(employee.EmployeeId.ToString(), Ui.FullName(employee), employee.Position.ToString(), employee.RestaurantId.ToString());

        AnsiConsole.Write(table);
    }

    public async Task ShowRestaurantDetailsViewAsync()
    {
        var rows = await context.Set<EmployeeRestaurantDetailsView>()
            .AsNoTracking()
            .OrderBy(row => row.RestauranName)
            .ThenBy(row => row.EmployeeName)
            .ToListAsync();

        var table = Ui.CreateTable("Employee Restaurant Details View", "Employee", "Position", "Restaurant", "Phone", "Hours");
        foreach (var row in rows)
            table.AddRow(row.EmployeeName, row.Position, row.RestauranName, row.RestauranPhoneNumber, row.OpeningHours);

        AnsiConsole.Write(table);
    }
}
