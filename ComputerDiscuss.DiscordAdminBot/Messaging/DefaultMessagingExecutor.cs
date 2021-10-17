/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
using ComputerDiscuss.DiscordAdminBot.Models;
using Discord.WebSocket;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerDiscuss.DiscordAdminBot.Messaging
{
    /// <summary>
    /// The default messaging executor.
    /// </summary>
    class DefaultMessagingExecutor : IMessagingExecutor
    {
        /// <summary>
        /// Logger for logging.
        /// </summary>
        private ILog log = null;
        /// <summary>
        /// Discord client where the bot is logged on.
        /// </summary>
        private DiscordSocketClient client = null;
        /// <summary>
        /// Database connection of the bot.
        /// </summary>
        private BotDBContext dbContext = null;

        /// <inheritdoc/>
        public void ApplyDependencies(IServiceProvider provider)
        {
            log = provider.GetRequiredService<ILog>();
            client = provider.GetRequiredService<DiscordSocketClient>();
            dbContext = provider.GetRequiredService<BotDBContext>();
        }

        /// <inheritdoc/>
        public Task<bool> DistributeExecution(SocketMessage message, ConverSession session)
        {
            var action = session.Action;

            switch (action)
            {
                case "sticker_add":
                    _ = HandleAddSticker(message, session);
                    break;
                default:
                    return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        /// <inheritdoc/>
        public Task HandleAddSticker(SocketMessage message, ConverSession session)
        {
            var context = JObject.Parse(session.Context);
            log.Debug(session.Context);
            return Task.CompletedTask;
        }
    }
}
