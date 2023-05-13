using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnjOffer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class setBeahaviourOnDeleteForUserStatisticsAndWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_statistics_AspNetUsers_UserId",
                table: "user_statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_user_words_AspNetUsers_UserId",
                table: "user_words");

            migrationBuilder.AddForeignKey(
                name: "FK_user_statistics_AspNetUsers_UserId",
                table: "user_statistics",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_words_AspNetUsers_UserId",
                table: "user_words",
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
        }
    }
}
