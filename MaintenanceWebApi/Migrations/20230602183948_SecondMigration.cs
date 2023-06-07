using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaintenanceWebApi.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "statusAndRepairItems");

            migrationBuilder.AddColumn<string>(
                name: "cost",
                table: "wOItems",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "faultyNotFaulty",
                table: "statusAndRepairs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "itemName",
                table: "statusAndRepairs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "woAutoId",
                table: "statusAndRepairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "worker",
                table: "statusAndRepairs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_statusAndRepairs_woAutoId",
                table: "statusAndRepairs",
                column: "woAutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_statusAndRepairs_workOrders_woAutoId",
                table: "statusAndRepairs",
                column: "woAutoId",
                principalTable: "workOrders",
                principalColumn: "woAutoId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_statusAndRepairs_workOrders_woAutoId",
                table: "statusAndRepairs");

            migrationBuilder.DropIndex(
                name: "IX_statusAndRepairs_woAutoId",
                table: "statusAndRepairs");

            migrationBuilder.DropColumn(
                name: "cost",
                table: "wOItems");

            migrationBuilder.DropColumn(
                name: "faultyNotFaulty",
                table: "statusAndRepairs");

            migrationBuilder.DropColumn(
                name: "itemName",
                table: "statusAndRepairs");

            migrationBuilder.DropColumn(
                name: "woAutoId",
                table: "statusAndRepairs");

            migrationBuilder.DropColumn(
                name: "worker",
                table: "statusAndRepairs");

            migrationBuilder.CreateTable(
                name: "statusAndRepairItems",
                columns: table => new
                {
                    sriAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    srAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    faultyNotFaulty = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    itemName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusAndRepairItems", x => x.sriAutoId);
                    table.ForeignKey(
                        name: "FK_statusAndRepairItems_statusAndRepairs_srAutoId",
                        column: x => x.srAutoId,
                        principalTable: "statusAndRepairs",
                        principalColumn: "srAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_statusAndRepairItems_srAutoId",
                table: "statusAndRepairItems",
                column: "srAutoId");
        }
    }
}
