using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Migrations
{
    /// <inheritdoc />
    public partial class product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_AdminIdApproved",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ViewersNumber",
                table: "Products",
                newName: "ViewsNumber");

            migrationBuilder.RenameColumn(
                name: "AdminIdApproved",
                table: "Products",
                newName: "AdminCheckedId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_AdminIdApproved",
                table: "Products",
                newName: "IX_Products_AdminCheckedId");

            migrationBuilder.AddColumn<string>(
                name: "AdminId1",
                table: "VendorPermissions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorId1",
                table: "VendorPermissions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId1",
                table: "SavedProducts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId1",
                table: "ProductReviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId1",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorId1",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorPermissions_AdminId1",
                table: "VendorPermissions",
                column: "AdminId1");

            migrationBuilder.CreateIndex(
                name: "IX_VendorPermissions_VendorId1",
                table: "VendorPermissions",
                column: "VendorId1");

            migrationBuilder.CreateIndex(
                name: "IX_SavedProducts_CustomerId1",
                table: "SavedProducts",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_CustomerId1",
                table: "ProductReviews",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId1",
                table: "Orders",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VendorId1",
                table: "Orders",
                column: "VendorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerId1",
                table: "Orders",
                column: "CustomerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_VendorId1",
                table: "Orders",
                column: "VendorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_AspNetUsers_CustomerId1",
                table: "ProductReviews",
                column: "CustomerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_AdminCheckedId",
                table: "Products",
                column: "AdminCheckedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedProducts_AspNetUsers_CustomerId1",
                table: "SavedProducts",
                column: "CustomerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorPermissions_AspNetUsers_AdminId1",
                table: "VendorPermissions",
                column: "AdminId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorPermissions_AspNetUsers_VendorId1",
                table: "VendorPermissions",
                column: "VendorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerId1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_VendorId1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_AspNetUsers_CustomerId1",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_AdminCheckedId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedProducts_AspNetUsers_CustomerId1",
                table: "SavedProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorPermissions_AspNetUsers_AdminId1",
                table: "VendorPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorPermissions_AspNetUsers_VendorId1",
                table: "VendorPermissions");

            migrationBuilder.DropIndex(
                name: "IX_VendorPermissions_AdminId1",
                table: "VendorPermissions");

            migrationBuilder.DropIndex(
                name: "IX_VendorPermissions_VendorId1",
                table: "VendorPermissions");

            migrationBuilder.DropIndex(
                name: "IX_SavedProducts_CustomerId1",
                table: "SavedProducts");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_CustomerId1",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_VendorId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AdminId1",
                table: "VendorPermissions");

            migrationBuilder.DropColumn(
                name: "VendorId1",
                table: "VendorPermissions");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "SavedProducts");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VendorId1",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ViewsNumber",
                table: "Products",
                newName: "ViewersNumber");

            migrationBuilder.RenameColumn(
                name: "AdminCheckedId",
                table: "Products",
                newName: "AdminIdApproved");

            migrationBuilder.RenameIndex(
                name: "IX_Products_AdminCheckedId",
                table: "Products",
                newName: "IX_Products_AdminIdApproved");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_AdminIdApproved",
                table: "Products",
                column: "AdminIdApproved",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
