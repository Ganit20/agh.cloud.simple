using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloud.core.database.Migrations
{
    /// <inheritdoc />
    public partial class SubscriptionSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "subscriptions",
                columns: new[] { "Id", "MaximmumSpace", "Name" },
                values: new object[] { 1, 107374182.4, "Test subscription" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "subscriptions",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
