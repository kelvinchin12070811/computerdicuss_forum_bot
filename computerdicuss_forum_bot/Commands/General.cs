/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using log4net;
using System;
using System.Threading.Tasks;

namespace ComputerDiscuss.DiscordAdminBot.Modules
{
    /// <summary>
    /// General commands, includes all uncategorised commands.
    /// </summary>
    public class General : ModuleBase
    {
        /// <summary>
        /// Logger that used in logging.
        /// </summary>
        private readonly ILog log;

        /// <summary>
        /// Constructor that used for dependencies injection.
        /// </summary>
        /// <param name="log">Logger instance used for logging.</param>
        public General(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// Ping command, echo back to user and tell them their latency between message sent and received by the bot.
        /// Also latency between the bot to discord.
        /// </summary>
        /// <returns>Asynchronous task that represent the execution of the command.</returns>
        [Command("ping")]
        public async Task Ping()
        {
            var msg = Context.Message;
            var latency = Math.Abs(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                - msg.Timestamp.ToUnixTimeMilliseconds());
            var client = Context.Client as DiscordSocketClient;

            var buildEmbed = new EmbedBuilder()
                .WithColor(new Color(0x00, 0xff, 0x00))
                .WithThumbnailUrl(
                    "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png"
                )
                .WithTitle("Pong")
                .AddField(new EmbedFieldBuilder()
                        .WithName(":robot: Latency")
                        .WithValue($"{latency}ms"))
                .AddField(new EmbedFieldBuilder()
                        .WithName(":globe_with_meridians: Latency")
                        .WithValue($"{client.Latency}ms"))
                .WithCurrentTimestamp()
                .WithFooter(client.CurrentUser.Username,
                    "https://cdn.discordapp.com/avatars/890272720120586260/c325408b5c71f09ee12dd1606917abb5.png");
            var embed = buildEmbed.Build();
            await Context.Channel.SendMessageAsync(embed: embed,
                messageReference: new MessageReference(msg.Id, Context.Channel.Id, Context.Guild.Id));
        }
    }
}
