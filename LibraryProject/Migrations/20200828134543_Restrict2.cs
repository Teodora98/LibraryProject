using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryProject.Migrations
{
    public partial class Restrict2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckOut_Book_BookId",
                table: "CheckOut");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckOut_Customer_CustomerId",
                table: "CheckOut");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckOut_Librarian_LibrarianId",
                table: "CheckOut");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Librarian_LibrarianId",
                table: "Customer");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOut_Book_BookId",
                table: "CheckOut",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOut_Customer_CustomerId",
                table: "CheckOut",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOut_Librarian_LibrarianId",
                table: "CheckOut",
                column: "LibrarianId",
                principalTable: "Librarian",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Librarian_LibrarianId",
                table: "Customer",
                column: "LibrarianId",
                principalTable: "Librarian",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckOut_Book_BookId",
                table: "CheckOut");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckOut_Customer_CustomerId",
                table: "CheckOut");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckOut_Librarian_LibrarianId",
                table: "CheckOut");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Librarian_LibrarianId",
                table: "Customer");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOut_Librarian_LibrarianId",
                table: "CheckOut",
                column: "LibrarianId",
                principalTable: "Librarian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Librarian_LibrarianId",
                table: "Customer",
                column: "LibrarianId",
                principalTable: "Librarian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
