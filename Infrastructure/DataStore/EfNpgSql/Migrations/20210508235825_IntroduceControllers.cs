using Microsoft.EntityFrameworkCore.Migrations;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Migrations
{
    public partial class IntroduceControllers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Controller",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Controller_UserId",
                table: "Controller",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Controller_AspNetUsers_UserId",
                table: "Controller",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Controller_AspNetUsers_UserId",
                table: "Controller");

            migrationBuilder.DropIndex(
                name: "IX_Controller_UserId",
                table: "Controller");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Controller");
        }
    }
}
