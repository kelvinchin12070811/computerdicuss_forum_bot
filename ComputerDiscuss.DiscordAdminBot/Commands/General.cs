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

        /// <summary>
        /// A command that return link to the bot's issues tracker.
        /// </summary>
        /// <returns>Asynchronous task that execute the command handler.</returns>
        [Command("issues")]
        public async Task IssuesTrackerPortal()
        {
            var refMsg = new MessageReference(Context.Message.Id);
            var iTrackerLink = "https://github.com/kelvinchin12070811/computerdicuss_forum_bot/issues";
            await ReplyAsync($"Issues Tracker Protal:\n{iTrackerLink}", messageReference: refMsg);
        }

        [Command("who are you")]
        public async Task GetBotInfo()
        {
            await ReplyAsync($"{Context.Client.CurrentUser.Username} is a bot that created by a team of members of " +
                "an asian ICT discussion forum and is open sourced under MPL 2.0, visit the github repository with " +
                "the link bellow for more information:\nhttps://github.com/kelvinchin12070811/" +
                "computerdicuss_forum_bot\n\n~~but ussually only one developer is working on it, LOL~~");
        }

        /// <summary>
        /// Retrive Google search directly via the bot, no longer need to open browser and search Google on Google.
        /// </summary>
        /// <param name="keyword">Keywords to search.</param>
        /// <returns>Asynchonous task that execute the command handler.</returns>
        [Command("gg")]
        public async Task GoogleThat([Remainder] string keyword)
        {
            const string googleBaseURL = "https://www.google.com/search?q={0}";
            var refMsg = new MessageReference(Context.Message.Id);
            var query = string.Format(googleBaseURL, keyword.Trim().Replace(" ", "+"));
            var replyMsg = new EmbedBuilder();

            replyMsg.WithTitle("Click here to get Google result.");
            replyMsg.WithUrl(query);
            replyMsg.WithColor(66, 133, 244);
            replyMsg.WithFooter("Powered by Google", "https://icons.duckduckgo.com/ip3/www.google.com.ico");

            await ReplyAsync(embed: replyMsg.Build(), messageReference: refMsg);
        }

        /// <summary>
        /// Retrive DuckDuckGo serach directly via the bot, no longer need to open browser and search DuckDuckGo on
        /// DuckDuckGo.
        /// </summary>
        /// <param name="keyword">Keywords to search.</param>
        /// <returns>Asynchonous task that execut the command handler.</returns>
        [Command("ddg")]
        public async Task DuckDuckGoThat([Remainder] string keyword)
        {
            const string ddgBaseURL = "https://www.duckduckgo.com/?q={0}";
            var refMsg = new MessageReference(Context.Message.Id);
            var query = string.Format(ddgBaseURL, keyword.Trim().Replace(" ", "+"));
            var replyMsg = new EmbedBuilder();

            replyMsg.WithTitle("Click here to get DuckDuckGo result.");
            replyMsg.WithUrl(query);
            replyMsg.WithColor(227, 113, 81);
            replyMsg.WithFooter("Powered by DuckDuckGo", "https://icons.duckduckgo.com/ip3/www.duckduckgo.com.ico");

            await ReplyAsync(embed: replyMsg.Build(), messageReference: refMsg);
        }
    }
}