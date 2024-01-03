using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterstorestableaddcompanyId : Migration
    {

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the scalar function to get the default value
            migrationBuilder.Sql(@"
                CREATE FUNCTION dbo.GetDefaultCompanyId()
                RETURNS uniqueidentifier
                AS
                BEGIN
                    RETURN (SELECT TOP 1 Id FROM Company);
                END;
            ");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Store",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "dbo.GetDefaultCompanyId()");

            migrationBuilder.CreateIndex(
                name: "IX_Store_CompanyId",
                table: "Store",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_Company_CompanyId",
                table: "Store",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Store_Company_CompanyId",
                table: "Store");

            migrationBuilder.DropIndex(
                name: "IX_Store_CompanyId",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Store");
        }
    }
}
