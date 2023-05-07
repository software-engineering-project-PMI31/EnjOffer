using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnjOffer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedUniqueConstraintInDefaultWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_default_words_default_word_word",
                table: "default_words");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 1, 22, 28, 52, 9, DateTimeKind.Local).AddTicks(4409),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 4, 2, 20, 11, 836, DateTimeKind.Local).AddTicks(6127));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_last_time_entered",
                table: "user_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 1, 22, 28, 52, 4, DateTimeKind.Local).AddTicks(6536),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 4, 2, 20, 11, 834, DateTimeKind.Local).AddTicks(7778));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "user_words",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 1, 22, 28, 52, 5, DateTimeKind.Local).AddTicks(429),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 4, 2, 20, 11, 834, DateTimeKind.Local).AddTicks(9045));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "user_statistics",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_default_words_default_word_word_default_word_word_translati~",
                table: "default_words",
                columns: new[] { "default_word_word", "default_word_word_translation" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_default_words_default_word_word_default_word_word_translati~",
                table: "default_words");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 4, 2, 20, 11, 836, DateTimeKind.Local).AddTicks(6127),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 1, 22, 28, 52, 9, DateTimeKind.Local).AddTicks(4409));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_last_time_entered",
                table: "user_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 4, 2, 20, 11, 834, DateTimeKind.Local).AddTicks(7778),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 1, 22, 28, 52, 4, DateTimeKind.Local).AddTicks(6536));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "user_words",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 4, 2, 20, 11, 834, DateTimeKind.Local).AddTicks(9045),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 1, 22, 28, 52, 5, DateTimeKind.Local).AddTicks(429));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "user_statistics",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_default_words_default_word_word",
                table: "default_words",
                column: "default_word_word",
                unique: true);
        }
    }
}
