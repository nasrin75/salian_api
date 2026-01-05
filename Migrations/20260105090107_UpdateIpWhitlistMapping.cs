using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace salian_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIpWhitlistMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IpWhiteLists_Users_UserEntityId",
                table: "IpWhiteLists");

            migrationBuilder.DropIndex(
                name: "IX_IpWhiteLists_UserEntityId",
                table: "IpWhiteLists");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "IpWhiteLists");

            migrationBuilder.CreateIndex(
                name: "IX_IpWhiteLists_UserId",
                table: "IpWhiteLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_IpWhiteLists_Users_UserId",
                table: "IpWhiteLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IpWhiteLists_Users_UserId",
                table: "IpWhiteLists");

            migrationBuilder.DropIndex(
                name: "IX_IpWhiteLists_UserId",
                table: "IpWhiteLists");

            migrationBuilder.AddColumn<long>(
                name: "UserEntityId",
                table: "IpWhiteLists",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IpWhiteLists_UserEntityId",
                table: "IpWhiteLists",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_IpWhiteLists_Users_UserEntityId",
                table: "IpWhiteLists",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
