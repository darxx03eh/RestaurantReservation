using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fn_GetRestaurantTotalRevenue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var functionSql =
                """
                CREATE FUNCTION [dbo].[fn_GetRestaurantTotalRevenue]
                    (@RestaurantId INT) RETURNS DECIMAL(18, 2)
                AS
                    BEGIN
                        DECLARE @TotalRevenue DECIMAL(18, 2)
                        SELECT @TotalRevenue = SUM([oi].Quantity * [i].Price)
                        FROM [dbo].[Orders] AS [o]
                        JOIN [dbo].[Reservations] AS [r] ON [o].[ReservationId] = [r].[ReservationId]
                        JOIN [dbo].[OrderItems] AS [oi] ON [o].[OrderId] = [oi].[OrderId]
                        JOIN [dbo].[MenuItems] AS [i] ON [oi].[ItemId] = [i].[ItemId]
                        WHERE [r].[RestaurantId] = @RestaurantId
                        RETURN ISNULL(@TotalRevenue, 0)
                    END
                """;
            migrationBuilder.Sql(functionSql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
            => migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fn_GetRestaurantTotalRevenue]");
    }
}
