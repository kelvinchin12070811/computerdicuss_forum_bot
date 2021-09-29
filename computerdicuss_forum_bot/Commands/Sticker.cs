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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerDiscuss.DiscordAdminBot.Commands
{
    /// <summary>
    /// Commands that related to sticker.
    /// </summary>
    [Group("sticker")]
    public class Sticker : CommandBase
    {
        /// <summary>
        /// Logger that use for logging.
        /// </summary>
        private readonly ILog logger;
        /// <summary>
        /// Database context that used to access data in database.
        /// </summary>
        private readonly BotDBContext dbContext;

        /// <summary>
        /// Constructor that used to perform dependencies injection.
        /// </summary>
        /// <param name="logger">Logger use for logging</param>
        /// <param name="dbContext">Database context used to access data in database.</param>
        public Sticker(ILog logger, BotDBContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
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
                            select sticker.Keyword).ToList();

            if (stickers.Count == 0)
            {
                await ReplyAsync("No stickers in library,", messageReference: refMsg);
                return;
            }

            string stickerList = string.Join(", ", stickers);
            await ReplyAsync($"Stickers avaliable:\n{stickerList}", messageReference: refMsg);
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
            stickerName = stickerName.ToLower();

            var msg = Context.Message;
            var refMsg = new MessageReference(msg.Id);
            var target = (from sticker in dbContext.Stickers.AsEnumerable()
                          where sticker.Keyword == stickerName
                          select sticker).FirstOrDefault();

            if (target == null)
            {
                await ReplyAsync($"Sticker {stickerName} does not exits in library.", messageReference: refMsg);
                return;
            }

            await ReplyAsync(target.URI, messageReference: refMsg);
        }

        /// <summary>
        /// Allow an admin to rename an existing sticker to another keyword. The new keyword must not already exist in
        /// the library.
        /// </summary>
        /// <param name="args">
        /// Parameters of the command refer <see cref="StickerRenameOperationType" /> for more info.
        /// </param>
        /// <returns></returns>
        [Command("rename")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireOwner]
        public async Task RenameSticker(StickerRenameOperationType args)
        {
            var refMsg = new MessageReference(Context.Message.Id);
            
            if ((from sticker in dbContext.Stickers.ToEnumerable()
                 where sticker.Keyword == args.Next
                 select sticker).FirstOrDefault() != null)
            {
                var embedBuilder = GetEmbedWithErrorTemplate("Rename Sticker")
                    .AddField("Error", $"There's already a sticker called \"{args.Next}\"");
                await ReplyAsync(embed: embedBuilder.Build(), messageReference: refMsg);
                return;
            }

            var curSticker = (from sticker in dbContext.Stickers.ToEnumerable()
                              where sticker.Keyword == args.Cur
                              select sticker).FirstOrDefault();

            if (curSticker == null)
            {
                var embedBuilder = GetEmbedWithErrorTemplate("Rename Sticker")
                    .AddField("Error", $"Sticker \"{args.Cur}\" did not exist in library.");
                await ReplyAsync(embed: embedBuilder.Build(), messageReference: refMsg);
                return;
            }

            try
            {
                curSticker.Keyword = args.Next;
                await dbContext.SaveChangesAsync();
                var embed = GetEmbedWithSuccessTemplate("Rename Sticker")
                    .AddField("Sticker Renamed", $"\"{args.Cur}\" has been renamed to \"{args.Next}\"")
                    .WithThumbnailUrl(curSticker.URI);
                await ReplyAsync(embed: embed.Build(), messageReference: refMsg);
            }
            catch (Exception e)
            {
                logger.Info("Exception occurred!", e);
                await ReplyWithInternalServerError(refMsg);
            }
        }

        /// <summary>
        /// Allow admin to add sticker to library.
        /// </summary>
        /// <param name="args">Required args to execute this command</param>
        /// <returns>Asynchronous task that host the event handler of add sticker command.</returns>
        [Command("add")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireOwner]
        public async Task AddSticker(StickerAddOrReplaceOperationType args)
        {
            var refMsg = new MessageReference(Context.Message.Id);

            if ((from sticker in dbContext.Stickers.ToEnumerable()
                 where sticker.Keyword == args.Keyword
                 select sticker).FirstOrDefault() != null)
            {
                var embed = GetEmbedWithErrorTemplate("Add Sticker")
                    .AddField("Sticker Exist", $"\"{args.Keyword}\" already exist in the library.");
                await ReplyAsync(embed: embed.Build(), messageReference: refMsg);
                return;
            }

            try
            {
                var sticker = new Models.Sticker(args.Keyword, args.URI);
                await dbContext.Stickers.AddAsync(sticker);
                await dbContext.SaveChangesAsync();

                var embed = GetEmbedWithSuccessTemplate("Add Sticker")
                    .AddField("Sticker", $"\"{args.Keyword}\" has been added to the library.")
                    .WithThumbnailUrl(args.URI);
                await ReplyAsync(embed: embed.Build(), messageReference: refMsg);
            }
            catch (Exception e)
            {
                logger.Error("Exception occurred!", e);
                await ReplyWithInternalServerError(refMsg);
            }
        }

        /// <summary>
        /// Allow admin to replace a sticker with another URI while keep the keyword as same.
        /// </summary>
        /// <param name="args">Args required to executed the command.</param>
        /// <returns>Asynchronous task that host the execution of the command handler.</returns>
        [Command("replace")]
        public async Task ReplaceSticker(StickerAddOrReplaceOperationType args)
        {
            var refMsg = new MessageReference(Context.Message.Id);
            var sticker = (from dbSticker in dbContext.Stickers.ToEnumerable()
                           where dbSticker.Keyword == args.Keyword
                           select dbSticker).FirstOrDefault();

            if (sticker == null)
            {
                var embed = GetEmbedWithErrorTemplate("Replace Sticker")
                    .AddField("Error", $"Sticker \"{args.Keyword}\" does not exits in library.");
                await ReplyAsync(embed: embed.Build(), messageReference: refMsg);
                return;
            }

            try
            {
                sticker.URI = args.URI;
                await dbContext.SaveChangesAsync();

                var embed = GetEmbedWithSuccessTemplate("Replace Sticker")
                    .AddField("Sticker Replaced", $"\"{sticker.Keyword}\" has been updated")
                    .WithThumbnailUrl(sticker.URI);
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
