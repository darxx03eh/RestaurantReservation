using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Entities.Procedures;
using RestaurantReservation.Infrastructure.Db;
using RestaurantReservation.Infrastructure.Interfaces;
using RestaurantReservation.UI.Common;
using Spectre.Console;

namespace RestaurantReservation.UI.Features.Customers;

internal sealed class CustomerConsole(
    RestaurantReservationDbContext context,
    IUnitOfWork unitOfWork,
    EntitySelector selector)
{
    public async Task ListAsync()
    {
        var customers = await unitOfWork.Customers.GetAllAsync();
        if (Ui.ShowEmptyIfNeeded(customers, "customers"))
            return;

        var table = Ui.CreateTable("Customers", "Id", "Name", "Email", "Phone");

        foreach (var customer in customers.OrderBy(customer => customer.CustomerId))
            table.AddRow(customer.CustomerId.ToString(), Ui.FullName(customer), customer.Email, customer.PhoneNumber);

        AnsiConsole.Write(table);
    }

    public async Task ShowByPartySizeAsync()
    {
        var partySize = AnsiConsole.Prompt(
            new TextPrompt<int>("Minimum party size:")
                .Validate(value => value >= 0 ? ValidationResult.Success() : ValidationResult.Error("Use zero or more.")));

        var rows = await context.Set<CustomerReservationProcedure>()
            .FromSqlInterpolated($"EXEC dbo.sp_GetCustomersByPartySize {partySize}")
            .AsNoTracking()
            .ToListAsync();

        if (Ui.ShowEmptyIfNeeded(rows, "customers matching this party size"))
            return;

        var table = Ui.CreateTable("Stored Procedure Result", "Id", "Name", "Email", "Phone");
        foreach (var row in rows)
            table.AddRow(row.CustomerId.ToString(), row.FullName, row.Email, row.PhoneNumber);

        AnsiConsole.Write(table);
    }

    public async Task CreateAsync()
    {
        var customer = new Customer
        {
            FirstName = Ui.PromptRequired("First name:", 50),
            LastName = Ui.PromptRequired("Last name:", 50),
            Email = Ui.PromptRequired("Email:", 100),
            PhoneNumber = Ui.PromptRequired("Phone number:", 20)
        };

        await unitOfWork.Customers.AddAsync(customer);
        AnsiConsole.MarkupLine($"[green]Customer #{customer.CustomerId} created.[/]");
    }

    public async Task UpdateAsync()
    {
        var customer = await selector.SelectCustomerAsync();
        if (customer is null)
            return;

        customer.FirstName = Ui.PromptRequired("First name:", 50, customer.FirstName);
        customer.LastName = Ui.PromptRequired("Last name:", 50, customer.LastName);
        customer.Email = Ui.PromptRequired("Email:", 100, customer.Email);
        customer.PhoneNumber = Ui.PromptRequired("Phone number:", 20, customer.PhoneNumber);

        await unitOfWork.Customers.UpdateAsync(customer);
        AnsiConsole.MarkupLine("[green]Customer updated.[/]");
    }

    public async Task DeleteAsync()
    {
        var customer = await selector.SelectCustomerAsync();
        if (customer is null)
            return;

        if (!AnsiConsole.Confirm($"Delete {Ui.FullName(customer)} and related reservations?"))
            return;

        await unitOfWork.Customers.DeleteAsync(customer);
        AnsiConsole.MarkupLine("[green]Customer deleted.[/]");
    }
}
