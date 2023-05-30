using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetWebApi.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_equipmentSubItems_equipmentModels_eAuotId",
                table: "equipmentSubItems");

            migrationBuilder.DropIndex(
                name: "IX_equipmentSubItems_eAuotId",
                table: "equipmentSubItems");

            migrationBuilder.DropColumn(
                name: "eAuotId",
                table: "equipmentSubItems");

            migrationBuilder.DropColumn(
                name: "esId",
                table: "equipmentSubItems");

            migrationBuilder.CreateIndex(
                name: "IX_equipmentSubItems_eAutoId",
                table: "equipmentSubItems",
                column: "eAutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_equipmentSubItems_equipmentModels_eAutoId",
                table: "equipmentSubItems",
                column: "eAutoId",
                principalTable: "equipmentModels",
                principalColumn: "eAutoId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_equipmentSubItems_equipmentModels_eAutoId",
                table: "equipmentSubItems");

            migrationBuilder.DropIndex(
                name: "IX_equipmentSubItems_eAutoId",
                table: "equipmentSubItems");

            migrationBuilder.AddColumn<int>(
                name: "eAuotId",
                table: "equipmentSubItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "esId",
                table: "equipmentSubItems",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_equipmentSubItems_eAuotId",
                table: "equipmentSubItems",
                column: "eAuotId");

            migrationBuilder.AddForeignKey(
                name: "FK_equipmentSubItems_equipmentModels_eAuotId",
                table: "equipmentSubItems",
                column: "eAuotId",
                principalTable: "equipmentModels",
                principalColumn: "eAutoId");
        }
    }
}
