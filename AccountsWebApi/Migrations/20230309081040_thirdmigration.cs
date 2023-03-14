using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountsWebApi.Migrations
{
    public partial class thirdmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userandprofiles",
                columns: table => new
                {
                    userandprofileautoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    profileautoid = table.Column<int>(type: "int", nullable: false),
                    userautoid = table.Column<int>(type: "int", nullable: false),
                    companyid = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userandprofiles", x => x.userandprofileautoid);
                    table.ForeignKey(
                        name: "FK_userandprofiles_companies_companyid",
                        column: x => x.companyid,
                        principalTable: "companies",
                        principalColumn: "companyid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userandprofiles_profiles_profileautoid",
                        column: x => x.profileautoid,
                        principalTable: "profiles",
                        principalColumn: "profileautoid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userandprofiles_users_userautoid",
                        column: x => x.userautoid,
                        principalTable: "users",
                        principalColumn: "userautoid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_userandprofiles_companyid",
                table: "userandprofiles",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_userandprofiles_profileautoid",
                table: "userandprofiles",
                column: "profileautoid");

            migrationBuilder.CreateIndex(
                name: "IX_userandprofiles_userautoid",
                table: "userandprofiles",
                column: "userautoid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userandprofiles");
        }
    }
}
