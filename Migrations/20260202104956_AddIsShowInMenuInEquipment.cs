using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace salian_api.Migrations
{
    /// <inheritdoc />
    public partial class AddIsShowInMenuInEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShowInMenu",
                table: "Equipments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_LocationId",
                table: "Inventories",
                column: "LocationId");

           /* migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Locations_LocationId",
                table: "Inventories",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Locations_LocationId",
                table: "Inventories");

           /* migrationBuilder.DropIndex(
                name: "IX_Inventories_LocationId",
                table: "Inventories");*/

            migrationBuilder.DropColumn(
                name: "IsShowInMenu",
                table: "Equipments");
        }
    }
}
