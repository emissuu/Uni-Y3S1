using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "flights",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    flight_number = table.Column<string>(type: "varchar(6)", nullable: false),
                    origin = table.Column<string>(type: "varchar(4)", nullable: false),
                    destination = table.Column<string>(type: "varchar(4)", nullable: false),
                    departure_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    arrival_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    seats_count = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_flights", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_flights_flight_number",
                table: "flights",
                column: "flight_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "flights");
        }
    }
}
