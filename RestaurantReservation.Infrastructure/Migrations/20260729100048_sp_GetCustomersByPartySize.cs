using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sp_GetCustomersByPartySize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var procedureSql =
                """
                CREATE PROCEDURE [dbo].[sp_GetCustomersByPartySize]
                (@PartySize INT) AS
                BEGIN
                    SELECT [c].[CustomerId], CONCAT([c].[FirstName], ' ', [c].[LastName]) AS [FullName],
                           [c].[Email], [c].[PhoneNumber]
                    FROM [dbo].[Customers] AS [c]
                    JOIN [dbo].[Reservations] AS [r] ON [c].[CustomerId] = [r].[CustomerId]
                    WHERE [r].[PartySize] > @PartySize
                END
                """;
            migrationBuilder.Sql(procedureSql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
            => migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_GetCustomersByPartySize]");
    }
}
