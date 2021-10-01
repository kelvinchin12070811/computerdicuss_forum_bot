/**********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *********************************************************************************************************************/
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace ComputerDiscuss.DiscordAdminBot.Commands
{
    /// <summary>
    /// Base class of commands handler classes. Defines utilities for handling commands.
    /// </summary>
    public abstract class CommandBase : ModuleBase
    {
        /// <summary>
        /// Create EmbedBuilder with "success" template where the border of embed is green.
        /// </summary>
        /// <param name="title">Title of the embed.</param>
        /// <returns>Constructed EmbedBuilder.</returns>
        protected EmbedBuilder GetEmbedWithSuccessTemplate(string title)
        {
            return GetDefaultTemplatedEmbedBuilder()
                .WithColor(0x00, 0xff, 0x00)
                .WithTitle(title);
        }

        /// <summary>
        /// Create EmbedBuilder with "error" template where the border of embed is red.
        /// </summary>
        /// <param name="title">Title of the embed.</param>
        /// <returns>Constructed EmbedBuilder.</returns>
        protected EmbedBuilder GetEmbedWithErrorTemplate(string title)
        {
            return GetDefaultTemplatedEmbedBuilder()
                .WithColor(0xff, 0x00, 0x00)
                .WithTitle(title);
        }

        /// <summary>
        /// Generate default template of embed message.
        /// </summary>
        /// <returns>Embed builder with default template applied.</returns>
        protected EmbedBuilder GetDefaultTemplatedEmbedBuilder()
        {
            var user = Context.Client.CurrentUser;
            return new EmbedBuilder()
                .WithCurrentTimestamp()
                .WithFooter(user.Username, user.GetAvatarUrl());
        }

        /// <summary>
        /// Reply with error if server encountered an issues.
        /// </summary>
        /// <param name="messageReference">message reference to reply.</param>
        /// <returns>Asynchronous task that execute the command handler.</returns>
        protected async Task ReplyWithInternalServerError(MessageReference messageReference = null)
        {
            var embed = GetEmbedWithErrorTemplate("Internal Server Error")
                .WithFields(
                    new EmbedFieldBuilder[]
                    {
                        new EmbedFieldBuilder()
                            .WithName("HTTP 500")
                            .WithValue("500 internal server error")
                    });
            await ReplyAsync(embed: embed.Build(), messageReference: messageReference);
        }
    }
}
