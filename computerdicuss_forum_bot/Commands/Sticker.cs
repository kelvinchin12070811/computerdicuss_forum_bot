using ComputerDiscuss.DiscordAdminBot.Models;
using Discord;
using Discord.Commands;
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
    public class Sticker : ModuleBase
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

        [Command("send")]
        public async Task SendSticker([Remainder]string stickerName)
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
    }
}
