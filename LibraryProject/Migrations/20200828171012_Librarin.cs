using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryProject.Migrations
{
    public partial class Librarin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Librarian_LibrarianId",
                table: "Customer");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Librarian_LibrarianId",
                table: "Customer",
                column: "LibrarianId",
                principalTable: "Librarian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Librarian_LibrarianId",
                table: "Customer");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Librarian_LibrarianId",
                table: "Customer",
                column: "LibrarianId",
                principalTable: "Librarian",
                principalColumn: "Id");
        }
    }
}
