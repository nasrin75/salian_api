using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace salian_api.Migrations
{
    /// <inheritdoc />
    public partial class ConvertLoginTypeToJsonInUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginType",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "LoginTypes",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginTypes",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "LoginType",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }
    }
}
