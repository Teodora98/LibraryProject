using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryProject.Migrations
{
    public partial class Restrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckOut_Librarian_LibrarianId",
                table: "CheckOut");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Librarian_LibrarianId",
                table: "Customer");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckOut_Librarian_LibrarianId",
                table: "CheckOut");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Librarian_LibrarianId",
                table: "Customer");

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
    }
}
