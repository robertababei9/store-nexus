using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterstorestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Store");

            migrationBuilder.RenameColumn(
                name: "StartHour",
                table: "Store",
                newName: "WorkingHours");

            migrationBuilder.RenameColumn(
                name: "EndHour",
                table: "Store",
                newName: "LocationLatLng");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkingHours",
                table: "Store",
                newName: "StartHour");

            migrationBuilder.RenameColumn(
                name: "LocationLatLng",
                table: "Store",
                newName: "EndHour");

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "Store",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
