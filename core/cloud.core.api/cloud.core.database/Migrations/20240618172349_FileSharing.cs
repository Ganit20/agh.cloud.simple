using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloud.core.database.Migrations
{
    /// <inheritdoc />
    public partial class FileSharing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "file_share_links",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file_share_links", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "subscriptions",
                keyColumn: "Id",
                keyValue: 1,
                column: "MaximmumSpace",
                value: 107374182.40000001);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "file_share_links");

            migrationBuilder.UpdateData(
                table: "subscriptions",
                keyColumn: "Id",
                keyValue: 1,
                column: "MaximmumSpace",
                value: 1000000.0);
        }
    }
}
