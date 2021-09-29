/**********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *********************************************************************************************************************/
using Discord.Commands;

namespace ComputerDiscuss.DiscordAdminBot.Commands.DTOs.Stickers
{
    /// <summary>
    /// DTO that used to deliver arguments from user input to the bot.
    /// This DTO used to map sticker keyword and URI that provided by the user to the library.
    /// </summary>
    [NamedArgumentType]
    public class StickerAddOrReplaceOperationType
    {
        /// <summary>
        /// Keyword of the sticker.
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// Image URI of the sticker.
        /// </summary>
        public string URI { get; set; }
    }
}
