/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
using Discord.Commands;
using Discord.WebSocket;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ComputerDiscuss.DiscordAdminBot.Services
{
    /// <summary>
    /// Parse messages and handle Discord's client events. Also invoke interaction that assigned to current bot.
    /// </summary>
    public class CommandHandler
    {
        public static DiscordSocketClient discord;
        public static CommandService commands;
        public static IConfigurationRoot config;
        public static IServiceProvider provider;
        public static ILog log;

        public CommandHandler(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config,
            IServiceProvider provider, ILog log)
        {
            CommandHandler.discord = discord;
            CommandHandler.commands = commands;
            CommandHandler.config = config;
            CommandHandler.provider = provider;
            CommandHandler.log = log;

            CommandHandler.discord.Ready += OnReady;
            CommandHandler.discord.LoggedOut += OnLoggedOut;
            CommandHandler.discord.MessageReceived += OnMessageReceived;
        }

        private async Task OnMessageReceived(SocketMessage message)
        {
            var usrmsg = message as SocketUserMessage;
            if (usrmsg.Author.IsBot) return;

            var context = new SocketCommandContext(discord, usrmsg);
            int pos = 0;

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

        private Task OnLoggedOut()
        {
            log.Info("Bot logged out from Discord!");
            return Task.CompletedTask;
        }

        private Task OnReady()
        {
            var curUser = discord.CurrentUser;
            log.Info($"Logged in as {curUser.Username}#{curUser.Discriminator}.");
            return Task.CompletedTask;
        }
    }
}
