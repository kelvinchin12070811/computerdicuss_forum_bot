/**********************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *********************************************************************************************************************/
using ComputerDiscuss.DiscordAdminBot.Messaging;
using ComputerDiscuss.DiscordAdminBot.Models;
using ComputerDiscuss.DiscordAdminBot.Services;
using Discord.Commands;
using Discord.WebSocket;
using log4net;
using log4net.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerDiscuss.DiscordAdminBot
{
    /// <summary>
    /// Main object that start everything up
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration object, object that manage the configuration of the bot.
        /// Read only.
        /// </summary>
        public IConfigurationRoot Configuration { get; }
        /// <summary>
        /// Logger object that perform logging.
        /// Read only.
        /// </summary>
        public ILog Logger { get; }

        /// <summary>
        /// Constructor that initialize the Startup instance.
        /// </summary>
        /// <param name="args">Command line arguments passed to the application while startup.</param>
        public Startup(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("./log4net.config"));
            Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            Logger.Info("Starting bot...");

            var baseDirectory = AppContext.BaseDirectory;
            Logger.Info($"Base directory at {baseDirectory}.");
            Logger.Info("Building configurations...");

            var builder = new ConfigurationBuilder()
                .SetBasePath(baseDirectory)
                .AddYamlFile("config.yaml");

            Configuration = builder.Build();
        }

        /// <summary>
        /// Main entry point called to start the bot.
        /// </summary>
        /// <param name="args">Command line arguments passed to the application while startup.</param>
        /// <returns>Asynchronous task that represent the operation that kick start the bot.</returns>
        public static async Task RunAsync(string[] args)
        {
            var startup = new Startup(args);
            await startup.RunAsync();
        }

        /// <summary>
        /// Initialize required services, event handlers and invoke the bot, also handle shutdown once Ctrl + C is
        /// pressed.
        /// </summary>
        /// <returns>Asynchronous task that represent the operation that kick start the bot.</returns>
        public async Task RunAsync()
        {
            var cancelToken = new CancellationTokenSource();

            Console.CancelKeyPress += new ConsoleCancelEventHandler((obj, ev) =>
            {
                cancelToken.Cancel();
                ev.Cancel = true;
            });

            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<IMessagingHandler>().ApplyDependencies(provider);
            provider.GetRequiredService<CommandHandler>();

            var startupService = provider.GetRequiredService<StartupService>();
            var dbContextService = provider.GetRequiredService<BotDBContext>();

            try
            {
                Logger.Info("Migrating database...");
                dbContextService.Database.Migrate();
                Logger.Info("Database migrated!");

                await startupService.StartAsync();
                await Task.Delay(-1, cancelToken.Token);
            }
            catch (TaskCanceledException)
            {
                Logger.Info("Interupt accepted, shutting down...");
                await startupService.StopAsync();
            }
            catch (Exception e)
            {
                Logger.Fatal("Critical error occurred!", e);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Configure and register required services to start the bot, also perform dependencies injection to added
        /// modules.
        /// </summary>
        /// <param name="services">ServiceCollection object to configure.</param>
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 1000
            }));
            services.AddSingleton(new CommandService(new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false
            }));
            services.AddSingleton<CommandHandler>();
            services.AddSingleton<StartupService>();
            services.AddSingleton(Configuration);
            services.AddSingleton<ILog>(Logger);
            services.AddDbContext<BotDBContext>();
            services.AddSingleton<IMessagingHandler>(new DefaultMessagingHandler());
        }
    }
}
