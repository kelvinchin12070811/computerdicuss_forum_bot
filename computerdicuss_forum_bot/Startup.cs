using ComputerDiscussForumBot.Services;
using Discord.Commands;
using Discord.WebSocket;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerDiscussForumBot
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public ILog Logger { get; }

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

        public static async Task RunAsync(string[] args)
        {
            var startup = new Startup(args);
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
            var cancelToken = new CancellationTokenSource();

            Console.CancelKeyPress += new ConsoleCancelEventHandler((obj, ev) => {
                cancelToken.Cancel();
                ev.Cancel = true;
            });

            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<CommandHandler>();

            var startupService = provider.GetRequiredService<StartupService>();
            await startupService.StartAsync();

            try
            {
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

        private void ConfigureServices(IServiceCollection services)
        {

            services
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    MessageCacheSize = 1000
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    DefaultRunMode = RunMode.Async
                }))
                .AddSingleton<CommandHandler>()
                .AddSingleton<StartupService>()
                .AddSingleton(Configuration)
                .AddSingleton<ILog>(Logger);
        }
    }
}
