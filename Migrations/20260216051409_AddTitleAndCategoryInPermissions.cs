using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace salian_api.Migrations
{
    /// <inheritdoc />
    public partial class AddTitleAndCategoryInPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Permissions",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Permissions",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Permissions");
        }
    }
}
