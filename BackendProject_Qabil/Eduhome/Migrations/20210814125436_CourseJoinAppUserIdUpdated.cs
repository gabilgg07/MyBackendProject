using Microsoft.EntityFrameworkCore.Migrations;

namespace Eduhome.Migrations
{
    public partial class CourseJoinAppUserIdUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseJoins_AspNetUsers_AppUserId1",
                table: "CourseJoins");

            migrationBuilder.DropIndex(
                name: "IX_CourseJoins_AppUserId1",
                table: "CourseJoins");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "CourseJoins");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "CourseJoins",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CourseJoins_AppUserId",
                table: "CourseJoins",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseJoins_AspNetUsers_AppUserId",
                table: "CourseJoins",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseJoins_AspNetUsers_AppUserId",
                table: "CourseJoins");

            migrationBuilder.DropIndex(
                name: "IX_CourseJoins_AppUserId",
                table: "CourseJoins");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "CourseJoins",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "CourseJoins",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseJoins_AppUserId1",
                table: "CourseJoins",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseJoins_AspNetUsers_AppUserId1",
                table: "CourseJoins",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
