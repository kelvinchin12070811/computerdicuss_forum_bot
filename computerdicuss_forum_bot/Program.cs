using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace ComputerDiscussForumBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Instance.Init();
            Config.Instance.Init();

            Log.Instance.Logger.Info("Starting bot...");
            new Program().BotMain().GetAwaiter().GetResult();
        }

        public async Task BotMain()
        {
            try
            {
                Log.Instance.Logger.Info("Initializing bot...");
                Client.Instance.Init();
                Client.Instance.BotClient.Ready += OnReady;
                Client.Instance.BotClient.LoggedOut += OnLoggedOut;
                Client.Instance.BotClient.MessageReceived += OnMessageReceived;

                Log.Instance.Logger.Info("Logging into Discord...");
                await Client.Instance.BotClient.LoginAsync(TokenType.Bot, Config.Instance.Token);
                await Client.Instance.BotClient.StartAsync();
            }
            catch (Exception e)
            {
                Log.Instance.Logger.Error("Critical error occured.", e);
            }

            bool running = true;
            while (running)
            {
                string command = "";
                command = Console.ReadLine();
                if (command == "exit")
                    running = false;
            }

            Log.Instance.Logger.Info("Logging out from Discord...");
            await Client.Instance.BotClient.LogoutAsync();
        }

        private async Task OnMessageReceived(SocketMessage msg)
        {
            var usrmsg = msg as SocketUserMessage;
            if (usrmsg.Author.IsBot) return;

            var context = new SocketCommandContext(Client.Instance.BotClient, usrmsg);
            int pos = 0;

            if (context.Message.Content == "hi")
                await context.Message.ReplyAsync("Hi there!");
        }

        private Task OnLoggedOut()
        {
            Log.Instance.Logger.Info("Logged out from Discord!");
            return Task.CompletedTask;
        }

        private Task OnReady()
        {
            var currentUser = Client.Instance.BotClient.CurrentUser;
            Log.Instance.Logger.Info($"Logged in as { currentUser.Username }#{currentUser.Discriminator}!");
            return Task.CompletedTask;
        }
    }
}
