/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/

using System;
using System.Linq;
using System.Threading.Tasks;
using ComputerDiscuss.DiscordAdminBot.Models;
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
        /// <summary>
        /// Database API for bot.
        /// </summary>
        private BotDBContext dbContext = null;

        /// <inheritdoc />
        public void ApplyDependencies(IServiceProvider provider)
        {
            client = provider.GetRequiredService<DiscordSocketClient>();
            logger = provider.GetRequiredService<ILog>();
            dbContext = provider.GetRequiredService<BotDBContext>();
        }

        /// <inheritdoc />
        public Task<bool> Exec(SocketMessage message)
        {
            var content = message.Content.Trim();
            ConverSession session = null;

            session = (from converSession in dbContext.ConverSessions.AsEnumerable()
                       where converSession.MessageId == message.Id
                       select converSession).FirstOrDefault();

            if (session != null)
            {
            }

            return Task.FromResult(false);
        }
    }
}