using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaintenanceWebApi.Migrations
{
    public partial class thirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "itemName",
                table: "wOItems",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "inhouseOrOutsource",
                table: "statusAndRepairs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "woAssetItems",
                columns: table => new
                {
                    woAssetItemAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    woAutoId = table.Column<int>(type: "int", nullable: false),
                    woAssetItemName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    woAssetItemType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    woAssetItemsApproval = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_woAssetItems", x => x.woAssetItemAutoId);
                    table.ForeignKey(
                        name: "FK_woAssetItems_workOrders_woAutoId",
                        column: x => x.woAutoId,
                        principalTable: "workOrders",
                        principalColumn: "woAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_woAssetItems_woAutoId",
                table: "woAssetItems",
                column: "woAutoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "woAssetItems");

            migrationBuilder.DropColumn(
                name: "itemName",
                table: "wOItems");

            migrationBuilder.DropColumn(
                name: "inhouseOrOutsource",
                table: "statusAndRepairs");
        }
    }
}
