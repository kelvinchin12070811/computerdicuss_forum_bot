using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerDiscuss.DiscordAdminBot.Migrations
{
    public partial class AddedBTreeIndexToStickerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Stickers_Keyword",
                table: "Stickers",
                column: "Keyword");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stickers_Keyword",
                table: "Stickers");
        }
    }
}
