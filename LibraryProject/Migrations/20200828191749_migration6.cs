using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryProject.Migrations
{
    public partial class migration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckOut_Librarian_LibrarianId",
                table: "CheckOut");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOut_Librarian_LibrarianId",
                table: "CheckOut",
                column: "LibrarianId",
                principalTable: "Librarian",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckOut_Librarian_LibrarianId",
                table: "CheckOut");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOut_Librarian_LibrarianId",
                table: "CheckOut",
                column: "LibrarianId",
                principalTable: "Librarian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
