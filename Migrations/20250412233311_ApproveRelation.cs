using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class ApproveRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminIdApproved",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminIdApproved",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_AdminIdApproved",
                table: "Products",
                column: "AdminIdApproved");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AdminIdApproved",
                table: "AspNetUsers",
                column: "AdminIdApproved");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_AdminIdApproved",
                table: "AspNetUsers",
                column: "AdminIdApproved",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_AdminIdApproved",
                table: "Products",
                column: "AdminIdApproved",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_AdminIdApproved",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_AdminIdApproved",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_AdminIdApproved",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AdminIdApproved",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdminIdApproved",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AdminIdApproved",
                table: "AspNetUsers");
        }
    }
}
