using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedStoreStatustable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "StoreStatus",
                columns: new[] { "Id", "StoreStatusType", "Description", "CreatedAt" },
                values: new object[,]
                {
                    {Guid.NewGuid(), 1, "Open", DateTime.Now},
                    {Guid.NewGuid(), 2, "Closed", DateTime.Now},
                    {Guid.NewGuid(), 3, "Temporarily  Closed", DateTime.Now},
                    {Guid.NewGuid(), 4, "Under Renovation", DateTime.Now},
                    {Guid.NewGuid(), 5, "Coming Soon", DateTime.Now},
                    {Guid.NewGuid(), 6, "Permanently Closed", DateTime.Now},
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE StoreStatus");
        }
    }
}
