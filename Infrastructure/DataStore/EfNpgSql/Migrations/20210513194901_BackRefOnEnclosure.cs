using Microsoft.EntityFrameworkCore.Migrations;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Migrations
{
    public partial class BackRefOnEnclosure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Pets_RealInhabitantId",
                table: "Environments");

            migrationBuilder.DropIndex(
                name: "IX_Environments_RealInhabitantId",
                table: "Environments");

            migrationBuilder.RenameColumn(
                name: "RealInhabitantId",
                table: "Environments",
                newName: "InhabitantId");

            migrationBuilder.CreateIndex(
                name: "IX_Environments_InhabitantId",
                table: "Environments",
                column: "InhabitantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Pets_InhabitantId",
                table: "Environments",
                column: "InhabitantId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Pets_InhabitantId",
                table: "Environments");

            migrationBuilder.DropIndex(
                name: "IX_Environments_InhabitantId",
                table: "Environments");

            migrationBuilder.RenameColumn(
                name: "InhabitantId",
                table: "Environments",
                newName: "RealInhabitantId");

            migrationBuilder.CreateIndex(
                name: "IX_Environments_RealInhabitantId",
                table: "Environments",
                column: "RealInhabitantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Pets_RealInhabitantId",
                table: "Environments",
                column: "RealInhabitantId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
