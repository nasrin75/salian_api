using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace salian_api.Migrations
{
    /// <inheritdoc />
    public partial class DeleteEquipmentFeatureTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentFeatures");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentFeatures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentId = table.Column<long>(type: "bigint", nullable: false),
                    FeatureId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentFeatures", x => x.Id);
                });
        }
    }
}
