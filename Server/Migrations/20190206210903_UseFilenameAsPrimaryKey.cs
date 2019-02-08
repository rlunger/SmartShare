using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Migrations
{
    public partial class UseFilenameAsPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "storage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "storage",
                nullable: false,
                defaultValue: 0);
        }
    }
}
