using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class vw_ReservationsWithCustomerAndRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var viewSql =
                """
                CREATE VIEW [dbo].[vw_ReservationsWithCustomerAndRestaurant] AS
                SELECT [r].[ReservationId], [r].[ReservationDate], [r].[PartySize], [r].[TableId],
                       [c].[CustomerId], concat([c].[FirstName], ' ', [c].[LastName]) AS [CustomerName],
                       [c].[PhoneNumber] AS [CustomerPhoneNumber],
                       [res].[RestaurantId], [res].[Name] AS [RestaurantName], [res].[Address] AS [RestaurantAddress]
                FROM [dbo].[Reservations] AS [r]
                JOIN [dbo].[Customers] AS [c] ON [r].[CustomerId] = [c].[CustomerId]
                JOIN [dbo].[Restaurants] AS [res] ON [r].[RestaurantId] = [res].[RestaurantId]
                """;
            migrationBuilder.Sql(viewSql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
            => migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_ReservationsWithCustomerAndRestaurant]");
    }
}
