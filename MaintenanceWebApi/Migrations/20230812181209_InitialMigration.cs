using System;
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
                name: "workOrders",
                columns: table => new
                {
                    woAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    woId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    woType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    topName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    requestId = table.Column<int>(type: "int", nullable: false),
                    assetDetials = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workOrders", x => x.woAutoId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "workRequests",
                columns: table => new
                {
                    wrAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    topName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    approve = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workRequests", x => x.wrAutoId);
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
                    phsAutoId = table.Column<int>(type: "int", nullable: false)
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
                    table.PrimaryKey("PK_procedureHealthAndSafeties", x => x.phsAutoId);
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
                    pAutoId = table.Column<int>(type: "int", nullable: false),
                    pmName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
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
                        principalColumn: "pAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "evaluations",
                columns: table => new
                {
                    evaluationAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    woAutoId = table.Column<int>(type: "int", nullable: false),
                    evaluationId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    topName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    startTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    endTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    remarks = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    eventId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluations", x => x.evaluationAutoId);
                    table.ForeignKey(
                        name: "FK_evaluations_workOrders_woAutoId",
                        column: x => x.woAutoId,
                        principalTable: "workOrders",
                        principalColumn: "woAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "executions",
                columns: table => new
                {
                    executionAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    woAutoId = table.Column<int>(type: "int", nullable: false),
                    executionId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    topName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    startTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    endTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    remarks = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    eventId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_executions", x => x.executionAutoId);
                    table.ForeignKey(
                        name: "FK_executions_workOrders_woAutoId",
                        column: x => x.woAutoId,
                        principalTable: "workOrders",
                        principalColumn: "woAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "healthAndSafeties",
                columns: table => new
                {
                    hsAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    woAutoId = table.Column<int>(type: "int", nullable: false),
                    userName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    remarks = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_healthAndSafeties", x => x.hsAutoId);
                    table.ForeignKey(
                        name: "FK_healthAndSafeties_workOrders_woAutoId",
                        column: x => x.woAutoId,
                        principalTable: "workOrders",
                        principalColumn: "woAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "instructions",
                columns: table => new
                {
                    insAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pAutoId = table.Column<int>(type: "int", nullable: false),
                    woAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instructions", x => x.insAutoId);
                    table.ForeignKey(
                        name: "FK_instructions_procedures_pAutoId",
                        column: x => x.pAutoId,
                        principalTable: "procedures",
                        principalColumn: "pAutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_instructions_workOrders_woAutoId",
                        column: x => x.woAutoId,
                        principalTable: "workOrders",
                        principalColumn: "woAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "statusAndRepairs",
                columns: table => new
                {
                    srAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    srId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    woAutoId = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    itemName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    faultyNotFaulty = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    inhouseOrOutsource = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    worker = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusAndRepairs", x => x.srAutoId);
                    table.ForeignKey(
                        name: "FK_statusAndRepairs_workOrders_woAutoId",
                        column: x => x.woAutoId,
                        principalTable: "workOrders",
                        principalColumn: "woAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
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

            migrationBuilder.CreateTable(
                name: "wOItems",
                columns: table => new
                {
                    woItemsAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    woItemsId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    woAutoId = table.Column<int>(type: "int", nullable: false),
                    itemAutoId = table.Column<int>(type: "int", nullable: false),
                    itemName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    stock = table.Column<int>(type: "int", nullable: false),
                    userAutoId = table.Column<int>(type: "int", nullable: false),
                    cost = table.Column<int>(type: "int", nullable: false),
                    requestStatus = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    itemDescp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wOItems", x => x.woItemsAutoId);
                    table.ForeignKey(
                        name: "FK_wOItems_workOrders_woAutoId",
                        column: x => x.woAutoId,
                        principalTable: "workOrders",
                        principalColumn: "woAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HealthAndSafetyItems",
                columns: table => new
                {
                    hsiAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hsAutoId = table.Column<int>(type: "int", nullable: false),
                    phsAutoId = table.Column<int>(type: "int", nullable: false),
                    woAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthAndSafetyItems", x => x.hsiAutoId);
                    table.ForeignKey(
                        name: "FK_HealthAndSafetyItems_healthAndSafeties_hsAutoId",
                        column: x => x.hsAutoId,
                        principalTable: "healthAndSafeties",
                        principalColumn: "hsAutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HealthAndSafetyItems_procedureHealthAndSafeties_phsAutoId",
                        column: x => x.phsAutoId,
                        principalTable: "procedureHealthAndSafeties",
                        principalColumn: "phsAutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HealthAndSafetyItems_workOrders_woAutoId",
                        column: x => x.woAutoId,
                        principalTable: "workOrders",
                        principalColumn: "woAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_evaluations_woAutoId",
                table: "evaluations",
                column: "woAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_executions_woAutoId",
                table: "executions",
                column: "woAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_healthAndSafeties_woAutoId",
                table: "healthAndSafeties",
                column: "woAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthAndSafetyItems_hsAutoId",
                table: "HealthAndSafetyItems",
                column: "hsAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthAndSafetyItems_phsAutoId",
                table: "HealthAndSafetyItems",
                column: "phsAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthAndSafetyItems_woAutoId",
                table: "HealthAndSafetyItems",
                column: "woAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_instructions_pAutoId",
                table: "instructions",
                column: "pAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_instructions_woAutoId",
                table: "instructions",
                column: "woAutoId");

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

            migrationBuilder.CreateIndex(
                name: "IX_statusAndRepairs_woAutoId",
                table: "statusAndRepairs",
                column: "woAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_woAssetItems_woAutoId",
                table: "woAssetItems",
                column: "woAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_wOItems_woAutoId",
                table: "wOItems",
                column: "woAutoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "evaluations");

            migrationBuilder.DropTable(
                name: "executions");

            migrationBuilder.DropTable(
                name: "HealthAndSafetyItems");

            migrationBuilder.DropTable(
                name: "instructions");

            migrationBuilder.DropTable(
                name: "methodSteps");

            migrationBuilder.DropTable(
                name: "procedureMethods");

            migrationBuilder.DropTable(
                name: "statusAndRepairs");

            migrationBuilder.DropTable(
                name: "woAssetItems");

            migrationBuilder.DropTable(
                name: "wOItems");

            migrationBuilder.DropTable(
                name: "workRequests");

            migrationBuilder.DropTable(
                name: "healthAndSafeties");

            migrationBuilder.DropTable(
                name: "procedureHealthAndSafeties");

            migrationBuilder.DropTable(
                name: "methods");

            migrationBuilder.DropTable(
                name: "workOrders");

            migrationBuilder.DropTable(
                name: "procedures");
        }
    }
}
