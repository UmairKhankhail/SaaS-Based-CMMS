using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountsWebApi.Migrations
{
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    companyId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyPhone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userFirstName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userLastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.companyId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    permissionId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    permissionName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.permissionId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    deptAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    deptId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    deptName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.deptAutoId);
                    table.ForeignKey(
                        name: "FK_departments_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "facilities",
                columns: table => new
                {
                    facilityAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    facilityId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    facilityName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_facilities", x => x.facilityAutoId);
                    table.ForeignKey(
                        name: "FK_facilities_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "methodTypes",
                columns: table => new
                {
                    mtAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    mtId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mtName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_methodTypes", x => x.mtAutoId);
                    table.ForeignKey(
                        name: "FK_methodTypes_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "positions",
                columns: table => new
                {
                    positionAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    positionId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    positionName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_positions", x => x.positionAutoId);
                    table.ForeignKey(
                        name: "FK_positions_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "priorities",
                columns: table => new
                {
                    priorityAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    priorityId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    priorityName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_priorities", x => x.priorityAutoId);
                    table.ForeignKey(
                        name: "FK_priorities_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "profiles",
                columns: table => new
                {
                    profileAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    profileId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    profileName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profiles", x => x.profileAutoId);
                    table.ForeignKey(
                        name: "FK_profiles_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    roleAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    roleId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    roleName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.roleAutoId);
                    table.ForeignKey(
                        name: "FK_roles_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tools",
                columns: table => new
                {
                    toolAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    toolId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    toolName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tools", x => x.toolAutoId);
                    table.ForeignKey(
                        name: "FK_tools_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "typeOfMaintenances",
                columns: table => new
                {
                    tomAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tomId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tomName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typeOfMaintenances", x => x.tomAutoId);
                    table.ForeignKey(
                        name: "FK_typeOfMaintenances_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "typesOfProblems",
                columns: table => new
                {
                    topAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    topId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    topName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typesOfProblems", x => x.topAutoId);
                    table.ForeignKey(
                        name: "FK_typesOfProblems_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    employeeAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employeeId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    employeeName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    employeeFatherName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    employeeDesignation = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    employeeContactNo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    employeeeMail = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    deptAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.employeeAutoId);
                    table.ForeignKey(
                        name: "FK_employees_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employees_departments_deptAutoId",
                        column: x => x.deptAutoId,
                        principalTable: "departments",
                        principalColumn: "deptAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "subDepartments",
                columns: table => new
                {
                    subDeptAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    subDeptId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    subDeptName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    deptAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subDepartments", x => x.subDeptAutoId);
                    table.ForeignKey(
                        name: "FK_subDepartments_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_subDepartments_departments_deptAutoId",
                        column: x => x.deptAutoId,
                        principalTable: "departments",
                        principalColumn: "deptAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "floors",
                columns: table => new
                {
                    floorAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    floorId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    floorName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    facilityAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_floors", x => x.floorAutoId);
                    table.ForeignKey(
                        name: "FK_floors_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_floors_facilities_facilityAutoId",
                        column: x => x.facilityAutoId,
                        principalTable: "facilities",
                        principalColumn: "facilityAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roleAndDepartments",
                columns: table => new
                {
                    roleDeptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    roleAutoId = table.Column<int>(type: "int", nullable: false),
                    deptAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roleAndDepartments", x => x.roleDeptId);
                    table.ForeignKey(
                        name: "FK_roleAndDepartments_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roleAndDepartments_departments_deptAutoId",
                        column: x => x.deptAutoId,
                        principalTable: "departments",
                        principalColumn: "deptAutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roleAndDepartments_roles_roleAutoId",
                        column: x => x.roleAutoId,
                        principalTable: "roles",
                        principalColumn: "roleAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roleAndPermissions",
                columns: table => new
                {
                    rolePermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    roleAutoId = table.Column<int>(type: "int", nullable: false),
                    permissionId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roleAndPermissions", x => x.rolePermissionId);
                    table.ForeignKey(
                        name: "FK_roleAndPermissions_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roleAndPermissions_permissions_permissionId",
                        column: x => x.permissionId,
                        principalTable: "permissions",
                        principalColumn: "permissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roleAndPermissions_roles_roleAutoId",
                        column: x => x.roleAutoId,
                        principalTable: "roles",
                        principalColumn: "roleAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    userId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    role = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    employeeAutoId = table.Column<int>(type: "int", nullable: false),
                    deptAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userAutoId);
                    table.ForeignKey(
                        name: "FK_users_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_departments_deptAutoId",
                        column: x => x.deptAutoId,
                        principalTable: "departments",
                        principalColumn: "deptAutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_employees_employeeAutoId",
                        column: x => x.employeeAutoId,
                        principalTable: "employees",
                        principalColumn: "employeeAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "functionalLocations",
                columns: table => new
                {
                    flAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    flId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    flName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    facilityAutoId = table.Column<int>(type: "int", nullable: false),
                    floorAutoId = table.Column<int>(type: "int", nullable: false),
                    subDeptAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_functionalLocations", x => x.flAutoId);
                    table.ForeignKey(
                        name: "FK_functionalLocations_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_functionalLocations_facilities_facilityAutoId",
                        column: x => x.facilityAutoId,
                        principalTable: "facilities",
                        principalColumn: "facilityAutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_functionalLocations_floors_floorAutoId",
                        column: x => x.floorAutoId,
                        principalTable: "floors",
                        principalColumn: "floorAutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_functionalLocations_subDepartments_subDeptAutoId",
                        column: x => x.subDeptAutoId,
                        principalTable: "subDepartments",
                        principalColumn: "subDeptAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userAndProfiles",
                columns: table => new
                {
                    userAndProfileAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    profileAutoId = table.Column<int>(type: "int", nullable: false),
                    userAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userAndProfiles", x => x.userAndProfileAutoId);
                    table.ForeignKey(
                        name: "FK_userAndProfiles_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userAndProfiles_profiles_profileAutoId",
                        column: x => x.profileAutoId,
                        principalTable: "profiles",
                        principalColumn: "profileAutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userAndProfiles_users_userAutoId",
                        column: x => x.userAutoId,
                        principalTable: "users",
                        principalColumn: "userAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userAndRoles",
                columns: table => new
                {
                    roleUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    roleAutoId = table.Column<int>(type: "int", nullable: false),
                    userAutoId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userAndRoles", x => x.roleUserId);
                    table.ForeignKey(
                        name: "FK_userAndRoles_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "companyId", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userAndRoles_roles_roleAutoId",
                        column: x => x.roleAutoId,
                        principalTable: "roles",
                        principalColumn: "roleAutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userAndRoles_users_userAutoId",
                        column: x => x.userAutoId,
                        principalTable: "users",
                        principalColumn: "userAutoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_companies_companyEmail",
                table: "companies",
                column: "companyEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_companies_companyName",
                table: "companies",
                column: "companyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_departments_companyId",
                table: "departments",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_employees_companyId",
                table: "employees",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_employees_deptAutoId",
                table: "employees",
                column: "deptAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_facilities_companyId",
                table: "facilities",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_floors_companyId",
                table: "floors",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_floors_facilityAutoId",
                table: "floors",
                column: "facilityAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_functionalLocations_companyId",
                table: "functionalLocations",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_functionalLocations_facilityAutoId",
                table: "functionalLocations",
                column: "facilityAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_functionalLocations_floorAutoId",
                table: "functionalLocations",
                column: "floorAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_functionalLocations_subDeptAutoId",
                table: "functionalLocations",
                column: "subDeptAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_methodTypes_companyId",
                table: "methodTypes",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_positions_companyId",
                table: "positions",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_priorities_companyId",
                table: "priorities",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_profiles_companyId",
                table: "profiles",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_roleAndDepartments_companyId",
                table: "roleAndDepartments",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_roleAndDepartments_deptAutoId",
                table: "roleAndDepartments",
                column: "deptAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_roleAndDepartments_roleAutoId",
                table: "roleAndDepartments",
                column: "roleAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_roleAndPermissions_companyId",
                table: "roleAndPermissions",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_roleAndPermissions_permissionId",
                table: "roleAndPermissions",
                column: "permissionId");

            migrationBuilder.CreateIndex(
                name: "IX_roleAndPermissions_roleAutoId",
                table: "roleAndPermissions",
                column: "roleAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_roles_companyId",
                table: "roles",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_subDepartments_companyId",
                table: "subDepartments",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_subDepartments_deptAutoId",
                table: "subDepartments",
                column: "deptAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_tools_companyId",
                table: "tools",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_typeOfMaintenances_companyId",
                table: "typeOfMaintenances",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_typesOfProblems_companyId",
                table: "typesOfProblems",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_userAndProfiles_companyId",
                table: "userAndProfiles",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_userAndProfiles_profileAutoId",
                table: "userAndProfiles",
                column: "profileAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_userAndProfiles_userAutoId",
                table: "userAndProfiles",
                column: "userAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_userAndRoles_companyId",
                table: "userAndRoles",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_userAndRoles_roleAutoId",
                table: "userAndRoles",
                column: "roleAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_userAndRoles_userAutoId",
                table: "userAndRoles",
                column: "userAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_users_companyId",
                table: "users",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_users_deptAutoId",
                table: "users",
                column: "deptAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_users_employeeAutoId",
                table: "users",
                column: "employeeAutoId");

            migrationBuilder.CreateIndex(
                name: "IX_users_userName",
                table: "users",
                column: "userName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "functionalLocations");

            migrationBuilder.DropTable(
                name: "methodTypes");

            migrationBuilder.DropTable(
                name: "positions");

            migrationBuilder.DropTable(
                name: "priorities");

            migrationBuilder.DropTable(
                name: "roleAndDepartments");

            migrationBuilder.DropTable(
                name: "roleAndPermissions");

            migrationBuilder.DropTable(
                name: "tools");

            migrationBuilder.DropTable(
                name: "typeOfMaintenances");

            migrationBuilder.DropTable(
                name: "typesOfProblems");

            migrationBuilder.DropTable(
                name: "userAndProfiles");

            migrationBuilder.DropTable(
                name: "userAndRoles");

            migrationBuilder.DropTable(
                name: "floors");

            migrationBuilder.DropTable(
                name: "subDepartments");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "profiles");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "facilities");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "companies");
        }
    }
}
