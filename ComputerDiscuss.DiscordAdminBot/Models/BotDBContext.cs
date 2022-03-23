/**********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *********************************************************************************************************************/
using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace ComputerDiscuss.DiscordAdminBot.Models
{
    /// <summary>
    /// Represent as the class that communicate with the database.
    /// </summary>
    public class BotDBContext : DbContext
    {
        /// <summary>
        /// Stickers data.
        /// </summary>
        public DbSet<Sticker> Stickers { get; set; }
        /// <summary>
        /// Conversations data.
        /// </summary>
        public DbSet<ConverSession> ConverSessions { get; set; }

        /// <summary>
        /// Path to the SQLite3 database file.
        /// </summary>
        public string DbPath { get; private set; }

        private readonly ILog logger;

        /// <summary>
        /// Initialize database and perform dependencies injection.
        /// </summary>

        public BotDBContext()
        {
            init();
        }

        public BotDBContext(ILog logger)
        {
            this.logger = logger;
            init();
        }

        private void init()
        {
            var path = AppContext.BaseDirectory;
            if (logger != null)
                logger.Info($"Initializing SQLite3 database at: {path}");
            DbPath = Path.Join(new string[] { path, "application.db" });
        }

        /// <summary>
        /// Initialize database.
        /// </summary>
        /// <param name="optionsBuilder">Builder that build the options to connect database.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (logger != null)
                logger.Info("Connecting database...");
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }
}
