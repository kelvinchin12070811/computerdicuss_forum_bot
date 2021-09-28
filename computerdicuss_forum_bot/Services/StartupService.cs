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
    /// <summary>
    /// Service that handle the start-up of the bot.
    /// </summary>
    public class StartupService
    {
        /// <summary>
        /// ServicesProvider that contains all the required dependencies.
        /// </summary>
        public static IServiceProvider provider;
        /// <summary>
        /// Discord client that logged into the Discord.
        /// </summary>
        private readonly DiscordSocketClient discord;
        /// <summary>
        /// CommandService that register all the required command and run or execute them.
        /// </summary>
        private readonly CommandService commands;
        /// <summary>
        /// Configuration of the bot.
        /// </summary>
        private readonly IConfigurationRoot config;
        /// <summary>
        /// Logger that perform logging.
        /// </summary>
        private readonly ILog log;

        /// <summary>
        /// Constructor that perform dependencies injection.
        /// </summary>
        /// <param name="provider">ServicesProvider that contains all the required dependencies.</param>
        /// <param name="discord">Discord client that logged into the Discord.</param>
        /// <param name="commands">CommandService that register all the required command and run or execute.</param>
        /// <param name="config">Configuration of the bot</param>
        /// <param name="log">Logger that perform logging.</param>
        public StartupService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands,
            IConfigurationRoot config, ILog log)
        {
            StartupService.provider = provider;
            this.discord = discord;
            this.commands = commands;
            this.config = config;
            this.log = log;
        }

        /// <summary>
        /// Start and initialize the StartupService. Also log the bot into Discord.
        /// </summary>
        /// <returns>Asynchronous task that represent as the task where initialize and start the service up.</returns>
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

        /// <summary>
        /// Stop and log the bot out from Discord.
        /// </summary>
        /// <returns>Asynchronous task that represent as the operation where stop and logout the bot.</returns>
        public async Task StopAsync()
        {
            log.Info("Logging out from Discord...");
            await discord.LogoutAsync();
        }
    }
}
