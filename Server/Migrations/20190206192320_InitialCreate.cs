using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "storage",
                columns: table => new
                {
                    file_name = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    time_created = table.Column<DateTime>(nullable: false),
                    time_expiring = table.Column<DateTime>(nullable: false),
                    downloads_left = table.Column<int>(nullable: false),
                    password = table.Column<string>(nullable: true),
                    file_hash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_storage", x => x.file_name);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "storage");
        }
    }
}
