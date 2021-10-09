/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/

using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using log4net;
using Microsoft.Extensions.DependencyInjection;

namespace ComputerDiscuss.DiscordAdminBot.Messaging
{
    /// <summary>
    /// Default Messaging Handler that distribute tasks to different objects.
    /// </summary>
    public class DefaultMessagingHandler : IMessagingHandler
    {
        /// <summary>
        /// Client where the bot is running on.
        /// </summary>
        private DiscordSocketClient client = null;
        /// <summary>
        /// Logger for logging.
        /// </summary>
        private ILog logger = null;

        /// <inheritdoc />
        public void ApplyDependencies(IServiceProvider provider)
        {
            client = provider.GetRequiredService<DiscordSocketClient>();
            logger = provider.GetRequiredService<ILog>();
        }

        /// <inheritdoc />
        public Task<bool> Exec(SocketMessage message)
        {
            var content = message.Content.Trim();

            switch (content)
            {
                case "%end%":
                    logger.Info("End session request received!");
                    return Task.FromResult(true);

                default:
                    return Task.FromResult(false);
            }
        }
    }
}