using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerDiscuss.DiscordAdminBot.Migrations
{
    public partial class RemovedClosedFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Closed",
                table: "ConverSessions");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "ConverSessions",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "ConverSessions");

            migrationBuilder.AddColumn<bool>(
                name: "Closed",
                table: "ConverSessions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
