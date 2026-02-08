using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace salian_api.Migrations
{
    /// <inheritdoc />
    public partial class NullableItParentNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ItParentNumber",
                table: "Inventories",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryFeatures_FeatureId",
                table: "InventoryFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryFeatures_InventoryId",
                table: "InventoryFeatures",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryFeatures_Features_FeatureId",
                table: "InventoryFeatures",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryFeatures_Inventories_InventoryId",
                table: "InventoryFeatures",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryFeatures_Features_FeatureId",
                table: "InventoryFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryFeatures_Inventories_InventoryId",
                table: "InventoryFeatures");

            migrationBuilder.DropIndex(
                name: "IX_InventoryFeatures_FeatureId",
                table: "InventoryFeatures");

            migrationBuilder.DropIndex(
                name: "IX_InventoryFeatures_InventoryId",
                table: "InventoryFeatures");

            migrationBuilder.AlterColumn<long>(
                name: "ItParentNumber",
                table: "Inventories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
