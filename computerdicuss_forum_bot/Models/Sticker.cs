/**********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *********************************************************************************************************************/
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerDiscuss.DiscordAdminBot.Models
{
    public class Sticker
    {
        public Sticker()
        {
        }

        public Sticker(string keyword, string uri)
        {
            Keyword = keyword;
            URI = uri;
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string URI { get; set; }
    }
}
