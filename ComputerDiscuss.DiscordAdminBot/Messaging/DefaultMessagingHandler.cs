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
        /// <summary>
        /// Messaging Executor that handle the distribution of the jobs.
        /// </summary>
        private IMessagingExecutor messagingExecutor = new DefaultMessagingExecutor();

        /// <inheritdoc />
        public void ApplyDependencies(IServiceProvider provider)
        {
            client = provider.GetRequiredService<DiscordSocketClient>();
            logger = provider.GetRequiredService<ILog>();
            dbContext = provider.GetRequiredService<BotDBContext>();

            messagingExecutor.ApplyDependencies(provider);
        }

        /// <inheritdoc />
        public async Task<bool> Exec(SocketMessage message)
        {
            ConverSession session = null;

            ulong msgId = message.Reference == null ? message.Id : message.Reference.MessageId.Value;

            session = (from converSession in dbContext.ConverSessions.AsEnumerable()
                       where converSession.MessageId == msgId
                       orderby converSession.Id descending
                       select converSession).FirstOrDefault();

            if (session == null ||
                session.CreatedTime + (long)session.Lifetime <= DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                return false;
            }

            return await messagingExecutor.DistributeExecution(message, session);
        }
    }
}