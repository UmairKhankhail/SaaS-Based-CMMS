using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaintenanceWebApi.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "methods",
                columns: table => new
                {
                    mtAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    mtName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    assetName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    downTimeValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_methods", x => x.mtAutoId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "procedures",
                columns: table => new
                {
                    pAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tom = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    assetName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_procedures", x => x.pAutoId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "methodSteps",
                columns: table => new
                {
                    msAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    mtAutoId = table.Column<int>(type: "int", nullable: false),
                    timeRequired = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    toolRequired = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_methodSteps", x => x.msAutoId);
                    table.ForeignKey(
                        name: "FK_methodSteps_methods_mtAutoId",
                        column: x => x.mtAutoId,
                        principalTable: "methods",
                        principalColumn: "mtAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "procedureHealthAndSafeties",
                columns: table => new
                {
                    hsAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pAutoId = table.Column<int>(type: "int", nullable: false),
                    cpName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_procedureHealthAndSafeties", x => x.hsAutoId);
                    table.ForeignKey(
                        name: "FK_procedureHealthAndSafeties_procedures_pAutoId",
                        column: x => x.pAutoId,
                        principalTable: "procedures",
                        principalColumn: "pAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "procedureMethods",
                columns: table => new
                {
                    pmAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pAuotId = table.Column<int>(type: "int", nullable: false),
                    pAutoId = table.Column<int>(type: "int", nullable: true),
                    pmName = table.Column<int>(type: "int", nullable: false),
                    subItemPosition = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_procedureMethods", x => x.pmAutoId);
                    table.ForeignKey(
                        name: "FK_procedureMethods_procedures_pAutoId",
                        column: x => x.pAutoId,
                        principalTable: "procedures",
                        principalColumn: "pAutoId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_methodSteps_mtAutoId",
                table: "methodSteps",
                column: "mtAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_procedureHealthAndSafeties_pAutoId",
                table: "procedureHealthAndSafeties",
                column: "pAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_procedureMethods_pAutoId",
                table: "procedureMethods",
                column: "pAutoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "methodSteps");

            migrationBuilder.DropTable(
                name: "procedureHealthAndSafeties");

            migrationBuilder.DropTable(
                name: "procedureMethods");

            migrationBuilder.DropTable(
                name: "methods");

            migrationBuilder.DropTable(
                name: "procedures");
        }
    }
}
