using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class vw_EmployeeRestaurantDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var viewSql =
                """
                CREATE VIEW [dbo].[vw_EmployeeRestaurantDetails] AS
                SELECT [e].[EmployeeId], CONCAT([e].[FirstName], ' ', [e].[LastName]) AS [EmployeeName],
                       [e].[Position], [r].[RestaurantId], [r].[Name] AS [RestauranName],
                       [r].[Address] AS [RestauranAddress], [r].[PhoneNumber] AS [RestauranPhoneNumber],
                       [r].[OpeningHours]
                FROM [dbo].[Employees] AS [e]
                JOIN [dbo].[Restaurants] AS [r] ON [e].[RestaurantId] = [r].[RestaurantId]
                """;
            migrationBuilder.Sql(viewSql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
            => migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_EmployeeRestaurantDetails]");
    }
}
