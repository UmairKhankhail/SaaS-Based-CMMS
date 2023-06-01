using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaintenanceWebApi.Migrations
{
    public partial class secondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_procedureMethods_procedures_pAutoId",
                table: "procedureMethods");

            migrationBuilder.DropColumn(
                name: "pAuotId",
                table: "procedureMethods");

            migrationBuilder.AlterColumn<int>(
                name: "pAutoId",
                table: "procedureMethods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_procedureMethods_procedures_pAutoId",
                table: "procedureMethods",
                column: "pAutoId",
                principalTable: "procedures",
                principalColumn: "pAutoId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_procedureMethods_procedures_pAutoId",
                table: "procedureMethods");

            migrationBuilder.AlterColumn<int>(
                name: "pAutoId",
                table: "procedureMethods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "pAuotId",
                table: "procedureMethods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_procedureMethods_procedures_pAutoId",
                table: "procedureMethods",
                column: "pAutoId",
                principalTable: "procedures",
                principalColumn: "pAutoId");
        }
    }
}
