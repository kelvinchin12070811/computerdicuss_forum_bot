/**********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *********************************************************************************************************************/
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace ComputerDiscuss.DiscordAdminBot.Commands
{
    /// <summary>
    /// Base class of commands handler classes. Defines utilities for handling commands.
    /// </summary>
    public abstract class CommandBase : ModuleBase
    {
        /// <summary>
        /// Check if the author of the context is admin of the guild where the message created at.
        /// </summary>
        /// <returns>True if the author is admin, false otherwise.</returns>
        protected bool IsAuthorAdmin()
        {
            if (Context.Message.Author is not SocketGuildUser user) return false;

            if (user.GuildPermissions.Administrator == true)
                return true;

            return false;
        }

        /// <summary>
        /// Create EmbedBuilder with "success" template where the border of embed is green.
        /// </summary>
        /// <param name="title">Title of the embed.</param>
        /// <param name="fields">Fields of the embed.</param>
        /// <returns>Constructed EmbedBuilder.</returns>
        protected EmbedBuilder GetEmbedWithSuccessTemplate(string title, EmbedFieldBuilder[] fields)
        {
            return GetDefaultTemplatedEmbedBuilder()
                .WithColor(0x00, 0xff, 0x00)
                .WithFields(fields);
        }

        /// <summary>
        /// Create EmbedBuilder with "error" template where the border of embed is red.
        /// </summary>
        /// <param name="title">Title of the embed.</param>
        /// <param name="fields">Fields of the embed.</param>
        /// <returns>Constructed EmbedBuilder.</returns>
        protected EmbedBuilder GetEmbedWithErrorTemplate(string title, EmbedFieldBuilder[] fields)
        {
            return GetDefaultTemplatedEmbedBuilder()
                .WithColor(0xff, 0x00, 0x00)
                .WithFields(fields);
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
    }
}
