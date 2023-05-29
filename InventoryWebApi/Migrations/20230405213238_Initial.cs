using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    brandAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    brandId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    brandName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands", x => x.brandAutoId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Inventorygroups",
                columns: table => new
                {
                    groupAutoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    groupID = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    groupName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventorygroups", x => x.groupAutoID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "issuences",
                columns: table => new
                {
                    issuenceAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    issuenceId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    qty = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    issuenceDescp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issuences", x => x.issuenceAutoId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "purchases",
                columns: table => new
                {
                    purchaseAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    purchaseId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    qty = table.Column<int>(type: "int", nullable: false),
                    userAutoId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    purchasesDescp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchases", x => x.purchaseAutoId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "unitOfMeasurements",
                columns: table => new
                {
                    uomAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    uomId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    uomName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unitOfMeasurements", x => x.uomAutoId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    catAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    catId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    catName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    groupAutoId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.catAutoId);
                    table.ForeignKey(
                        name: "FK_categories_Inventorygroups_groupAutoId",
                        column: x => x.groupAutoId,
                        principalTable: "Inventorygroups",
                        principalColumn: "groupAutoID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "equipments",
                columns: table => new
                {
                    equipAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    equipId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    equipName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    equipCost = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    equipLeadTime = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    catAutoId = table.Column<int>(type: "int", nullable: false),
                    groupAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipments", x => x.equipAutoId);
                    table.ForeignKey(
                        name: "FK_equipments_categories_catAutoId",
                        column: x => x.catAutoId,
                        principalTable: "categories",
                        principalColumn: "catAutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_equipments_Inventorygroups_groupAutoId",
                        column: x => x.groupAutoId,
                        principalTable: "Inventorygroups",
                        principalColumn: "groupAutoID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "issuenceandEquipment",
                columns: table => new
                {
                    issueEquipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    issuenceAutoId = table.Column<int>(type: "int", nullable: false),
                    equipAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issuenceandEquipment", x => x.issueEquipId);
                    table.ForeignKey(
                        name: "FK_issuenceandEquipment_equipments_equipAutoId",
                        column: x => x.equipAutoId,
                        principalTable: "equipments",
                        principalColumn: "equipAutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_issuenceandEquipment_issuences_issuenceAutoId",
                        column: x => x.issuenceAutoId,
                        principalTable: "issuences",
                        principalColumn: "issuenceAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "purchaseandEquipment",
                columns: table => new
                {
                    purchaseEquipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    purchaseAutoId = table.Column<int>(type: "int", nullable: false),
                    equipAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchaseandEquipment", x => x.purchaseEquipId);
                    table.ForeignKey(
                        name: "FK_purchaseandEquipment_equipments_equipAutoId",
                        column: x => x.equipAutoId,
                        principalTable: "equipments",
                        principalColumn: "equipAutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_purchaseandEquipment_purchases_purchaseAutoId",
                        column: x => x.purchaseAutoId,
                        principalTable: "purchases",
                        principalColumn: "purchaseAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_categories_groupAutoId",
                table: "categories",
                column: "groupAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_equipments_catAutoId",
                table: "equipments",
                column: "catAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_equipments_groupAutoId",
                table: "equipments",
                column: "groupAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_issuenceandEquipment_equipAutoId",
                table: "issuenceandEquipment",
                column: "equipAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_issuenceandEquipment_issuenceAutoId",
                table: "issuenceandEquipment",
                column: "issuenceAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_purchaseandEquipment_equipAutoId",
                table: "purchaseandEquipment",
                column: "equipAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_purchaseandEquipment_purchaseAutoId",
                table: "purchaseandEquipment",
                column: "purchaseAutoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropTable(
                name: "issuenceandEquipment");

            migrationBuilder.DropTable(
                name: "purchaseandEquipment");

            migrationBuilder.DropTable(
                name: "unitOfMeasurements");

            migrationBuilder.DropTable(
                name: "issuences");

            migrationBuilder.DropTable(
                name: "equipments");

            migrationBuilder.DropTable(
                name: "purchases");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "Inventorygroups");
        }
    }
}
