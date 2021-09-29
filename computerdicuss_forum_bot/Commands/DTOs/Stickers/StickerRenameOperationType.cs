/**********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *********************************************************************************************************************/
using Discord.Commands;

namespace ComputerDiscuss.DiscordAdminBot.Commands.DTOs.Stickers
{
    /// <summary>
    /// Datatype holder that represented as dto that process rename sticker. This object provide current keyword of the
    /// sticker and the new keyword of the sticker where both provided by the user.
    /// </summary>
    [NamedArgumentType]
    public class StickerRenameOperationType
    {
        /// <summary>
        /// Current keyword of the sticker, represented as the sticker to rename.
        /// </summary>
        public string Cur
        {
            get { return _Cur; }
            set { _Cur = value.ToLower(); }
        }
        private string _Cur;
        /// <summary>
        /// New keyword of the sticker, represented as the target name where the sticker should be renamed.
        /// </summary>
        public string Next
        {
            get { return _Next; }
            set { _Next = value.ToLower(); }
        }
        private string _Next;
    }
}