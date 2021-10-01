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

namespace ComputerDiscuss.DiscordAdminBot.Commands
{
    /// <summary>
    /// General commands, includes all uncategorised commands.
    /// </summary>
    public class General : CommandBase
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

            var embed = GetEmbedWithSuccessTemplate("Pong!")
                .WithFields(
                    new EmbedFieldBuilder[]
                    {
                        new EmbedFieldBuilder()
                            .WithName(":robot: Latency")
                            .WithValue($"{latency}ms"),
                        new EmbedFieldBuilder()
                            .WithName(":globe_with_meridians: Latency")
                            .WithValue($"{client.Latency}ms")
                    });
            await ReplyAsync(embed: embed.Build(), messageReference: new MessageReference(msg.Id));
        }

        /// <summary>
        /// Command that used to set the "Playing" status of the bot, only admin can use this command.
        /// </summary>
        /// <param name="game">Game to played by the bot.</param>
        /// <returns>Asynchronous task that execute the command handler.</returns>
        [Command("game")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetGame([Remainder] string game)
        {
            var refMsg = new MessageReference(Context.Message.Id);
            await (Context.Client as DiscordSocketClient).SetGameAsync(game);
            var embed = GetEmbedWithSuccessTemplate("Set Playing Status")
                .AddField("Playing Status Changed", $"Now playing {game}");
            await ReplyAsync(embed: embed.Build(), messageReference: refMsg);
        }
    }
}