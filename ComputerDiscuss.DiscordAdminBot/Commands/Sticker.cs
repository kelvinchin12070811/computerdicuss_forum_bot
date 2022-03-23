/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/

using ComputerDiscuss.DiscordAdminBot.Commands.DTOs.Stickers;
using ComputerDiscuss.DiscordAdminBot.Models;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using log4net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComputerDiscuss.DiscordAdminBot.Commands
{
    /// <summary>
    /// Commands that related to sticker.
    /// </summary>
    [Group("sticker")]
    public class Sticker : CommandBase
    {
        private static readonly Regex validURL = new("^http(s)://");

        /// <summary>
        /// Logger that use for logging.
        /// </summary>
        private readonly ILog logger;

        /// <summary>
        /// Database context that used to access data in database.
        /// </summary>
        private readonly BotDBContext dbContext;

        /// <summary>
        /// Configuration of the bot.
        /// </summary>
        private readonly IConfigurationRoot config;

        /// <summary>
        /// Constructor that used to perform dependencies injection.
        /// </summary>
        /// <param name="logger">Logger use for logging</param>
        /// <param name="dbContext">Database context used to access data in database.</param>
        /// <param name="config">Configuration of the bot.</param>
        public Sticker(ILog logger, BotDBContext dbContext, IConfigurationRoot config)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.config = config;
        }

        /// <summary>
        /// Command that list all stickers in the library.
        /// </summary>
        /// <returns>Asynchronous task that represent as operation that list all stickers in the database.</returns>
        [Command("list")]
        public async Task ListSticker()
        {
            var msg = Context.Message;
            var client = Context.Client;
            var refMsg = new MessageReference(msg.Id);
            var stickers = (from sticker in dbContext.Stickers.AsEnumerable()
                            orderby sticker.Keyword ascending
                            select sticker.Keyword).ToList();

            if (stickers.Count == 0)
            {
                await ReplyAsync("No stickers in library,", messageReference: refMsg);
                return;
            }

            string stickerList = string.Join(", ", stickers);
            await ReplyAsync($"Stickers available:\n{stickerList}", messageReference: refMsg);
        }

        /// <summary>
        /// Command that send a sticker selected by the user. Return an error message to user if the sticker with given
        /// keyword did not found in the library.
        /// </summary>
        /// <param name="stickerName">Name or keyword of the sticker to send.</param>
        /// <returns>Asynchronous task of the SendSticker command's handler</returns>
        [Command("send")]
        public async Task SendSticker([Remainder] string stickerName)
        {
            var msg = Context.Message;
            var refMsg = new MessageReference(msg.Id);
            var target = (from sticker in dbContext.Stickers.AsEnumerable()
                          where new Regex($"{stickerName}", RegexOptions.IgnoreCase).IsMatch(sticker.Keyword)
                          select sticker).FirstOrDefault();

            if (target == null)
            {
                await ReplyAsync($"Sticker {stickerName} does not exits in library.", messageReference: refMsg);
                return;
            }

            await ReplyAsync(target.URI, messageReference: refMsg);
        }

        /// <summary>
        /// Reply a specific message with sticker, by replying a message while executing command.
        /// </summary>
        /// <param name="keyword">Keyword of the sticker to send</param>
        /// <returns>Asynchronous task that current command handler running on.</returns>
        [Command("reply")]
        public async Task ReplyWithSticker([Remainder] string keyword)
        {
            var tgMsg = Context.Message.ReferencedMessage as SocketMessage;
            var refMsg = new MessageReference(Context.Message.Id);

            if (tgMsg == null)
            {
                var embed = GetEmbedWithErrorTemplate("Reply With Sticker")
                    .AddField("Error",
                        "No reply message specified, make sure you reply with a message with this command");
                await ReplyAsync(embed: embed.Build(), messageReference: refMsg);
                return;
            }

            Models.Sticker sticker = null;

            try
            {
                sticker = (from dbSticker in dbContext.Stickers
                           where new Regex($"{keyword}", RegexOptions.IgnoreCase).IsMatch(dbSticker.Keyword)
                           select dbSticker).FirstOrDefault();

                if (sticker == null)
                {
                    var embed = GetEmbedWithErrorTemplate("Reply With Sticker")
                        .AddField("Error", $"Sticker \"{keyword}\" did not exist in library.");
                    await ReplyAsync(embed: embed.Build(), messageReference: refMsg);
                    return;
                }

                await ReplyAsync(sticker.URI, messageReference: new MessageReference(tgMsg.Id));
                await Context.Channel.DeleteMessageAsync(refMsg.MessageId.Value);
            }
            catch (Exception e)
            {
                logger.Error("Exception occurred!", e);
                await ReplyWithInternalServerError(refMsg);
            }
        }

        /// <summary>
        /// Let a user to preview the selected sticker, the sticker and the caller command will be deleted after
        /// certain time.
        /// </summary>
        /// <param name="keyword">Keyword of sticker to preview.</param>
        /// <returns>Asynchronous task that host the executing methods.</returns>
        [Command("preview")]
        public async Task PreviewSticker([Remainder] string keyword)
        {
            var refMsg = new MessageReference(Context.Message.Id);

            if (!int.TryParse(config["auto dispose duration"], out int autoDisposeDuration))
            {
                logger.Warn("Auto dispose duration parse failed, fallback to default value.");
                autoDisposeDuration = 5000;
            }

            try
            {
                var sticker = (from dbSticker in dbContext.Stickers
                               where new Regex($"{keyword}", RegexOptions.IgnoreCase).IsMatch(dbSticker.Keyword)
                               select dbSticker).FirstOrDefault();

                if (sticker == null)
                {
                    var embed = GetEmbedWithErrorTemplate("Preview Sticker")
                        .AddField("No Sticker Found", $"Sticker \"{keyword}\" did not exist in library.");
                    var notFoundMsg = await ReplyAsync(embed: embed.Build(), messageReference: refMsg);
                    await Task.Delay(autoDisposeDuration);
                    _ = Context.Channel.DeleteMessageAsync(refMsg.MessageId.Value);
                    _ = Context.Channel.DeleteMessageAsync(notFoundMsg.Id);
                    return;
                }

                var replied = await ReplyAsync(sticker.URI, messageReference: refMsg);
                await Task.Delay(autoDisposeDuration);
                _ = Context.Channel.DeleteMessageAsync(refMsg.MessageId.Value);
                _ = Context.Channel.DeleteMessageAsync(replied.Id);
            }
            catch (Exception e)
            {
                logger.Error("Exception occurred!", e);
                await ReplyWithInternalServerError(refMsg);
            }
        }

        /// <summary>
        /// Allow an admin to rename an existing sticker to another keyword. The new keyword must not already exist in
        /// the library.
        /// </summary>
        /// <param name="args">
        /// Parameters of the command refer <see cref="StickerRenameOperationType" /> for more info.
        /// </param>
        /// <returns>Asynchronous task that current command handler running on.</returns>
        [Command("rename")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RenameSticker([Remainder]String keyword)
        {
            var refMsg = new MessageReference(Context.Message.Id);
            var tgSticker = (from sticker in dbContext.Stickers
                             where new Regex($"{keyword}", RegexOptions.IgnoreCase).IsMatch(sticker.Keyword)
                             select sticker).FirstOrDefault();

            if (tgSticker == null)
            {
                var errMsg = GetEmbedWithErrorTemplate("Rename sticker")
                    .AddField("Sticker not exist", $"Can't find sticker {keyword} in the library.")
                    .Build();
                await ReplyAsync(embed: errMsg, messageReference: refMsg);
                return;
            }

            var successMsg = GetEmbedWithSuccessTemplate("Rename sticker")
                .AddField("What's is the new keyword of the sticker?", "Reply this message to complete the action or "
                    + "reply \"cancel\" to cancel the action.")
                .WithThumbnailUrl(tgSticker.URI)
                .Build();
            var converStartMsg = await ReplyAsync(embed: successMsg, messageReference: refMsg);
            var msgAuthor = Context.Message.Author;
            var converSession = new ConverSession
            {
                MessageId = converStartMsg.Id,
                ChannelId = Context.Channel.Id,
                GuildId = Context.Guild.Id,
                CreatedTime = Context.Message.CreatedAt.ToUnixTimeSeconds(),
                Username = msgAuthor.Username,
                Discriminator = msgAuthor.Discriminator,
                Lifetime = 5 * 3600,
                Action = "sticker_rename",
                Context = new JObject(
                    new JProperty("sticker_name", keyword.ToLower())
                ).ToString()
            };

            try
            {
                await dbContext.ConverSessions.AddAsync(converSession);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Error("Exception occured!", ex);
            }
        }

        /// <summary>
        /// Allow admin to add sticker to library.
        /// </summary>
        /// <param name="keyword">Keyword of the sticker to add.</param>
        /// <returns>Asynchronous task that host the event handler of add sticker command.</returns>
        [Command("add")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddSticker([Remainder] string keyword)
        {
            var msgRef = new MessageReference(Context.Message.Id);
            var existSticker = (from sticker in dbContext.Stickers
                                where new Regex($"{keyword}", RegexOptions.IgnoreCase).IsMatch(sticker.Keyword)
                                select sticker).FirstOrDefault();

            if (existSticker != null)
            {
                var embed = GetEmbedWithErrorTemplate("Add Sticker")
                    .AddField("Sticker Exists", $"Sticker \"{keyword}\" already exist in library.")
                    .Build();
                await ReplyAsync(embed: embed, messageReference: msgRef);
                return;
            }

            var newStickerEmbed = GetEmbedWithSuccessTemplate("Add Sticker")
                .AddField("What's the URI of the sticker?", "Reply with this message to complete the action or reply " +
                    "\"cancel\" to cancel the action.")
                .Build();
            var converStartMsg = await ReplyAsync(embed: newStickerEmbed, messageReference: msgRef);
            var msgAuthor = Context.Message.Author;
            var converSession = new ConverSession
            {
                MessageId = converStartMsg.Id,
                ChannelId = Context.Channel.Id,
                GuildId = Context.Guild.Id,
                CreatedTime = Context.Message.CreatedAt.ToUnixTimeSeconds(),
                Username = msgAuthor.Username,
                Discriminator = msgAuthor.Discriminator,
                Lifetime = 5 * 3600,
                Action = "sticker_add",
                Context = new JObject(
                    new JProperty("sticker_name", keyword.ToLower())
                ).ToString()
            };

            try
            {
                await dbContext.ConverSessions.AddAsync(converSession);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.Error("Exception occured!", e);
            }
        }

        /// <summary>
        /// Allow admin to replace a sticker with another URI while keep the keyword as same.
        /// </summary>
        /// <param name="keyword">Keyword of the sticker to remove.</param>
        /// <returns>Asynchronous task that host the execution of the command handler.</returns>
        [Command("replace")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ReplaceSticker([Remainder] string keyword)
        {
            try
            {
                var msgReference = new MessageReference(Context.Message.Id);
                var target = (from sticker in dbContext.Stickers
                              where new Regex($"{keyword}", RegexOptions.IgnoreCase).IsMatch(sticker.Keyword)
                              select sticker).FirstOrDefault();

                if (target == null)
                {
                    var errMsg = GetEmbedWithErrorTemplate("Replace Sticker")
                        .AddField("Sticker not exist", $"Couldn't found sticker \"{keyword}\"")
                        .Build();
                    await ReplyAsync(embed: errMsg, messageReference: msgReference);
                    return;
                }

                var requestNewStickerURIMsg = GetEmbedWithSuccessTemplate("Replace Sticker")
                    .AddField("What's the URI of the sticker?", "Reply with this message to complete the action or reply " +
                        "\"cancel\" to cancel the action.")
                    .Build();
                var converStartMsg = await ReplyAsync(embed: requestNewStickerURIMsg, messageReference: msgReference);

                var context = new ConverSession
                {
                    MessageId = converStartMsg.Id,
                    ChannelId = Context.Channel.Id,
                    GuildId = Context.Guild.Id,
                    CreatedTime = Context.Message.CreatedAt.ToUnixTimeSeconds(),
                    Username = Context.User.Username,
                    Discriminator = Context.User.Discriminator,
                    Lifetime = 5 * 3600,
                    Action = "sticker_replace",
                    Context = new JObject(
                        new JProperty("sticker_name", keyword.ToLower())
                    ).ToString()
                };

                try
                {
                    await dbContext.AddAsync(context);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    logger.Error("Exception occured!", ex);
                }
            }
            catch (Exception e)
            {
                logger.Error("Exception occured!", e);
            }
        }

        /// <summary>
        /// Allow admin to remove a sticker from library.
        /// </summary>
        /// <param name="keyword">Keyword of a sticker to remove.</param>
        /// <returns>Asynchronous task where this command handler running on.</returns>
        [Command("remove")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RemoveSticker([Remainder] string keyword)
        {
            keyword = keyword.ToLower();

            var sticker = (from dbSticker in dbContext.Stickers
                           where new Regex($"{keyword}", RegexOptions.IgnoreCase).IsMatch(dbSticker.Keyword)
                           select dbSticker).FirstOrDefault();
            var refMsg = new MessageReference(Context.Message.Id);

            if (sticker == null)
            {
                var embed = GetEmbedWithErrorTemplate("Remove Sticker")
                    .AddField("Sticker Not Exist", $"Sticker \"{keyword}\" did not exist in library.");
                await ReplyAsync(embed: embed.Build(), messageReference: refMsg);
                return;
            }

            try
            {
                var stickerURI = sticker.URI;
                var embed = GetEmbedWithSuccessTemplate("Remove Sticker")
                    .AddField("Sticker Removed!", $"Sticker \"{keyword}\" has been removed.")
                    .WithThumbnailUrl(stickerURI);

                dbContext.Stickers.Remove(sticker);
                await dbContext.SaveChangesAsync();
                await ReplyAsync(embed: embed.Build(), messageReference: refMsg);
            }
            catch (Exception e)
            {
                logger.Error("Exception occurred!", e);
                await ReplyWithInternalServerError(refMsg);
            }
        }
    }
}