using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerDiscuss.DiscordAdminBot.Migrations
{
    public partial class ChangedStickerIDType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Stickers",
                newName: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Stickers",
                newName: "Id");
        }
    }
}
