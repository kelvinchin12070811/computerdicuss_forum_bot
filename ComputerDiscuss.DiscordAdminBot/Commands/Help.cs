/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
using Discord;
using Discord.Commands;
using log4net;
using System;
using System.Threading.Tasks;

namespace ComputerDiscuss.DiscordAdminBot.Commands
{
    /// <summary>
    /// A group of commands that tell user what the bot can do and how to call them.
    /// </summary>
    public class Help : CommandBase
    {
        /// <summary>
        /// Logger instance that used for logging.
        /// </summary>
        private readonly ILog logger;

        /// <summary>
        /// Constructor that use to perform dependencies injection.
        /// </summary>
        /// <param name="logger">Logger instance that use for logging.</param>
        public Help(ILog logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Get help of the selected category, use without parameter to get a list of categories.
        /// </summary>
        /// <param name="section">Category of commands to list. Leave it blank to list all avaliable categories.</param>
        /// <returns>Asynchornous task where the command handler running</returns>
        [Command("help")]
        public async Task GetHelp(string category = null)
        {
            var refMsg = new MessageReference(Context.Message.Id);
            var botOnServer = await Context.Guild.GetUserAsync(Context.Client.CurrentUser.Id);
            var botUsername = botOnServer.Nickname ?? botOnServer.Username;

            try
            {
                switch (category)
                {
                    case "general":
                        await GetGeneralHelp(refMsg, botUsername);
                        break;

                    case "sticker":
                        await GetStickersHelp(refMsg, botUsername);
                        break;

                    default:
                        await ListCategories(refMsg, botUsername);
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Error("Exception occurred!", e);
                await ReplyWithInternalServerError(refMsg);
            }
        }

        /// <summary>
        /// Get help of all available categories.
        /// </summary>
        /// <param name="refMsg">Message to reply.</param>
        /// <param name="botUsername">Username of the bot, might be bot's nickname if it has one.</parma>
        /// <returns>Asynchronous task where methods running on.</returns>
        private async Task ListCategories(MessageReference refMsg, string botUsername)
        {
            var embed = GetEmbedWithSuccessTemplate("Get Help")
                .AddField($"@{botUsername} help", "Show this message")
                .AddField($"@{botUsername} help general", "Show general commands")
                .AddField($"@{botUsername} help sticker", "Get help on stickers related commands");
            await ReplyAsync(embed: embed.Build(), messageReference: refMsg);
        }

        /// <summary>
        /// Get help of all general commands.
        /// </summary>
        /// <param name="refMsg">Message to reply</param>
        /// <param name="botUsername">Username of the bot, might be bot's nickname if it has one.</param>
        /// <returns>Asynchronous task where methods running on.</returns>
        private async Task GetGeneralHelp(MessageReference refMsg, string botUsername)
        {
            var embed = GetEmbedWithSuccessTemplate("Get Help - General")
                .AddField($"@{botUsername} ping", "Get the ping between the bot and your client, also between the "
                    + "connection of bot and Discord server")
                .AddField($"@{botUsername} game {{text}}", "Set the \"Playing\" status of the bot, only admin can "
                    + "access this command")
                .AddField($"@{botUsername} issues", "Get the location of the bot's issues tracker.");
            await ReplyAsync(embed: embed.Build(), messageReference: refMsg);
        }

        /// <summary>
        /// Get help of all sticker related commands.
        /// </summary>
        /// <param name="refMsg">Message to reply</param>
        /// <param name="botUsername">Username of the bot, might be bot's nickname if it has one.</param>
        /// <returns>Asynchronous task where methods running on.</returns>
        private async Task GetStickersHelp(MessageReference refMsg, string botUsername)
        {
            var embed = GetEmbedWithSuccessTemplate("Get Help - Sticker")
                .AddField($"@{botUsername} sticker send {{keyword}}",
                    "Send a sticker to the chat where the command is executed, where the {keyword} is the name of "
                        + $"sticker.")
                .AddField($"@{botUsername} sticker reply {{keyword}}", "Same as sticker send, but it send sticker by "
                    + "replying to a message where the command replied to.")
                .AddField($"@{botUsername} sticker preview {{keyword}}", "Also send sticker to channel, but it will "
                    + "be deleted after certain amount of time.")
                .AddField($"@{botUsername} sticker add keyword: {{keyword}} uri: {{url}}",
                    "Allow admin to add a sticker to library, where keyword unique id of the sticker and uri is url to "
                        + "the sticker's image or gif. Keyword that contain space must be enclosed with \"\"")
                .AddField($"@{botUsername} sticker rename cur: {{current keyword}} next: {{new keyword}}",
                    "Allow admin to rename a sticker's keyword to another, where cur is the current keyword and next "
                        + "is new unique keyword to the sticker. String that contain space must be enclosed with \"\"")
                .AddField($"@{botUsername} sticker replace keyword: {{keyword}} uri: {{uri}}",
                    "Allow admin to replace a sticker's URI to another, where keyword is the sticker to replace and "
                        + "uri is the new URL of the image or gif. String that contain space must be enclosed "
                        + "with \"\"")
                .AddField($"@{botUsername} sticker remove {{keyword}}",
                    "Allow admin to remove a sticker from library, where keyword is the unique id of sticker to "
                        + "remove.");
            await ReplyAsync(embed: embed.Build(), messageReference: refMsg);
        }
    }
}