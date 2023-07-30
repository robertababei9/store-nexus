using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedrolestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Initial data for Roles
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] {"Id", "Name", "Description", "CreatedAt" },
                values: new object[,]
                {
                    { Guid.NewGuid(), "Admin", "The admin of the company", DateTime.Now },
                    { Guid.NewGuid(), "Manager",  "The person in charge of the store", DateTime.Now},
                    { Guid.NewGuid(), "User", "The user or the employee of the store" , DateTime.Now},
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
