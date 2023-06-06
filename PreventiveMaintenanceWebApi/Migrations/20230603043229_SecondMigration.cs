using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PreventiveMaintenanceWebApi.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "eventIdCalendar",
                table: "scheduledWorkRequests",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "eventIdCalendar",
                table: "meterReadings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "eventIdCalendar",
                table: "inspections",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "eventIdCalendar",
                table: "scheduledWorkRequests");

            migrationBuilder.DropColumn(
                name: "eventIdCalendar",
                table: "meterReadings");

            migrationBuilder.DropColumn(
                name: "eventIdCalendar",
                table: "inspections");
        }
    }
}
