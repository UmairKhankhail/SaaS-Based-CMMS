using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetWebApi.Migrations
{
    public partial class thirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "lsName",
                table: "linearSubItems",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lsName",
                table: "linearSubItems");
        }
    }
}
