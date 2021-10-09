/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
using System;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace ComputerDiscuss.DiscordAdminBot.Messaging
{
    /// <summary>
    /// Interface of the MessagingHandler.
    /// </summary>
    public interface IMessagingHandler
    {
        /// <summary>
        /// Apply dependencies to required attribute.
        /// </summary>
        /// <param name="provider">Service Provider that contain the required services.</param>
        void ApplyDependencies(IServiceProvider provider);
        /// <summary>
        /// Execute the messaging handler.
        /// </summary>
        /// <param name="message">Message to run messaging handling.</param>
        /// <returns>Task with result that indicate whether the execution success or not.</returns>
        Task<bool> Exec(SocketMessage message);
    }
}