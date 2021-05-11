using Microsoft.EntityFrameworkCore.Migrations;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Migrations
{
    public partial class ControllerDirectAccess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Controller_AspNetUsers_UserId",
                table: "Controller");

            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Controller_ControllerId",
                table: "Environments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Controller",
                table: "Controller");

            migrationBuilder.RenameTable(
                name: "Controller",
                newName: "Controllers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Controllers",
                newName: "RealOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Controller_UserId",
                table: "Controllers",
                newName: "IX_Controllers_RealOwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Controllers",
                table: "Controllers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Controllers_AspNetUsers_RealOwnerId",
                table: "Controllers",
                column: "RealOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Controllers_ControllerId",
                table: "Environments",
                column: "ControllerId",
                principalTable: "Controllers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Controllers_AspNetUsers_RealOwnerId",
                table: "Controllers");

            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Controllers_ControllerId",
                table: "Environments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Controllers",
                table: "Controllers");

            migrationBuilder.RenameTable(
                name: "Controllers",
                newName: "Controller");

            migrationBuilder.RenameColumn(
                name: "RealOwnerId",
                table: "Controller",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Controllers_RealOwnerId",
                table: "Controller",
                newName: "IX_Controller_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Controller",
                table: "Controller",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Controller_AspNetUsers_UserId",
                table: "Controller",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Controller_ControllerId",
                table: "Environments",
                column: "ControllerId",
                principalTable: "Controller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
