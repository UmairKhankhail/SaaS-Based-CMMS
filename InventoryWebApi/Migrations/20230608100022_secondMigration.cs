using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryAPI.Migrations
{
    public partial class secondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "qty",
                table: "purchases");

            migrationBuilder.DropColumn(
                name: "qty",
                table: "issuences");

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "purchaseandEquipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "issuenceandEquipment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "quantity",
                table: "purchaseandEquipment");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "issuenceandEquipment");

            migrationBuilder.AddColumn<int>(
                name: "qty",
                table: "purchases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "qty",
                table: "issuences",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
