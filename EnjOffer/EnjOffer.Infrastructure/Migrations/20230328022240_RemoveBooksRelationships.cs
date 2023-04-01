using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnjOffer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBooksRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users_books");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 28, 5, 22, 40, 129, DateTimeKind.Local).AddTicks(9219),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 3, 28, 4, 24, 35, 851, DateTimeKind.Local).AddTicks(1508));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 28, 4, 24, 35, 851, DateTimeKind.Local).AddTicks(1508),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 3, 28, 5, 22, 40, 129, DateTimeKind.Local).AddTicks(9219));

            migrationBuilder.CreateTable(
                name: "users_books",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BookId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_books", x => new { x.UserId, x.BookId });
                    table.ForeignKey(
                        name: "FK_users_books_books_BookId",
                        column: x => x.BookId,
                        principalTable: "books",
                        principalColumn: "book_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_books_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_books_BookId",
                table: "users_books",
                column: "BookId");
        }
    }
}
