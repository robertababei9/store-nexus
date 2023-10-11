using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addtableStoreLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "LocationLatLng",
                table: "Store");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreLocationId",
                table: "Store",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StoreLocation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LatLng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreLocation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Store_StoreLocationId",
                table: "Store",
                column: "StoreLocationId",
                unique: true,
                filter: "[StoreLocationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_StoreLocation_StoreLocationId",
                table: "Store",
                column: "StoreLocationId",
                principalTable: "StoreLocation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Store_StoreLocation_StoreLocationId",
                table: "Store");

            migrationBuilder.DropTable(
                name: "StoreLocation");

            migrationBuilder.DropIndex(
                name: "IX_Store_StoreLocationId",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "StoreLocationId",
                table: "Store");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Store",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocationLatLng",
                table: "Store",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
