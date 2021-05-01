using Microsoft.EntityFrameworkCore.Migrations;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Migrations
{
    public partial class CareTakerReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_AspNetUsers_UserId",
                table: "Pets");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Pets",
                newName: "RealCareTakerId");

            migrationBuilder.RenameIndex(
                name: "IX_Pets_UserId",
                table: "Pets",
                newName: "IX_Pets_RealCareTakerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_AspNetUsers_RealCareTakerId",
                table: "Pets",
                column: "RealCareTakerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_AspNetUsers_RealCareTakerId",
                table: "Pets");

            migrationBuilder.RenameColumn(
                name: "RealCareTakerId",
                table: "Pets",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Pets_RealCareTakerId",
                table: "Pets",
                newName: "IX_Pets_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_AspNetUsers_UserId",
                table: "Pets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
