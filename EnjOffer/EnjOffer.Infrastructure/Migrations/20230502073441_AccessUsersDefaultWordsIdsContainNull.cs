using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnjOffer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AccessUsersDefaultWordsIdsContainNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 10, 34, 40, 896, DateTimeKind.Local).AddTicks(5614),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 1, 22, 28, 52, 9, DateTimeKind.Local).AddTicks(4409));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_last_time_entered",
                table: "user_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 10, 34, 40, 894, DateTimeKind.Local).AddTicks(5777),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 1, 22, 28, 52, 4, DateTimeKind.Local).AddTicks(6536));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 10, 34, 40, 894, DateTimeKind.Local).AddTicks(7383),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 1, 22, 28, 52, 5, DateTimeKind.Local).AddTicks(429));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 1, 22, 28, 52, 9, DateTimeKind.Local).AddTicks(4409),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 10, 34, 40, 896, DateTimeKind.Local).AddTicks(5614));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_last_time_entered",
                table: "user_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 1, 22, 28, 52, 4, DateTimeKind.Local).AddTicks(6536),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 10, 34, 40, 894, DateTimeKind.Local).AddTicks(5777));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 1, 22, 28, 52, 5, DateTimeKind.Local).AddTicks(429),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 10, 34, 40, 894, DateTimeKind.Local).AddTicks(7383));
        }
    }
}
