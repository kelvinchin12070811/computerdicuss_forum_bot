/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
using ComputerDiscuss.DiscordAdminBot.Messaging;
using ComputerDiscuss.DiscordAdminBot.Models;
using Discord.Commands;
using Discord.WebSocket;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerDiscuss.DiscordAdminBot.Services
{
    /// <summary>
    /// Parse messages and handle Discord's client events. Also invoke interaction that assigned to current bot.
    /// </summary>
    public class CommandHandler
    {
        /// <summary>
        /// Client that represent as current bot that logged in to the Discord.
        /// </summary>
        public static DiscordSocketClient discord;
        /// <summary>
        /// Command services that register and run or execute commands for bot,
        /// </summary>
        public static CommandService commands;
        /// <summary>
        /// Configuration of the bot.
        /// </summary>
        public static IConfigurationRoot config;
        /// <summary>
        /// ServicesProvider which provide required services or dependencies.
        /// </summary>
        public static IServiceProvider provider;
        /// <summary>
        /// Logger that perform logging.
        /// </summary>
        public static ILog log;
        /// <summary>
        /// Database of the bot.
        /// </summary>
        private readonly BotDBContext dbContext;
        /// <summary>
        /// Handler that handler specific pattern of message.
        /// </summary>
        private readonly IMessagingHandler messagingHandler;

        /// <summary>
        /// Constructor that used for dependencies injection.
        /// </summary>
        /// <param name="discord">Current client that logged in to Discord.</param>
        /// <param name="commands">Command services that register and run or execute commands for bot.</param>
        /// <param name="config">Configuration for bot.</param>
        /// <param name="provider">ServicesProvider which provide required services or dependencies.</param>
        /// <param name="log">Logger that perform logging.</param>
        /// <param name="dbContext">Database of the bot.</param>
        /// <param name="messagingHandler">Handler that handler specific pattern of message.</param>
        public CommandHandler(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config,
            IServiceProvider provider, ILog log, BotDBContext dbContext, IMessagingHandler messagingHandler)
        {
            CommandHandler.discord = discord;
            CommandHandler.commands = commands;
            CommandHandler.config = config;
            CommandHandler.provider = provider;
            CommandHandler.log = log;
            this.dbContext = dbContext;
            this.messagingHandler = messagingHandler;

            CommandHandler.discord.Ready += OnReady;
            CommandHandler.discord.LoggedOut += OnLoggedOut;
            CommandHandler.discord.MessageReceived += OnMessageReceived;
        }

        /// <summary>
        /// Event that fired when a new message arrived at the door of the bot.
        /// </summary>
        /// <param name="message">Message that received by the bot.</param>
        /// <returns>Asynchronous task that represent as the message receiving handler.</returns>
        private async Task OnMessageReceived(SocketMessage message)
        {
            var usrmsg = message as SocketUserMessage;
            if (usrmsg.Author.IsBot) return;

            var context = new SocketCommandContext(discord, usrmsg);
            int pos = 0;

            if (await messagingHandler.Exec(usrmsg))
                return;

            if (usrmsg.HasMentionPrefix(discord.CurrentUser, ref pos))
            {
                var result = await commands.ExecuteAsync(context, pos, provider);

                if (!result.IsSuccess)
                {
                    await message.Channel.SendMessageAsync($"Error: {result.Error}",
                        messageReference: context.Message.Reference);
                    log.Error($"Failed to execute command: {result.Error}");
                }
            }
        }

        /// <summary>
        /// Event that fired when the bot logged out from Discord.
        /// </summary>
        /// <returns>Asynchronous task that represent as the loggedOut event handling </returns>
        private Task OnLoggedOut()
        {
            log.Info("Bot logged out from Discord!");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Event that fired when the bot ready to operate.
        /// </summary>
        /// <returns>Asynchronous task that handling the event fired by the OnReady event.</returns>
        private Task OnReady()
        {
            var curUser = discord.CurrentUser;
            log.Info($"Logged in as {curUser.Username}#{curUser.Discriminator}.");
            return Task.CompletedTask;
        }
    }
}
