using Microsoft.EntityFrameworkCore.Migrations;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Migrations
{
    public partial class UserEmailField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnvDataSamples_Environment_EnvironmentId",
                table: "EnvDataSamples");

            migrationBuilder.DropForeignKey(
                name: "FK_Environment_AspNetUsers_BackedUserId",
                table: "Environment");

            migrationBuilder.DropForeignKey(
                name: "FK_Environment_Controllers_ControllerId",
                table: "Environment");

            migrationBuilder.DropForeignKey(
                name: "FK_Environment_Pets_InhabitantId",
                table: "Environment");

            migrationBuilder.DropForeignKey(
                name: "FK_EnvironmentUser_Environment_EnvironmentsId",
                table: "EnvironmentUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Environment",
                table: "Environment");

            migrationBuilder.RenameTable(
                name: "Environment",
                newName: "Environments");

            migrationBuilder.RenameIndex(
                name: "IX_Environment_InhabitantId",
                table: "Environments",
                newName: "IX_Environments_InhabitantId");

            migrationBuilder.RenameIndex(
                name: "IX_Environment_ControllerId",
                table: "Environments",
                newName: "IX_Environments_ControllerId");

            migrationBuilder.RenameIndex(
                name: "IX_Environment_BackedUserId",
                table: "Environments",
                newName: "IX_Environments_BackedUserId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Environments",
                table: "Environments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EnvDataSamples_Environments_EnvironmentId",
                table: "EnvDataSamples",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_AspNetUsers_BackedUserId",
                table: "Environments",
                column: "BackedUserId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Pets_InhabitantId",
                table: "Environments",
                column: "InhabitantId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EnvironmentUser_Environments_EnvironmentsId",
                table: "EnvironmentUser",
                column: "EnvironmentsId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnvDataSamples_Environments_EnvironmentId",
                table: "EnvDataSamples");

            migrationBuilder.DropForeignKey(
                name: "FK_Environments_AspNetUsers_BackedUserId",
                table: "Environments");

            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Controllers_ControllerId",
                table: "Environments");

            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Pets_InhabitantId",
                table: "Environments");

            migrationBuilder.DropForeignKey(
                name: "FK_EnvironmentUser_Environments_EnvironmentsId",
                table: "EnvironmentUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Environments",
                table: "Environments");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");

            migrationBuilder.RenameTable(
                name: "Environments",
                newName: "Environment");

            migrationBuilder.RenameIndex(
                name: "IX_Environments_InhabitantId",
                table: "Environment",
                newName: "IX_Environment_InhabitantId");

            migrationBuilder.RenameIndex(
                name: "IX_Environments_ControllerId",
                table: "Environment",
                newName: "IX_Environment_ControllerId");

            migrationBuilder.RenameIndex(
                name: "IX_Environments_BackedUserId",
                table: "Environment",
                newName: "IX_Environment_BackedUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Environment",
                table: "Environment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EnvDataSamples_Environment_EnvironmentId",
                table: "EnvDataSamples",
                column: "EnvironmentId",
                principalTable: "Environment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Environment_AspNetUsers_BackedUserId",
                table: "Environment",
                column: "BackedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Environment_Controllers_ControllerId",
                table: "Environment",
                column: "ControllerId",
                principalTable: "Controllers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Environment_Pets_InhabitantId",
                table: "Environment",
                column: "InhabitantId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EnvironmentUser_Environment_EnvironmentsId",
                table: "EnvironmentUser",
                column: "EnvironmentsId",
                principalTable: "Environment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
