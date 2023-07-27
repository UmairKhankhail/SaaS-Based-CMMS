using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetWebApi.Migrations
{
    public partial class finaltestnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "lsParentId",
                table: "linearSubItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lsParentId",
                table: "linearSubItems");
        }
    }
}
