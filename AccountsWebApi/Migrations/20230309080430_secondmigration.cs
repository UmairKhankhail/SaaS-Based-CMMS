using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountsWebApi.Migrations
{
    public partial class secondmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "facilities",
                columns: table => new
                {
                    facilityautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    facilityid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    facilityname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_facilities", x => x.facilityautoid);
                    table.ForeignKey(
                        name: "FK_facilities_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "methodtypes",
                columns: table => new
                {
                    mtautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    mtid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mtname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_methodtypes", x => x.mtautoid);
                    table.ForeignKey(
                        name: "FK_methodtypes_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "positions",
                columns: table => new
                {
                    positionautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    positionid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    positionname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_positions", x => x.positionautoid);
                    table.ForeignKey(
                        name: "FK_positions_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "priorities",
                columns: table => new
                {
                    priorityautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    priorityid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    priorityname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_priorities", x => x.priorityautoid);
                    table.ForeignKey(
                        name: "FK_priorities_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "profiles",
                columns: table => new
                {
                    profileautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    profileid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    profilename = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profiles", x => x.profileautoid);
                    table.ForeignKey(
                        name: "FK_profiles_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tools",
                columns: table => new
                {
                    toolautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    toolid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    toolname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tools", x => x.toolautoid);
                    table.ForeignKey(
                        name: "FK_tools_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "typeofmaintenances",
                columns: table => new
                {
                    tomautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tomid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tomname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typeofmaintenances", x => x.tomautoid);
                    table.ForeignKey(
                        name: "FK_typeofmaintenances_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "typesofproblems",
                columns: table => new
                {
                    topautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    topid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    topname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typesofproblems", x => x.topautoid);
                    table.ForeignKey(
                        name: "FK_typesofproblems_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "floors",
                columns: table => new
                {
                    floorautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    floorid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    floorname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    facilityautoid = table.Column<int>(type: "int", nullable: false),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_floors", x => x.floorautoid);
                    table.ForeignKey(
                        name: "FK_floors_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_floors_facilities_facilityautoid",
                        column: x => x.facilityautoid,
                        principalTable: "facilities",
                        principalColumn: "facilityautoid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "functionallocations",
                columns: table => new
                {
                    flautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    flid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    flname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    facilityautoid = table.Column<int>(type: "int", nullable: false),
                    floorautoid = table.Column<int>(type: "int", nullable: false),
                    subdeptautoid = table.Column<int>(type: "int", nullable: false),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_functionallocations", x => x.flautoid);
                    table.ForeignKey(
                        name: "FK_functionallocations_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_functionallocations_facilities_facilityautoid",
                        column: x => x.facilityautoid,
                        principalTable: "facilities",
                        principalColumn: "facilityautoid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_functionallocations_floors_floorautoid",
                        column: x => x.floorautoid,
                        principalTable: "floors",
                        principalColumn: "floorautoid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_functionallocations_sub_departments_subdeptautoid",
                        column: x => x.subdeptautoid,
                        principalTable: "sub_departments",
                        principalColumn: "subdeptautoid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_facilities_companyid",
                table: "facilities",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_floors_companyid",
                table: "floors",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_floors_facilityautoid",
                table: "floors",
                column: "facilityautoid");

            migrationBuilder.CreateIndex(
                name: "IX_functionallocations_companyid",
                table: "functionallocations",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_functionallocations_facilityautoid",
                table: "functionallocations",
                column: "facilityautoid");

            migrationBuilder.CreateIndex(
                name: "IX_functionallocations_floorautoid",
                table: "functionallocations",
                column: "floorautoid");

            migrationBuilder.CreateIndex(
                name: "IX_functionallocations_subdeptautoid",
                table: "functionallocations",
                column: "subdeptautoid");

            migrationBuilder.CreateIndex(
                name: "IX_methodtypes_companyid",
                table: "methodtypes",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_positions_companyid",
                table: "positions",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_priorities_companyid",
                table: "priorities",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_profiles_companyid",
                table: "profiles",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_tools_companyid",
                table: "tools",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_typeofmaintenances_companyid",
                table: "typeofmaintenances",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_typesofproblems_companyid",
                table: "typesofproblems",
                column: "companyid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "functionallocations");

            migrationBuilder.DropTable(
                name: "methodtypes");

            migrationBuilder.DropTable(
                name: "positions");

            migrationBuilder.DropTable(
                name: "priorities");

            migrationBuilder.DropTable(
                name: "profiles");

            migrationBuilder.DropTable(
                name: "tools");

            migrationBuilder.DropTable(
                name: "typeofmaintenances");

            migrationBuilder.DropTable(
                name: "typesofproblems");

            migrationBuilder.DropTable(
                name: "floors");

            migrationBuilder.DropTable(
                name: "facilities");
        }
    }
}
