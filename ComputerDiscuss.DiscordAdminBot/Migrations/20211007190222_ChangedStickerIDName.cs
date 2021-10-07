using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerDiscuss.DiscordAdminBot.Migrations
{
    public partial class ChangedStickerIDName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Stickers",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Stickers",
                newName: "id");
        }
    }
}
