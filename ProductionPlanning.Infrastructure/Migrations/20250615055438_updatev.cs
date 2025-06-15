using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionPlanning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatev : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BomDetails_BomMasters_BomMasterBomId",
                table: "BomDetails");

            migrationBuilder.DropIndex(
                name: "IX_BomDetails_BomMasterBomId",
                table: "BomDetails");

            migrationBuilder.DropColumn(
                name: "BomMasterBomId",
                table: "BomDetails");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "BomMasters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "BomMasters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "BomId",
                table: "BomLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BomMasterBomId",
                table: "BomLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BomLogs_BomMasterBomId",
                table: "BomLogs",
                column: "BomMasterBomId");

            migrationBuilder.CreateIndex(
                name: "IX_BomDetails_BomId",
                table: "BomDetails",
                column: "BomId");

            migrationBuilder.AddForeignKey(
                name: "FK_BomDetails_BomMasters_BomId",
                table: "BomDetails",
                column: "BomId",
                principalTable: "BomMasters",
                principalColumn: "BomId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BomLogs_BomMasters_BomMasterBomId",
                table: "BomLogs",
                column: "BomMasterBomId",
                principalTable: "BomMasters",
                principalColumn: "BomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BomDetails_BomMasters_BomId",
                table: "BomDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BomLogs_BomMasters_BomMasterBomId",
                table: "BomLogs");

            migrationBuilder.DropIndex(
                name: "IX_BomLogs_BomMasterBomId",
                table: "BomLogs");

            migrationBuilder.DropIndex(
                name: "IX_BomDetails_BomId",
                table: "BomDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "BomMasters");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "BomMasters");

            migrationBuilder.DropColumn(
                name: "BomId",
                table: "BomLogs");

            migrationBuilder.DropColumn(
                name: "BomMasterBomId",
                table: "BomLogs");

            migrationBuilder.AddColumn<int>(
                name: "BomMasterBomId",
                table: "BomDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BomDetails_BomMasterBomId",
                table: "BomDetails",
                column: "BomMasterBomId");

            migrationBuilder.AddForeignKey(
                name: "FK_BomDetails_BomMasters_BomMasterBomId",
                table: "BomDetails",
                column: "BomMasterBomId",
                principalTable: "BomMasters",
                principalColumn: "BomId");
        }
    }
}
