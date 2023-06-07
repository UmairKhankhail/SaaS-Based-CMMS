using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PreventiveMaintenanceWebApi.Migrations
{
    public partial class seventhMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "timeZone",
                table: "googleCalendarRecords",
                newName: "timeZoneWord");

            migrationBuilder.AddColumn<string>(
                name: "timeZoneGMT",
                table: "googleCalendarRecords",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "timeZoneRegional",
                table: "googleCalendarRecords",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "timeZoneGMT",
                table: "googleCalendarRecords");

            migrationBuilder.DropColumn(
                name: "timeZoneRegional",
                table: "googleCalendarRecords");

            migrationBuilder.RenameColumn(
                name: "timeZoneWord",
                table: "googleCalendarRecords",
                newName: "timeZone");
        }
    }
}
