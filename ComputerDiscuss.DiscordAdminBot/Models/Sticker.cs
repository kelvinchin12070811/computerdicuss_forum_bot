/**********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *********************************************************************************************************************/
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerDiscuss.DiscordAdminBot.Models
{
    /// <summary>
    /// Sticker entry, represent as a sticker stored in library.
    /// </summary>
    [Index(nameof(Keyword))]
    public class Sticker
    {
        /// <summary>
        /// Create empty sticker object, also the default constructor.
        /// </summary>
        public Sticker()
        {
        }

        /// <summary>
        /// Create a new sticker entry that will be committed to the library.
        /// </summary>
        /// <param name="keyword">Keyword of the sticker.</param>
        /// <param name="uri">URI of the sticker, must be an image's URI for this implementation.</param>
        public Sticker(string keyword, string uri)
        {
            Keyword = keyword;
            URI = uri;
        }

        /// <summary>
        /// Unique id of the sticker, used to uniquely identify the sticker in library.
        /// </summary>
        [Key]
        public ulong Id { get; set; }
        /// <summary>
        /// Keyword to trigger the sticker. Should be unique to avoid collision.
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// Resource URI to the sticker media, format must be supported by the Discord preview URL feature.
        /// List of MIME tested to work:
        ///  - image/jpg
        ///  - image/jpeg
        ///  - image/png
        ///  - image/gif
        ///  - video/mp4 (Those represented themself as gif only)
        /// </summary>
        public string URI { get; set; }
    }
}
