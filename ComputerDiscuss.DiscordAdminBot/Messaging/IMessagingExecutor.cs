/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
using ComputerDiscuss.DiscordAdminBot.Models;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace ComputerDiscuss.DiscordAdminBot.Messaging
{
    /// <summary>
    /// Interface that handle different logic based on different conditions.
    /// </summary>
    interface IMessagingExecutor
    {
        /// <summary>
        /// Apply dependencies of the object from DependencyInjection framework.
        /// </summary>
        /// <param name="provider">Services provider that provide required services.</param>
        public void ApplyDependencies(IServiceProvider provider);
        /// <summary>
        /// Close the session of the conversation.
        /// </summary>
        /// <param name="session">Session to close</param>
        /// <returns>Asynchornous task that handle the logic of closing the conversation session.</returns>
        public Task CloseSession(ConverSession session);
        /// <summary>
        /// Distribute message to different executor based on the content of the message.
        /// </summary>
        /// <param name="message">Message object that will trigger the action.</param>
        /// <param name="session">Session of the conversation.</param>
        /// <returns>True if jobs distributed successfully or false if failed.</returns>
        public Task<bool> DistributeExecution(SocketMessage message, ConverSession session);
        /// <summary>
        /// Method that handle logic for add sticker.
        /// </summary>
        /// <param name="message">Message object that triggered the action.</param>
        /// <param name="session">Session of the conversation.</param>
        public Task HandleAddSticker(SocketMessage message, ConverSession session);
        /// <summary>
        /// Method that handle logic for rename sticker.
        /// </summary>
        /// <param name="message">Message object that trriggered the action.</param>
        /// <param name="session">Session of the conversation.</param>
        /// <returns>Asynchornous task that perform the rename sticker task.</returns>
        public Task HandleRenameSticker(SocketMessage message, ConverSession session);
        /// <summary>
        /// Get the default template of EmbedBuilder.
        /// </summary>
        /// <param name="title">Title of the embed.</param>
        /// <param name="user">User who sent this message.</param>
        /// <returns>EmbedBuilder with the default styles.</returns>
        public EmbedBuilder GetDefaultEmbedBuilder(string title, SocketUser user);
        /// <summary>
        /// Get the default template of the EmbedBuilder that indicated as success.
        /// </summary>
        /// <param name="title">Title of the embed message.</param>
        /// <param name="user">User who sent this message.</param>
        /// <returns>EmbedBuilder with the default success template.</returns>
        public EmbedBuilder GetDefaultSuccessEmbedBuilder(string title, SocketUser user);
        /// <summary>
        /// Get the default template of the EmbedBuilder that indicated as error.
        /// </summary>
        /// <param name="title">Title of teh embed message.</param>
        /// <param name="user">User who sent this message.</param>
        /// <returns>EmbedBuilder with the default error template.</returns>
        public EmbedBuilder GetDefaultErrorEmbedBuilder(string title, SocketUser user);
    }
}
