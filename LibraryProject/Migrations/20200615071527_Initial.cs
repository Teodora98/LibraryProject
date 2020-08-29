using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryProject.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 30, nullable: false),
                    Author = table.Column<string>(maxLength: 15, nullable: false),
                    ISBN = table.Column<int>(nullable: false),
                    PageNumber = table.Column<int>(nullable: true),
                    ShortContent = table.Column<string>(maxLength: 220, nullable: false),
                    PublicationYear = table.Column<int>(nullable: true),
                    PublishingHouse = table.Column<string>(nullable: true),
                    Genre = table.Column<string>(maxLength: 30, nullable: false),
                    BookImage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Librarian",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    SSN = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Librarian", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    SSN = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Occupation = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    MembershipDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    LibrarianId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Librarian_LibrarianId",
                        column: x => x.LibrarianId,
                        principalTable: "Librarian",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CheckOut",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecksOut = table.Column<DateTime>(nullable: false),
                    ChecksIn = table.Column<DateTime>(nullable: true),
                    BookId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    LibrarianId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckOut", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckOut_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckOut_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckOut_Librarian_LibrarianId",
                        column: x => x.LibrarianId,
                        principalTable: "Librarian",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckOut_BookId",
                table: "CheckOut",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckOut_CustomerId",
                table: "CheckOut",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckOut_LibrarianId",
                table: "CheckOut",
                column: "LibrarianId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_LibrarianId",
                table: "Customer",
                column: "LibrarianId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckOut");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Librarian");
        }
    }
}
