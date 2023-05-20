using Microsoft.EntityFrameworkCore.Migrations;

namespace Eduhome.Migrations
{
    public partial class SettingsTbleChooseImageColumnAndSlidersTbleBtnTextColumnDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BtnText",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "ChoosePic",
                table: "Settings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BtnText",
                table: "Sliders",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChoosePic",
                table: "Settings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
