using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionPlanning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "BomMasters");

            migrationBuilder.DropColumn(
                name: "MaterialCode",
                table: "BomDetails");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "BomMasters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "BomDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RawMaterialMaterialId",
                table: "BomDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BomMasters_ProductId",
                table: "BomMasters",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BomDetails_RawMaterialMaterialId",
                table: "BomDetails",
                column: "RawMaterialMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_BomDetails_RawMaterials_RawMaterialMaterialId",
                table: "BomDetails",
                column: "RawMaterialMaterialId",
                principalTable: "RawMaterials",
                principalColumn: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_BomMasters_Products_ProductId",
                table: "BomMasters",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BomDetails_RawMaterials_RawMaterialMaterialId",
                table: "BomDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BomMasters_Products_ProductId",
                table: "BomMasters");

            migrationBuilder.DropIndex(
                name: "IX_BomMasters_ProductId",
                table: "BomMasters");

            migrationBuilder.DropIndex(
                name: "IX_BomDetails_RawMaterialMaterialId",
                table: "BomDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "BomMasters");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "BomDetails");

            migrationBuilder.DropColumn(
                name: "RawMaterialMaterialId",
                table: "BomDetails");

            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "BomMasters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaterialCode",
                table: "BomDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
