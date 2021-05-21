using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Migrations
{
    public partial class BlobRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_AspNetUsers_RealCareTakerId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Species_RealSpeciesId",
                table: "Pets");

            migrationBuilder.AlterColumn<int>(
                name: "RealSpeciesId",
                table: "Pets",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RealCareTakerId",
                table: "Pets",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Pets",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Morph",
                table: "Pets",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProfileRecordEntityId",
                table: "Pets",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlobRecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "text", nullable: true),
                    BlobName = table.Column<string>(type: "text", nullable: true),
                    MimeType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlobRecord", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pets_ProfileRecordEntityId",
                table: "Pets",
                column: "ProfileRecordEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_AspNetUsers_RealCareTakerId",
                table: "Pets",
                column: "RealCareTakerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_BlobRecord_ProfileRecordEntityId",
                table: "Pets",
                column: "ProfileRecordEntityId",
                principalTable: "BlobRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Species_RealSpeciesId",
                table: "Pets",
                column: "RealSpeciesId",
                principalTable: "Species",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_AspNetUsers_RealCareTakerId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_BlobRecord_ProfileRecordEntityId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Species_RealSpeciesId",
                table: "Pets");

            migrationBuilder.DropTable(
                name: "BlobRecord");

            migrationBuilder.DropIndex(
                name: "IX_Pets_ProfileRecordEntityId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "ProfileRecordEntityId",
                table: "Pets");

            migrationBuilder.AlterColumn<int>(
                name: "RealSpeciesId",
                table: "Pets",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "RealCareTakerId",
                table: "Pets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Pets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Morph",
                table: "Pets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_AspNetUsers_RealCareTakerId",
                table: "Pets",
                column: "RealCareTakerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Species_RealSpeciesId",
                table: "Pets",
                column: "RealSpeciesId",
                principalTable: "Species",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
