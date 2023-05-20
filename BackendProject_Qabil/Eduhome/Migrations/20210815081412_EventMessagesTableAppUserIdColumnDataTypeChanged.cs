using Microsoft.EntityFrameworkCore.Migrations;

namespace Eduhome.Migrations
{
    public partial class EventMessagesTableAppUserIdColumnDataTypeChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventMessages_AspNetUsers_AppUserId1",
                table: "EventMessages");

            migrationBuilder.DropIndex(
                name: "IX_EventMessages_AppUserId1",
                table: "EventMessages");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "EventMessages");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "EventMessages",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventMessages_AppUserId",
                table: "EventMessages",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventMessages_AspNetUsers_AppUserId",
                table: "EventMessages",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventMessages_AspNetUsers_AppUserId",
                table: "EventMessages");

            migrationBuilder.DropIndex(
                name: "IX_EventMessages_AppUserId",
                table: "EventMessages");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "EventMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "EventMessages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventMessages_AppUserId1",
                table: "EventMessages",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EventMessages_AspNetUsers_AppUserId1",
                table: "EventMessages",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
