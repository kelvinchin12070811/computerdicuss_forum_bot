using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerDiscussForumBot.Services
{
    public class StartupService
    {
        public static IServiceProvider provider;
        private readonly DiscordSocketClient discord;
        private readonly CommandService commands;
        private readonly IConfigurationRoot config;

        public StartupService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands,
            IConfigurationRoot config)
        {
            StartupService.provider = provider;
            this.discord = discord;
            this.commands = commands;
            this.config = config;
        }

        public async Task StartAsync()
        {
            string token = config["token"];
            if (String.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token is empty");
            }
        }
    }
}
