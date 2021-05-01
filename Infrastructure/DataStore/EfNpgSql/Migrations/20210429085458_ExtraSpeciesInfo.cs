using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Migrations
{
    public partial class ExtraSpeciesInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnvDataSamples_Environments_RealEnvironmentId",
                table: "EnvDataSamples");

            migrationBuilder.AddColumn<double>(
                name: "DefaultLatitude",
                table: "Species",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DefaultLongitude",
                table: "Species",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ScientificName",
                table: "Species",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RealEnvironmentId",
                table: "EnvDataSamples",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Captured",
                table: "EnvDataSamples",
                type: "timestamp without time zone",
                nullable: true,
                defaultValueSql: "(NOW() AT TIME ZONE 'utc')",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "(NOW() AT TIME ZONE 'utc')");

            migrationBuilder.AddForeignKey(
                name: "FK_EnvDataSamples_Environments_RealEnvironmentId",
                table: "EnvDataSamples",
                column: "RealEnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnvDataSamples_Environments_RealEnvironmentId",
                table: "EnvDataSamples");

            migrationBuilder.DropColumn(
                name: "DefaultLatitude",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "DefaultLongitude",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "ScientificName",
                table: "Species");

            migrationBuilder.AlterColumn<Guid>(
                name: "RealEnvironmentId",
                table: "EnvDataSamples",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Captured",
                table: "EnvDataSamples",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "(NOW() AT TIME ZONE 'utc')",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true,
                oldDefaultValueSql: "(NOW() AT TIME ZONE 'utc')");

            migrationBuilder.AddForeignKey(
                name: "FK_EnvDataSamples_Environments_RealEnvironmentId",
                table: "EnvDataSamples",
                column: "RealEnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
