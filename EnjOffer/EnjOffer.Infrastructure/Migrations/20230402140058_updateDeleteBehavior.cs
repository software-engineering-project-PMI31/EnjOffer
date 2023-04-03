using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnjOffer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_statistics_users_UserId",
                table: "user_statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_user_words_users_UserId",
                table: "user_words");

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
                defaultValue: new DateTime(2023, 4, 2, 17, 0, 58, 751, DateTimeKind.Local).AddTicks(7401),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 2, 16, 51, 37, 868, DateTimeKind.Local).AddTicks(682));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 17, 0, 58, 749, DateTimeKind.Local).AddTicks(9325),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 2, 16, 51, 37, 866, DateTimeKind.Local).AddTicks(5839));

            migrationBuilder.AddForeignKey(
                name: "FK_user_statistics_users_UserId",
                table: "user_statistics",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_user_words_users_UserId",
                table: "user_words",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.SetNull);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_statistics_users_UserId",
                table: "user_statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_user_words_users_UserId",
                table: "user_words");

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
                defaultValue: new DateTime(2023, 4, 2, 16, 51, 37, 868, DateTimeKind.Local).AddTicks(682),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 2, 17, 0, 58, 751, DateTimeKind.Local).AddTicks(7401));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 16, 51, 37, 866, DateTimeKind.Local).AddTicks(5839),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 2, 17, 0, 58, 749, DateTimeKind.Local).AddTicks(9325));

            migrationBuilder.AddForeignKey(
                name: "FK_user_statistics_users_UserId",
                table: "user_statistics",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_words_users_UserId",
                table: "user_words",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_default_words_default_words_DefaultWordId",
                table: "users_default_words",
                column: "DefaultWordId",
                principalTable: "default_words",
                principalColumn: "default_word_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_default_words_users_UserId",
                table: "users_default_words",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
