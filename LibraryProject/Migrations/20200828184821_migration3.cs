using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryProject.Migrations
{
    public partial class migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Librarian_LibrarianId",
                table: "Customer");

            migrationBuilder.AlterColumn<int>(
                name: "LibrarianId",
                table: "Customer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
                name: "FK_Customer_Librarian_LibrarianId",
                table: "Customer");

            migrationBuilder.AlterColumn<int>(
                name: "LibrarianId",
                table: "Customer",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Librarian_LibrarianId",
                table: "Customer",
                column: "LibrarianId",
                principalTable: "Librarian",
                principalColumn: "Id");
        }
    }
}
