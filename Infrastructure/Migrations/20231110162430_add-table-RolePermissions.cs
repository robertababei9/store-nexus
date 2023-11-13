using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addtableRolePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RolePermissionsId",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewDashboard = table.Column<bool>(type: "bit", nullable: false),
                    EditDashboard = table.Column<bool>(type: "bit", nullable: false),
                    ViewStore = table.Column<bool>(type: "bit", nullable: false),
                    EditStore = table.Column<bool>(type: "bit", nullable: false),
                    CreateStore = table.Column<bool>(type: "bit", nullable: false),
                    DeleteStore = table.Column<bool>(type: "bit", nullable: false),
                    ViewInvoice = table.Column<bool>(type: "bit", nullable: false),
                    CreateInvoice = table.Column<bool>(type: "bit", nullable: false),
                    ViewUser = table.Column<bool>(type: "bit", nullable: false),
                    EditUser = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<bool>(type: "bit", nullable: false),
                    DeleteUser = table.Column<bool>(type: "bit", nullable: false),
                    Settings = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                });

            // Populate RolePermissions table with default role permissions for ADMIN, MANAGER, USER
            // Not the best approach ... to many 0 and 1, too hard to understand
            // What if I add new properties ? Everything it's like 01110101011101010010 - what is what ?
            migrationBuilder.Sql(
                "INSERT INTO RolePermissions (Id, Name, " + 
                    " ViewDashboard, EditDashboard,"+
                    " ViewStore, CreateStore, EditStore, DeleteStore," +
                    " ViewInvoice, CreateInvoice," +
                    " ViewUser, EditUser, CreateUser, DeleteUser," + 
                    " Settings, CreatedAt, ModifiedAt)" +
                "VALUES " +
                "(NEWID(), 'Admin', 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, GETDATE(), null), " +  // ADMIN
                "(NEWID(), 'Manager', 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, GETDATE(), null), " +  // MANAGER
                "(NEWID(), 'User', 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, GETDATE(), null)" +  // USER
                ";");

            // Update RolePermissionsId
            migrationBuilder.Sql(
                "UPDATE Roles" +
                " SET RolePermissionsId =  (SELECT Id FROM RolePermissions rp WHERE rp.Name = 'Admin')" +
                " WHERE Name = 'Admin'" +
                " ;");

            migrationBuilder.Sql(
                " UPDATE Roles" +
                " SET RolePermissionsId =  (SELECT Id FROM RolePermissions rp WHERE rp.Name = 'Manager')" +
                " WHERE Name = 'Manager'" +
                " ;");

            migrationBuilder.Sql(
                " UPDATE Roles" +
                " SET RolePermissionsId =  (SELECT Id FROM RolePermissions rp WHERE rp.Name = 'User')" +
                " WHERE Name = 'User'" +
                " ;");


            migrationBuilder.CreateIndex(
                name: "IX_Roles_RolePermissionsId",
                table: "Roles",
                column: "RolePermissionsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_RolePermissions_RolePermissionsId",
                table: "Roles",
                column: "RolePermissionsId",
                principalTable: "RolePermissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_RolePermissions_RolePermissionsId",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_Roles_RolePermissionsId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "RolePermissionsId",
                table: "Roles");
        }
    }
}
