/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
using ComputerDiscuss.DiscordAdminBot.Models;
using Discord;
using Discord.WebSocket;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComputerDiscuss.DiscordAdminBot.Messaging
{
    /// <summary>
    /// The default messaging executor.
    /// </summary>
    class DefaultMessagingExecutor : IMessagingExecutor
    {
        /// <summary>
        /// Pattern of cancel keyword where used to cancel the session.
        /// </summary>
        private static Regex kwCancel = new Regex("^(?i)cancel$");

        /// <summary>
        /// Logger for logging.
        /// </summary>
        private ILog log = null;
        /// <summary>
        /// Discord client where the bot is logged on.
        /// </summary>
        private DiscordSocketClient client = null;
        /// <summary>
        /// Database connection of the bot.
        /// </summary>
        private BotDBContext dbContext = null;

        /// <inheritdoc/>
        public void ApplyDependencies(IServiceProvider provider)
        {
            log = provider.GetRequiredService<ILog>();
            client = provider.GetRequiredService<DiscordSocketClient>();
            dbContext = provider.GetRequiredService<BotDBContext>();
        }

        /// <inheritdoc/>
        public Task<bool> DistributeExecution(SocketMessage message, ConverSession session)
        {
            var action = session.Action;

            switch (action)
            {
                case "sticker_add":
                    _ = HandleAddSticker(message, session);
                    break;
                case "sticker_rename":
                    _ = HandleRenameSticker(message, session);
                    break;
                default:
                    return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        /// <inheritdoc/>
        public async Task HandleAddSticker(SocketMessage message, ConverSession session)
        {
            var context = JObject.Parse(session.Context);
            var stickerName = (string)context["sticker_name"];
            var stickerURI = message.Content.Trim();
            var msgRef = new MessageReference(message.Id);

            if (kwCancel.Match(stickerURI).Success || string.IsNullOrEmpty(stickerURI))
            {
                await CloseSession(session);
                var embed = GetDefaultErrorEmbedBuilder("Action Cancled!", client.CurrentUser).Build();
                await message.Channel.SendMessageAsync(embed: embed, messageReference: msgRef);
                return;
            }

            try
            {
                var nSticker = new ComputerDiscuss.DiscordAdminBot.Models.Sticker(stickerName, stickerURI);
                await dbContext.Stickers.AddAsync(nSticker);
                await dbContext.SaveChangesAsync();

                var embed = GetDefaultSuccessEmbedBuilder("Add Sticker", client.CurrentUser)
                    .AddField("Sticker Added", $"Sticker \"{stickerName}\" has been added to library!")
                    .WithThumbnailUrl(stickerURI)
                    .Build();
                await message.Channel.SendMessageAsync(embed: embed, messageReference: msgRef);
                await CloseSession(session);
            }
            catch (Exception e)
            {
                log.Error("Exception occurred: ", e);
                var embed = GetDefaultErrorEmbedBuilder("Error Ocurred!", client.CurrentUser)
                    .AddField("An error has ocurred!", e.Message)
                    .Build();
                await message.Channel.SendMessageAsync(embed: embed);
            }
        }

        /// <inheritdoc/>
        public async Task HandleRenameSticker(SocketMessage message, ConverSession session)
        {
            var nwStickerName = message.Content.Trim();
            log.Info($"message: {message.Content}");
            log.Info($"message after trimmed: {nwStickerName}");
            var msgRef = new MessageReference(message.Id);

            if (kwCancel.Match(nwStickerName).Success || string.IsNullOrEmpty(nwStickerName))
            {
                var cancelMsg = GetDefaultSuccessEmbedBuilder("Action Canceled!", client.CurrentUser).Build();
                await message.Channel.SendMessageAsync(embed: cancelMsg, messageReference: msgRef);
                await CloseSession(session);
                return;
            }

            if ((from sticker in dbContext.Stickers.ToEnumerable()
                 where sticker.Keyword == nwStickerName
                 select sticker).FirstOrDefault() != null)
            {
                var errMsg = GetDefaultErrorEmbedBuilder("Rename Sticker", client.CurrentUser)
                    .AddField("Selected Keyword already exits",
                        $"There's already have a sticker called {nwStickerName}! Use \"@{client.CurrentUser.Username} "
                        + "sticker list\" to get a clue on them.")
                    .Build();
                await message.Channel.SendMessageAsync(embed: errMsg, messageReference: msgRef);
                await CloseSession(session);
                return;
            }

            var sessionContext = JObject.Parse(session.Context);
            var tgSticker = (from sticker in dbContext.Stickers.ToEnumerable()
                             where sticker.Keyword == (string)sessionContext["sticker_name"]
                             select sticker).FirstOrDefault();
            tgSticker.Keyword = nwStickerName;

            try
            {
                await dbContext.SaveChangesAsync();
                var successMsg = GetDefaultSuccessEmbedBuilder("Rename Sticker", client.CurrentUser)
                    .AddField("Sticker Renamed!", $"Sticker \"{(string)sessionContext["sticker_name"]}\" has been "
                        + $"renamed to \"{nwStickerName}\".")
                    .Build();
                await message.Channel.SendMessageAsync(embed: successMsg, messageReference: msgRef);
            }
            catch (Exception ex)
            {
                log.Error("Exception occured!", ex);
                var errMsg = GetDefaultErrorEmbedBuilder("Rename Sticker", client.CurrentUser)
                    .AddField("Internal Server Error", ex.Message)
                    .Build();
                await message.Channel.SendMessageAsync(embed: errMsg, messageReference: msgRef);
            }

            await CloseSession(session);
        }

        /// <inheritdoc/>
        public async Task CloseSession(ConverSession session)
        {
            dbContext.ConverSessions.Remove(session);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public EmbedBuilder GetDefaultEmbedBuilder(string title, SocketUser user)
        {
            return new EmbedBuilder()
                .WithTitle(title)
                .WithCurrentTimestamp()
                .WithAuthor(user.Username, user.GetAvatarUrl());
        }

        /// <inheritdoc/>
        public EmbedBuilder GetDefaultSuccessEmbedBuilder(string title, SocketUser user)
        {
            return GetDefaultEmbedBuilder(title, user)
                .WithColor(0x00, 0xff, 0x00);
        }

        /// <inheritdoc/>

        public EmbedBuilder GetDefaultErrorEmbedBuilder(string title, SocketUser user)
        {
            return GetDefaultEmbedBuilder(title, user)
                .WithColor(0xff, 0x00, 0x00);
        }
    }
}
