using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace salian_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatEquipmentMappingRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EquipmentEntityId",
                table: "Inventories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_EquipmentEntityId",
                table: "Inventories",
                column: "EquipmentEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Equipments_EquipmentEntityId",
                table: "Inventories",
                column: "EquipmentEntityId",
                principalTable: "Equipments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Equipments_EquipmentEntityId",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_EquipmentEntityId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "EquipmentEntityId",
                table: "Inventories");
        }
    }
}
