﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using log4net;
using System;
using System.Threading.Tasks;

namespace ComputerDiscussForumBot.Modules
{
    public class General : ModuleBase
    {
        private readonly ILog log;

        public General(ILog log)
        {
            this.log = log;
        }

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
