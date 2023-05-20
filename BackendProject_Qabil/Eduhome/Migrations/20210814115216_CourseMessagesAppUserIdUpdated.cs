using Microsoft.EntityFrameworkCore.Migrations;

namespace Eduhome.Migrations
{
    public partial class CourseMessagesAppUserIdUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMessages_AspNetUsers_AppUserId1",
                table: "CourseMessages");

            migrationBuilder.DropIndex(
                name: "IX_CourseMessages_AppUserId1",
                table: "CourseMessages");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "CourseMessages");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "CourseMessages",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMessages_AppUserId",
                table: "CourseMessages",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMessages_AspNetUsers_AppUserId",
                table: "CourseMessages",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMessages_AspNetUsers_AppUserId",
                table: "CourseMessages");

            migrationBuilder.DropIndex(
                name: "IX_CourseMessages_AppUserId",
                table: "CourseMessages");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "CourseMessages",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "CourseMessages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseMessages_AppUserId1",
                table: "CourseMessages",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMessages_AspNetUsers_AppUserId1",
                table: "CourseMessages",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
