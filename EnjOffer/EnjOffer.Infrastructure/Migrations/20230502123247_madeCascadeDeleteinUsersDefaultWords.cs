using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnjOffer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class madeCascadeDeleteinUsersDefaultWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_default_words_default_words_DefaultWordId",
                table: "users_default_words");

            migrationBuilder.DropForeignKey(
                name: "FK_users_default_words_users_UserId",
                table: "users_default_words");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 32, 47, 51, DateTimeKind.Local).AddTicks(5028),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 20, 28, 363, DateTimeKind.Local).AddTicks(3564));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_last_time_entered",
                table: "user_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 32, 47, 49, DateTimeKind.Local).AddTicks(4792),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 20, 28, 361, DateTimeKind.Local).AddTicks(4506));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 32, 47, 49, DateTimeKind.Local).AddTicks(6378),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 20, 28, 361, DateTimeKind.Local).AddTicks(6021));

            migrationBuilder.AddForeignKey(
                name: "FK_users_default_words_default_words_DefaultWordId",
                table: "users_default_words",
                column: "DefaultWordId",
                principalTable: "default_words",
                principalColumn: "default_word_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_default_words_users_UserId",
                table: "users_default_words",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_default_words_default_words_DefaultWordId",
                table: "users_default_words");

            migrationBuilder.DropForeignKey(
                name: "FK_users_default_words_users_UserId",
                table: "users_default_words");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 20, 28, 363, DateTimeKind.Local).AddTicks(3564),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 32, 47, 51, DateTimeKind.Local).AddTicks(5028));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_last_time_entered",
                table: "user_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 20, 28, 361, DateTimeKind.Local).AddTicks(4506),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 32, 47, 49, DateTimeKind.Local).AddTicks(4792));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 20, 28, 361, DateTimeKind.Local).AddTicks(6021),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 32, 47, 49, DateTimeKind.Local).AddTicks(6378));

            migrationBuilder.AddForeignKey(
                name: "FK_users_default_words_default_words_DefaultWordId",
                table: "users_default_words",
                column: "DefaultWordId",
                principalTable: "default_words",
                principalColumn: "default_word_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_users_default_words_users_UserId",
                table: "users_default_words",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
