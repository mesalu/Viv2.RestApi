using Microsoft.EntityFrameworkCore.Migrations;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Migrations
{
    public partial class FixEnvToControllerRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Controllers_ControllerId",
                table: "Environments");

            migrationBuilder.RenameColumn(
                name: "ControllerId",
                table: "Environments",
                newName: "RealControllerId");

            migrationBuilder.RenameIndex(
                name: "IX_Environments_ControllerId",
                table: "Environments",
                newName: "IX_Environments_RealControllerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Controllers_RealControllerId",
                table: "Environments",
                column: "RealControllerId",
                principalTable: "Controllers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Controllers_RealControllerId",
                table: "Environments");

            migrationBuilder.RenameColumn(
                name: "RealControllerId",
                table: "Environments",
                newName: "ControllerId");

            migrationBuilder.RenameIndex(
                name: "IX_Environments_RealControllerId",
                table: "Environments",
                newName: "IX_Environments_ControllerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Controllers_ControllerId",
                table: "Environments",
                column: "ControllerId",
                principalTable: "Controllers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
