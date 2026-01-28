using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace salian_api.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipmentFeatureList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentEntityFeatureEntity",
                columns: table => new
                {
                    EquipmentsId = table.Column<long>(type: "bigint", nullable: false),
                    FeaturesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentEntityFeatureEntity", x => new { x.EquipmentsId, x.FeaturesId });
                    table.ForeignKey(
                        name: "FK_EquipmentEntityFeatureEntity_Equipments_EquipmentsId",
                        column: x => x.EquipmentsId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentEntityFeatureEntity_Features_FeaturesId",
                        column: x => x.FeaturesId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentEntityFeatureEntity_FeaturesId",
                table: "EquipmentEntityFeatureEntity",
                column: "FeaturesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentEntityFeatureEntity");
        }
    }
}
