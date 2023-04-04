using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnjOffer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePriorityPropertyFromtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "priority",
                table: "users_default_words");

            migrationBuilder.DropColumn(
                name: "user_word_priority",
                table: "user_words");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 4, 2, 20, 11, 836, DateTimeKind.Local).AddTicks(6127),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 2, 19, 3, 3, 291, DateTimeKind.Local).AddTicks(3650));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_last_time_entered",
                table: "user_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 4, 2, 20, 11, 834, DateTimeKind.Local).AddTicks(7778),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 2, 19, 3, 3, 289, DateTimeKind.Local).AddTicks(2699));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 4, 2, 20, 11, 834, DateTimeKind.Local).AddTicks(9045),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 2, 19, 3, 3, 289, DateTimeKind.Local).AddTicks(5678));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 19, 3, 3, 291, DateTimeKind.Local).AddTicks(3650),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 4, 2, 20, 11, 836, DateTimeKind.Local).AddTicks(6127));

            migrationBuilder.AddColumn<int>(
                name: "priority",
                table: "users_default_words",
                type: "integer",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_last_time_entered",
                table: "user_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 19, 3, 3, 289, DateTimeKind.Local).AddTicks(2699),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 4, 2, 20, 11, 834, DateTimeKind.Local).AddTicks(7778));

            migrationBuilder.AddColumn<int>(
                name: "user_word_priority",
                table: "user_words",
                type: "integer",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 2, 19, 3, 3, 289, DateTimeKind.Local).AddTicks(5678),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 4, 4, 2, 20, 11, 834, DateTimeKind.Local).AddTicks(9045));
        }
    }
}
