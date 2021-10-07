/***********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 **********************************************************************************************************************/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerDiscuss.DiscordAdminBot.Models
{
    /// <summary>
    /// Tuple that record all conversation that started by the bot.
    /// </summary>
    public class ConverSession
    {
        /// <summary>
        /// Unique Identifier of the conversation object.
        /// </summary>
        [Key]
        public long Id { get; set; }
        /// <summary>
        /// Message that the conversation pointed to. Marked as the message that started this conversation.
        /// </summary>
        public long MessageId { get; set; }
        /// <summary>
        /// Channel where the conversation starting message lived on.
        /// </summary>
        public long ChannelId { get; set; }
        /// <summary>
        /// Discord server where the conversation starting message lived on.
        /// </summary>
        public long GuildId { get; set; }
        /// <summary>
        /// Timestamp that represent as when the conversation is started, usually same as the message creation time.
        /// </summary>
        public long CreatedTime { get; set; }
        /// <summary>
        /// Lifetime in seconds to define the duration of the conversation to keep on.
        /// </summary>
        public long Lifetime { get; set; }
        /// <summary>
        /// JSON data of the session, keeps all required or related data for the session.
        /// </summary>
        public string Context { get; set; }
    }
}