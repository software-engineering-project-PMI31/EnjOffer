using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnjOffer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedNullConstraintsInIdsInUsersDefaultWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 20, 28, 363, DateTimeKind.Local).AddTicks(3564),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 10, 34, 40, 896, DateTimeKind.Local).AddTicks(5614));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_last_time_entered",
                table: "user_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 20, 28, 361, DateTimeKind.Local).AddTicks(4506),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 10, 34, 40, 894, DateTimeKind.Local).AddTicks(5777));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 20, 28, 361, DateTimeKind.Local).AddTicks(6021),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 10, 34, 40, 894, DateTimeKind.Local).AddTicks(7383));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 10, 34, 40, 896, DateTimeKind.Local).AddTicks(5614),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 20, 28, 363, DateTimeKind.Local).AddTicks(3564));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_last_time_entered",
                table: "user_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 10, 34, 40, 894, DateTimeKind.Local).AddTicks(5777),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 20, 28, 361, DateTimeKind.Local).AddTicks(4506));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 10, 34, 40, 894, DateTimeKind.Local).AddTicks(7383),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 20, 28, 361, DateTimeKind.Local).AddTicks(6021));
        }
    }
}
