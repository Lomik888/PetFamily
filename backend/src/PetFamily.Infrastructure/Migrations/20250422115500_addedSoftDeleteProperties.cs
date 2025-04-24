using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedSoftDeleteProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "is_active",
                table: "volunteers",
                type: "boolean",
                nullable: false,
                defaultValueSql: "TRUE");

            migrationBuilder.AddColumn<string>(
                name: "deleted_at",
                table: "volunteers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "is_active",
                table: "pets",
                type: "boolean",
                nullable: false,
                defaultValueSql: "TRUE");

            migrationBuilder.AddColumn<string>(
                name: "deleted_at",
                table: "pets",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                table: "pets",
                name: "is_active");

            migrationBuilder.DropColumn(
                table: "pets",
                name: "deleted_at");

            migrationBuilder.DropColumn(
                table: "volunteers",
                name: "is_active");

            migrationBuilder.DropColumn(
                table: "volunteers",
                name: "deleted_at");
        }
    }
}