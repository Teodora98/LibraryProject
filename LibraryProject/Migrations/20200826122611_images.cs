using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryProject.Migrations
{
    public partial class images : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LibrarianProfileImage",
                table: "Librarian",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerProfileImage",
                table: "Customer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LibrarianProfileImage",
                table: "Librarian");

            migrationBuilder.DropColumn(
                name: "CustomerProfileImage",
                table: "Customer");
        }
    }
}
