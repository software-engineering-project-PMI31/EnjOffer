using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnjOffer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class connectedAspNetUsersInsteadUsers : Migration
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
                name: "FK_users_default_words_users_UserId",
                table: "users_default_words");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.AddForeignKey(
                name: "FK_user_statistics_AspNetUsers_UserId",
                table: "user_statistics",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_words_AspNetUsers_UserId",
                table: "user_words",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_default_words_AspNetUsers_UserId",
                table: "users_default_words",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_statistics_AspNetUsers_UserId",
                table: "user_statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_user_words_AspNetUsers_UserId",
                table: "user_words");

            migrationBuilder.DropForeignKey(
                name: "FK_users_default_words_AspNetUsers_UserId",
                table: "users_default_words");

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

            migrationBuilder.CreateIndex(
                name: "IX_users_user_email",
                table: "users",
                column: "user_email",
                unique: true);

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
                name: "FK_users_default_words_users_UserId",
                table: "users_default_words",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
