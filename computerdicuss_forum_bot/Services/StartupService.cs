/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ComputerDiscuss.DiscordAdminBot.Services
{
    public class StartupService
    {
        public static IServiceProvider provider;
        private readonly DiscordSocketClient discord;
        private readonly CommandService commands;
        private readonly IConfigurationRoot config;
        private readonly ILog log;

        public StartupService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands,
            IConfigurationRoot config, ILog log)
        {
            StartupService.provider = provider;
            this.discord = discord;
            this.commands = commands;
            this.config = config;
            this.log = log;
        }

        public async Task StartAsync()
        {
            string token = config["token"];
            if (string.IsNullOrEmpty(token))
            {
                log.Fatal("Can't start bot as token did not supplied!");
                Environment.Exit(1);
            }

            log.Info("Logging in to discord...");
            await discord.LoginAsync(Discord.TokenType.Bot, token);
            await discord.StartAsync();

            log.Info("Registering commands...");
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), provider);
        }

        public async Task StopAsync()
        {
            log.Info("Logging out from Discord...");
            await discord.LogoutAsync();
        }
    }
}
