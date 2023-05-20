using Microsoft.EntityFrameworkCore.Migrations;

namespace Eduhome.Migrations
{
    public partial class TeachersTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Communication",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Design",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Development",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Innovation",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "TeamLeader",
                table: "Teachers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Communication",
                table: "Teachers",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Design",
                table: "Teachers",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Development",
                table: "Teachers",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Innovation",
                table: "Teachers",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Language",
                table: "Teachers",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "TeamLeader",
                table: "Teachers",
                type: "tinyint",
                nullable: true);
        }
    }
}
