using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnjOffer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedUserDefaultWordsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "correct_entered_count",
                table: "users_default_words",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "incorrect_entered_count",
                table: "users_default_words",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 16, 51, 37, 868, DateTimeKind.Local).AddTicks(682));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 16, 51, 37, 866, DateTimeKind.Local).AddTicks(5839),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 3, 28, 5, 22, 40, 129, DateTimeKind.Local).AddTicks(9219));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "correct_entered_count",
                table: "users_default_words");

            migrationBuilder.DropColumn(
                name: "incorrect_entered_count",
                table: "users_default_words");

            migrationBuilder.DropColumn(
                name: "last_time_entered",
                table: "users_default_words");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 28, 5, 22, 40, 129, DateTimeKind.Local).AddTicks(9219),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 2, 16, 51, 37, 866, DateTimeKind.Local).AddTicks(5839));
        }
    }
}
