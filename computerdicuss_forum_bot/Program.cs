using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Threading;

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
            var cancelToken = new CancellationTokenSource();
            Console.CancelKeyPress += delegate {
                cancelToken.Cancel();
                Log.Instance.Logger.Info("Logging out from Discord...");
                Client.Instance.BotClient.LogoutAsync();
                Log.Instance.Logger.Info("Logged out from discord.");
            };

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

            try
            {
                await Task.Delay(-1, cancelToken.Token);
            }
            catch (TaskCanceledException e)
            {
                Log.Instance.Logger.Info("Interupt request accepted.", e);
                return;
            }
            catch (Exception e)
            {
                Log.Instance.Logger.Fatal("Critical error occured.", e);
                Environment.Exit(1);
            }
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
