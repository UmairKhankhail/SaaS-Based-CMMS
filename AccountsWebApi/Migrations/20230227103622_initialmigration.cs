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
                    companyid = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyname = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyemail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyphone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userfirstname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    userlastname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.companyid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    permissionid = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    permissionname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.permissionid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    deptautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    deptid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    deptname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.deptautoid);
                    table.ForeignKey(
                        name: "FK_departments_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    roleautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    roleid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    rolename = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.roleautoid);
                    table.ForeignKey(
                        name: "FK_roles_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    employeeautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employeeid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    employeename = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    employeefathername = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    employeedesignation = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    employeecontactno = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    employeeemail = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    deptautoid = table.Column<int>(type: "int", nullable: false),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.employeeautoid);
                    table.ForeignKey(
                        name: "FK_employees_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employees_departments_deptautoid",
                        column: x => x.deptautoid,
                        principalTable: "departments",
                        principalColumn: "deptautoid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sub_departments",
                columns: table => new
                {
                    subdeptautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    subdeptid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    subdeptname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    deptautoid = table.Column<int>(type: "int", nullable: false),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sub_departments", x => x.subdeptautoid);
                    table.ForeignKey(
                        name: "FK_sub_departments_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sub_departments_departments_deptautoid",
                        column: x => x.deptautoid,
                        principalTable: "departments",
                        principalColumn: "deptautoid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roleanddepartments",
                columns: table => new
                {
                    roledeptid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    roleautoid = table.Column<int>(type: "int", nullable: false),
                    deptautoid = table.Column<int>(type: "int", nullable: false),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roleanddepartments", x => x.roledeptid);
                    table.ForeignKey(
                        name: "FK_roleanddepartments_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roleanddepartments_departments_deptautoid",
                        column: x => x.deptautoid,
                        principalTable: "departments",
                        principalColumn: "deptautoid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roleanddepartments_roles_roleautoid",
                        column: x => x.roleautoid,
                        principalTable: "roles",
                        principalColumn: "roleautoid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roleandpermissions",
                columns: table => new
                {
                    rolepermissionid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    roleautoid = table.Column<int>(type: "int", nullable: false),
                    permissionid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roleandpermissions", x => x.rolepermissionid);
                    table.ForeignKey(
                        name: "FK_roleandpermissions_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roleandpermissions_permissions_permissionid",
                        column: x => x.permissionid,
                        principalTable: "permissions",
                        principalColumn: "permissionid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roleandpermissions_roles_roleautoid",
                        column: x => x.roleautoid,
                        principalTable: "roles",
                        principalColumn: "roleautoid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    userid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    username = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    role = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    employeeautoid = table.Column<int>(type: "int", nullable: false),
                    deptautoid = table.Column<int>(type: "int", nullable: false),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userautoid);
                    table.ForeignKey(
                        name: "FK_users_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_departments_deptautoid",
                        column: x => x.deptautoid,
                        principalTable: "departments",
                        principalColumn: "deptautoid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_employees_employeeautoid",
                        column: x => x.employeeautoid,
                        principalTable: "employees",
                        principalColumn: "employeeautoid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userandroles",
                columns: table => new
                {
                    roleuserid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    roleautoid = table.Column<int>(type: "int", nullable: false),
                    userautoid = table.Column<int>(type: "int", nullable: false),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userandroles", x => x.roleuserid);
                    table.ForeignKey(
                        name: "FK_userandroles_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userandroles_roles_roleautoid",
                        column: x => x.roleautoid,
                        principalTable: "roles",
                        principalColumn: "roleautoid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userandroles_users_userautoid",
                        column: x => x.userautoid,
                        principalTable: "users",
                        principalColumn: "userautoid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_companies_companyemail",
                table: "companies",
                column: "companyemail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_companies_companyname",
                table: "companies",
                column: "companyname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_departments_companyid",
                table: "departments",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_employees_companyid",
                table: "employees",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_employees_deptautoid",
                table: "employees",
                column: "deptautoid");

            migrationBuilder.CreateIndex(
                name: "IX_roleanddepartments_companyid",
                table: "roleanddepartments",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_roleanddepartments_deptautoid",
                table: "roleanddepartments",
                column: "deptautoid");

            migrationBuilder.CreateIndex(
                name: "IX_roleanddepartments_roleautoid",
                table: "roleanddepartments",
                column: "roleautoid");

            migrationBuilder.CreateIndex(
                name: "IX_roleandpermissions_companyid",
                table: "roleandpermissions",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_roleandpermissions_permissionid",
                table: "roleandpermissions",
                column: "permissionid");

            migrationBuilder.CreateIndex(
                name: "IX_roleandpermissions_roleautoid",
                table: "roleandpermissions",
                column: "roleautoid");

            migrationBuilder.CreateIndex(
                name: "IX_roles_companyid",
                table: "roles",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_sub_departments_companyid",
                table: "sub_departments",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_sub_departments_deptautoid",
                table: "sub_departments",
                column: "deptautoid");

            migrationBuilder.CreateIndex(
                name: "IX_userandroles_companyid",
                table: "userandroles",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_userandroles_roleautoid",
                table: "userandroles",
                column: "roleautoid");

            migrationBuilder.CreateIndex(
                name: "IX_userandroles_userautoid",
                table: "userandroles",
                column: "userautoid");

            migrationBuilder.CreateIndex(
                name: "IX_users_companyid",
                table: "users",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_users_deptautoid",
                table: "users",
                column: "deptautoid");

            migrationBuilder.CreateIndex(
                name: "IX_users_employeeautoid",
                table: "users",
                column: "employeeautoid");

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "roleanddepartments");

            migrationBuilder.DropTable(
                name: "roleandpermissions");

            migrationBuilder.DropTable(
                name: "sub_departments");

            migrationBuilder.DropTable(
                name: "userandroles");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "companies");
        }
    }
}
