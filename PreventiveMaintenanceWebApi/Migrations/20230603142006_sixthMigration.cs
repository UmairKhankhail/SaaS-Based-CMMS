using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PreventiveMaintenanceWebApi.Migrations
{
    public partial class sixthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_googleCalendarRecords",
                table: "googleCalendarRecords");

            migrationBuilder.AlterColumn<string>(
                name: "googleCalendarId",
                table: "googleCalendarRecords",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "googleCalendarAutoId",
                table: "googleCalendarRecords",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_googleCalendarRecords",
                table: "googleCalendarRecords",
                column: "googleCalendarAutoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_googleCalendarRecords",
                table: "googleCalendarRecords");

            migrationBuilder.DropColumn(
                name: "googleCalendarAutoId",
                table: "googleCalendarRecords");

            migrationBuilder.AlterColumn<int>(
                name: "googleCalendarId",
                table: "googleCalendarRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_googleCalendarRecords",
                table: "googleCalendarRecords",
                column: "googleCalendarId");
        }
    }
}
