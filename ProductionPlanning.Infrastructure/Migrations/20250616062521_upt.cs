using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionPlanning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "BomDetails");

            migrationBuilder.AddColumn<int>(
                name: "OutputQuantiy",
                table: "BomMasters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "BomLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "BomDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutputQuantiy",
                table: "BomMasters");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "BomLogs");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "BomDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "BomDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
