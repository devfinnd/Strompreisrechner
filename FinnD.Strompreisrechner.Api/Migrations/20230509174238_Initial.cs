using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinnD.Strompreisrechner.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "history_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    kwh_per_year = table.Column<decimal>(type: "TEXT", nullable: false),
                    price_per_kwh = table.Column<decimal>(type: "TEXT", nullable: false),
                    price_per_year = table.Column<decimal>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_history_entries", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "history_entries");
        }
    }
}
