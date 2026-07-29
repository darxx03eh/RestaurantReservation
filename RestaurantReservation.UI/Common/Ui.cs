using RestaurantReservation.Domain.Entities;
using Spectre.Console;
using SpectreTable = Spectre.Console.Table;

namespace RestaurantReservation.UI.Common;

internal static class Ui
{
    public static SpectreTable CreateTable(string title, params string[] columns)
    {
        var table = new SpectreTable()
            .Title(title)
            .Border(TableBorder.Rounded)
            .Expand();

        foreach (var column in columns)
            table.AddColumn(new TableColumn(column).NoWrap());

        return table;
    }

    public static string PromptRequired(string prompt, int maxLength, string? defaultValue = null)
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

    public static string? PromptOptional(string prompt, int maxLength)
    {
        var value = AnsiConsole.Ask<string?>($"{prompt} [grey](leave empty for none)[/]", null);
        return string.IsNullOrWhiteSpace(value) ? null : value.Length <= maxLength ? value : value[..maxLength];
    }

    public static DateTime PromptDate(string prompt, DateTime defaultValue, bool mustBeFuture)
        => AnsiConsole.Prompt(
            new TextPrompt<DateTime>(prompt)
                .DefaultValue(defaultValue)
                .Validate(value =>
                {
                    if (mustBeFuture && value < DateTime.UtcNow)
                        return ValidationResult.Error("Use a future UTC date and time.");

                    return ValidationResult.Success();
                }));

    public static TEntity SelectEntity<TEntity>(IEnumerable<TEntity> entities, string title, Func<TEntity, string> display)
        where TEntity : class
        => AnsiConsole.Prompt(
            new SelectionPrompt<TEntity>()
                .Title(title)
                .PageSize(12)
                .UseConverter(display)
                .AddChoices(entities));

    public static string FullName(Customer customer) => $"{customer.FirstName} {customer.LastName}";

    public static string FullName(Employee employee) => $"{employee.FirstName} {employee.LastName}";

    public static string FormatDate(DateTime value) => value.ToString("yyyy-MM-dd HH:mm");

    public static string FormatMoney(decimal value) => value.ToString("C");

    public static bool ShowEmptyIfNeeded<T>(ICollection<T> rows, string name)
    {
        if (rows.Count > 0)
            return false;

        AnsiConsole.MarkupLine($"[yellow]No {name} found.[/]");
        return true;
    }

    public static void WriteError(Exception exception)
    {
        var message = Markup.Escape(exception.GetBaseException().Message);
        AnsiConsole.MarkupLine("[red]Action failed.[/]");
        AnsiConsole.MarkupLine($"[grey]{message}[/]");
    }

    public static void Pause()
    {
        AnsiConsole.MarkupLine("[grey]Press any key to continue.[/]");
        Console.ReadKey(intercept: true);
        AnsiConsole.Clear();
    }
}
