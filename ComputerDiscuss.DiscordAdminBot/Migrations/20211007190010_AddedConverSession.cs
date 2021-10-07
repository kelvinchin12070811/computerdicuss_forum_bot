using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerDiscuss.DiscordAdminBot.Migrations
{
    public partial class AddedConverSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConverSessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageId = table.Column<long>(type: "INTEGER", nullable: false),
                    ChannelId = table.Column<long>(type: "INTEGER", nullable: false),
                    GuildId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedTime = table.Column<long>(type: "INTEGER", nullable: false),
                    Lifetime = table.Column<long>(type: "INTEGER", nullable: false),
                    Context = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConverSessions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConverSessions");
        }
    }
}
