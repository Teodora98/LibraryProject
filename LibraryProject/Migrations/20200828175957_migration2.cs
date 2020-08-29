using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryProject.Migrations
{
    public partial class migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckOut_Book_BookId",
                table: "CheckOut");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckOut_Customer_CustomerId",
                table: "CheckOut");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOut_Book_BookId",
                table: "CheckOut",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOut_Customer_CustomerId",
                table: "CheckOut",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckOut_Book_BookId",
                table: "CheckOut");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckOut_Customer_CustomerId",
                table: "CheckOut");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOut_Book_BookId",
                table: "CheckOut",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOut_Customer_CustomerId",
                table: "CheckOut",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
