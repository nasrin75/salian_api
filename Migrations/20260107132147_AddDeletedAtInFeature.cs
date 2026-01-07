using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace salian_api.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedAtInFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShow",
                table: "EquipmentFeatures");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Features",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Features");

            migrationBuilder.AddColumn<bool>(
                name: "IsShow",
                table: "EquipmentFeatures",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }
    }
}
