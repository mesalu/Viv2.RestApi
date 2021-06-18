using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Migrations
{
    public partial class ReorgAndTrackLatestSamplePerPet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<long>(
                name: "LatestSampleId",
                table: "Pets",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "EnvDataSamples",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_LatestSampleId",
                table: "Pets",
                column: "LatestSampleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_EnvDataSamples_LatestSampleId",
                table: "Pets",
                column: "LatestSampleId",
                principalTable: "EnvDataSamples",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_EnvDataSamples_LatestSampleId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_LatestSampleId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "LatestSampleId",
                table: "Pets");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "EnvDataSamples",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
