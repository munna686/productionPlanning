using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionPlanning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancelBy",
                table: "BomLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CaneledOn",
                table: "BomLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelBy",
                table: "BomLogs");

            migrationBuilder.DropColumn(
                name: "CaneledOn",
                table: "BomLogs");
        }
    }
}
