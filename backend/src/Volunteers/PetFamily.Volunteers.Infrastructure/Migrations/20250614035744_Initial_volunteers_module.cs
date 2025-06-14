using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Volunteers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial_volunteers_module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Volunteers");

            migrationBuilder.CreateTable(
                name: "volunteers",
                schema: "Volunteers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    experience = table.Column<int>(type: "integer", nullable: false),
                    files = table.Column<string>(type: "jsonb", nullable: false),
                    social_networks = table.Column<string>(type: "jsonb", nullable: false),
                    details_for_help = table.Column<string>(type: "jsonb", nullable: false),
                    first_name = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    last_name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    surname = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    number = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    region_code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pets",
                schema: "Volunteers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    serial_number = table.Column<long>(type: "bigint", nullable: false),
                    age = table.Column<long>(type: "bigint", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    color = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    weight = table.Column<double>(type: "double precision", nullable: false),
                    height = table.Column<double>(type: "double precision", nullable: false),
                    sterilize = table.Column<bool>(type: "boolean", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vaccinated = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    details_for_help = table.Column<string>(type: "jsonb", nullable: false),
                    files = table.Column<string>(type: "jsonb", nullable: false),
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    apartment_number = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    country = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                    house_number = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    street = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    digestive_system_condition = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    mouth_condition = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    shared_health_status = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    skin_condition = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    region_code = table.Column<string>(type: "text", nullable: false),
                    breed_id = table.Column<Guid>(type: "uuid", nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pets", x => x.id);
                    table.ForeignKey(
                        name: "fk_pets_volunteers_volunteer_id",
                        column: x => x.volunteer_id,
                        principalSchema: "Volunteers",
                        principalTable: "volunteers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_pets_volunteer_id",
                schema: "Volunteers",
                table: "pets",
                column: "volunteer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pets",
                schema: "Volunteers");

            migrationBuilder.DropTable(
                name: "volunteers",
                schema: "Volunteers");
        }
    }
}
