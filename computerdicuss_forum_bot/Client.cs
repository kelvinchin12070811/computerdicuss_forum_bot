using Discord;
using Discord.WebSocket;

namespace ComputerDiscussForumBot
{
    /// <summary>
    /// Client instance of the bot, represent as the unique instance of the bot.
    /// </summary>
    class Client
    {
        /// <summary>
        /// The main bot instance that hosting the entire bot logic.
        /// </summary>
        public DiscordSocketClient BotClient { get; private set; }
        /// <summary>
        /// Get the main instance of the singleton bot instance.
        /// </summary>
        public static Client Instance { get { return instance; } }

        private static readonly Client instance = new Client();

        public void Init()
        {
            BotClient = new DiscordSocketClient();
        }
    }
}
