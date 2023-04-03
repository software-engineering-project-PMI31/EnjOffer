using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnjOffer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateUserWordsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 19, 3, 3, 291, DateTimeKind.Local).AddTicks(3650),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 2, 17, 0, 58, 751, DateTimeKind.Local).AddTicks(7401));

            migrationBuilder.AddColumn<int>(
                name: "user_correct_entered_count",
                table: "user_words",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "user_incorrect_entered_count",
                table: "user_words",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "user_last_time_entered",
                table: "user_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 19, 3, 3, 289, DateTimeKind.Local).AddTicks(2699));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 19, 3, 3, 289, DateTimeKind.Local).AddTicks(5678),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 2, 17, 0, 58, 749, DateTimeKind.Local).AddTicks(9325));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_correct_entered_count",
                table: "user_words");

            migrationBuilder.DropColumn(
                name: "user_incorrect_entered_count",
                table: "user_words");

            migrationBuilder.DropColumn(
                name: "user_last_time_entered",
                table: "user_words");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 17, 0, 58, 751, DateTimeKind.Local).AddTicks(7401),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 2, 19, 3, 3, 291, DateTimeKind.Local).AddTicks(3650));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 17, 0, 58, 749, DateTimeKind.Local).AddTicks(9325),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 2, 19, 3, 3, 289, DateTimeKind.Local).AddTicks(5678));
        }
    }
}
