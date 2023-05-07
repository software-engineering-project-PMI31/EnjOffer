using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnjOffer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cascadeDeleteInUsersDefaultWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 33, 52, 311, DateTimeKind.Local).AddTicks(7325),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 32, 47, 51, DateTimeKind.Local).AddTicks(5028));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_last_time_entered",
                table: "user_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 33, 52, 289, DateTimeKind.Local).AddTicks(7401),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 32, 47, 49, DateTimeKind.Local).AddTicks(4792));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 33, 52, 289, DateTimeKind.Local).AddTicks(8603),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 32, 47, 49, DateTimeKind.Local).AddTicks(6378));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "last_time_entered",
                table: "users_default_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 32, 47, 51, DateTimeKind.Local).AddTicks(5028),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 33, 52, 311, DateTimeKind.Local).AddTicks(7325));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_last_time_entered",
                table: "user_words",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 32, 47, 49, DateTimeKind.Local).AddTicks(4792),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 33, 52, 289, DateTimeKind.Local).AddTicks(7401));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_statistic_answer_date",
                table: "user_statistics",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(2023, 5, 2, 15, 32, 47, 49, DateTimeKind.Local).AddTicks(6378),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValue: new DateTime(2023, 5, 2, 15, 33, 52, 289, DateTimeKind.Local).AddTicks(8603));
        }
    }
}
