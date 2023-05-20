using Microsoft.EntityFrameworkCore.Migrations;

namespace Eduhome.Migrations
{
    public partial class SettingTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone2",
                table: "Settings",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone1",
                table: "Settings",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AboutDesc",
                table: "Settings",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AboutPic",
                table: "Settings",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AboutTitle",
                table: "Settings",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AboutVideo",
                table: "Settings",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChoosePic",
                table: "Settings",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChooseTitle",
                table: "Settings",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorLogo",
                table: "Settings",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "Settings",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pinteres",
                table: "Settings",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                table: "Settings",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vimeo",
                table: "Settings",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutDesc",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "AboutPic",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "AboutTitle",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "AboutVideo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ChoosePic",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ChooseTitle",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ColorLogo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Pinteres",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Twitter",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Vimeo",
                table: "Settings");

            migrationBuilder.AlterColumn<string>(
                name: "Phone2",
                table: "Settings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone1",
                table: "Settings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);
        }
    }
}
