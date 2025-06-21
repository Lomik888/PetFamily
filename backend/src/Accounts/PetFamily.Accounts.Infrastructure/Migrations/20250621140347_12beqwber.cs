using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Accounts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _12beqwber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_roles_RoleId",
                schema: "Accounts",
                table: "users");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                schema: "Accounts",
                table: "users",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_users_roles_RoleId",
                schema: "Accounts",
                table: "users",
                column: "RoleId",
                principalSchema: "Accounts",
                principalTable: "roles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_roles_RoleId",
                schema: "Accounts",
                table: "users");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                schema: "Accounts",
                table: "users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_users_roles_RoleId",
                schema: "Accounts",
                table: "users",
                column: "RoleId",
                principalSchema: "Accounts",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
