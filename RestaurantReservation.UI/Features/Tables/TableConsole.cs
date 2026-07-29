using RestaurantReservation.Infrastructure.Interfaces;
using RestaurantReservation.UI.Common;
using Spectre.Console;
using RestaurantTable = RestaurantReservation.Domain.Entities.Table;

namespace RestaurantReservation.UI.Features.Tables;

internal sealed class TableConsole(IUnitOfWork unitOfWork, EntitySelector selector)
{
    public async Task ListAsync()
    {
        var tables = await unitOfWork.Tables.GetAllAsync();
        if (Ui.ShowEmptyIfNeeded(tables, "tables"))
            return;

        var table = Ui.CreateTable("Tables", "Id", "Restaurant", "Capacity");

        foreach (var restaurantTable in tables.OrderBy(item => item.TableId))
            table.AddRow(restaurantTable.TableId.ToString(), restaurantTable.RestaurantId.ToString(), restaurantTable.Capacity.ToString());

        AnsiConsole.Write(table);
    }

    public async Task CreateAsync()
    {
        var restaurant = await selector.SelectRestaurantAsync();
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

        await unitOfWork.Tables.AddAsync(restaurantTable);
        AnsiConsole.MarkupLine($"[green]Table #{restaurantTable.TableId} created.[/]");
    }
}
