using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Interfaces;
using RestaurantReservation.UI.Common;
using Spectre.Console;

namespace RestaurantReservation.UI.Features.MenuItems;

internal sealed class MenuItemConsole(IUnitOfWork unitOfWork, EntitySelector selector)
{
    public async Task ListAsync()
    {
        var menuItems = await unitOfWork.MenuItems.GetAllAsync();
        var table = Ui.CreateTable("Menu Items", "Id", "Restaurant", "Name", "Price", "Description");

        foreach (var item in menuItems.OrderBy(item => item.ItemId))
            table.AddRow(item.ItemId.ToString(), item.RestaurantId.ToString(), item.Name, Ui.FormatMoney(item.Price), item.Description ?? "");

        AnsiConsole.Write(table);
    }

    public async Task CreateAsync()
    {
        var restaurant = await selector.SelectRestaurantAsync();
        if (restaurant is null)
            return;

        var item = new MenuItem
        {
            RestaurantId = restaurant.RestaurantId,
            Name = Ui.PromptRequired("Name:", 100),
            Description = Ui.PromptOptional("Description:", 500) ?? string.Empty,
            Price = AnsiConsole.Prompt(
                new TextPrompt<decimal>("Price:")
                    .Validate(value => value > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("Price must be greater than zero.")))
        };

        await unitOfWork.MenuItems.AddAsync(item);
        AnsiConsole.MarkupLine($"[green]Menu item #{item.ItemId} created.[/]");
    }
}
