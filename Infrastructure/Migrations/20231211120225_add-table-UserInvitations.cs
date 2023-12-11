using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addtableUserInvitations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_RolePermissions_RolePermissionsId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_RolePermissionsId",
                table: "Roles");

            migrationBuilder.AlterColumn<Guid>(
                name: "RolePermissionsId",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "UserInvitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InviterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInvitations_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInvitations_Users_InviterId",
                        column: x => x.InviterId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RolePermissionsId",
                table: "Roles",
                column: "RolePermissionsId",
                unique: true,
                filter: "[RolePermissionsId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserInvitations_InviterId",
                table: "UserInvitations",
                column: "InviterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInvitations_RoleId",
                table: "UserInvitations",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_RolePermissions_RolePermissionsId",
                table: "Roles",
                column: "RolePermissionsId",
                principalTable: "RolePermissions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_RolePermissions_RolePermissionsId",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "UserInvitations");

            migrationBuilder.DropIndex(
                name: "IX_Roles_RolePermissionsId",
                table: "Roles");

            migrationBuilder.AlterColumn<Guid>(
                name: "RolePermissionsId",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

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
    }
}
