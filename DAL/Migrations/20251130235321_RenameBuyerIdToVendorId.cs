using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameBuyerIdToVendorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Vendors_BuyerId",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "Cars",
                newName: "VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_BuyerId",
                table: "Cars",
                newName: "IX_Cars_VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Vendors_VendorId",
                table: "Cars",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Vendors_VendorId",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "Cars",
                newName: "BuyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_VendorId",
                table: "Cars",
                newName: "IX_Cars_BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Vendors_BuyerId",
                table: "Cars",
                column: "BuyerId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
