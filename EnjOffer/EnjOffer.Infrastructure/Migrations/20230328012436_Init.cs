using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnjOffer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "advice",
                columns: table => new
                {
                    advice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    advice_number = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    advice_content = table.Column<string>(type: "text", nullable: false, defaultValue: "Advice isn't supplied")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_advice", x => x.advice_id);
                });

            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    book_title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, defaultValue: "Book title isn't supplied"),
                    book_description = table.Column<string>(type: "text", nullable: false, defaultValue: "Book description isn't supplied"),
                    book_author = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, defaultValue: "The author isn't supplied"),
                    book_content = table.Column<string>(type: "text", nullable: false, defaultValue: "Book content isn't supplied"),
                    book_image_src = table.Column<string>(type: "text", nullable: true, defaultValue: "imgNotFound.png")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.book_id);
                });

            migrationBuilder.CreateTable(
                name: "default_words",
                columns: table => new
                {
                    default_word_id = table.Column<Guid>(type: "uuid", nullable: false),
                    default_word_word = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: false, defaultValue: ""),
                    default_word_word_translation = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: false, defaultValue: ""),
                    default_word_image_src = table.Column<string>(type: "text", nullable: true, defaultValue: "imgNotFound.png")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_default_words", x => x.default_word_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_email = table.Column<string>(type: "varchar(320)", maxLength: 320, nullable: false),
                    user_password = table.Column<byte[]>(type: "bytea", nullable: false),
                    user_role = table.Column<string>(type: "text", nullable: false, defaultValue: "SuperAdmin")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "user_statistics",
                columns: table => new
                {
                    user_statistic_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_statistic_answer_date = table.Column<DateTime>(type: "date", nullable: false, defaultValue: new DateTime(2023, 3, 28, 4, 24, 35, 851, DateTimeKind.Local).AddTicks(1508)),
                    user_statistic_correct_answer_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    user_statistic_incorrect_answer_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_statistics", x => x.user_statistic_id);
                    table.ForeignKey(
                        name: "FK_user_statistics_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_words",
                columns: table => new
                {
                    user_word_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_word_word = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: false, defaultValue: ""),
                    user_word_word_translation = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: false, defaultValue: ""),
                    user_word_priority = table.Column<int>(type: "integer", nullable: false, defaultValue: 5),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_words", x => x.user_word_id);
                    table.ForeignKey(
                        name: "FK_user_words_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "users_default_words",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DefaultWordId = table.Column<Guid>(type: "uuid", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false, defaultValue: 5)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_default_words", x => new { x.UserId, x.DefaultWordId });
                    table.ForeignKey(
                        name: "FK_users_default_words_default_words_DefaultWordId",
                        column: x => x.DefaultWordId,
                        principalTable: "default_words",
                        principalColumn: "default_word_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_default_words_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_advice_advice_number",
                table: "advice",
                column: "advice_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_default_words_default_word_word",
                table: "default_words",
                column: "default_word_word",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_statistics_UserId",
                table: "user_statistics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_words_user_word_word",
                table: "user_words",
                column: "user_word_word",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_words_UserId",
                table: "user_words",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_user_email",
                table: "users",
                column: "user_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_books_BookId",
                table: "users_books",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_users_default_words_DefaultWordId",
                table: "users_default_words",
                column: "DefaultWordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "advice");

            migrationBuilder.DropTable(
                name: "user_statistics");

            migrationBuilder.DropTable(
                name: "user_words");

            migrationBuilder.DropTable(
                name: "users_books");

            migrationBuilder.DropTable(
                name: "users_default_words");

            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "default_words");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
