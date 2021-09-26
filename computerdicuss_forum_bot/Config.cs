using System;
using System.IO;
using YamlDotNet.Serialization;

namespace ComputerDiscussForumBot
{
    /// <summary>
    /// Configuration of the bot.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Token of the bot use to log into Discord.
        /// </summary>
        public string Token { get; private set; }
        /// <summary>
        /// Client ID that represent as the application.
        /// </summary>
        public string ClientID { get; private set; }
        /// <summary>
        /// Determine the time in milliseconds where an auto dispose message to delete.
        /// </summary>
        public long AutoDisposeDuration { get; private set; }

        private static readonly Config instance = new Config();

        /// <summary>
        /// Get the singleton instance of the Config object.
        /// </summary>
        public static Config Instance { get { return instance; } }

        /// <summary>
        /// Initialize the Config object.
        /// </summary>
        public void Init()
        {
            Log.Instance.Logger.Info("Reading configuration...");
            try
            {
                var deserializer = new DeserializerBuilder().Build();
                using var fConfig = new StreamReader("./config.yaml");
                var docConfig = deserializer.Deserialize<dynamic>(fConfig);
                Token = docConfig["token"];
                ClientID = docConfig["client id"];
                AutoDisposeDuration = long.Parse(docConfig["auto dispose duration"]);
            }
            catch (Exception e)
            {
                Log.Instance.Logger.Error("Critical error occured! ", e);
                Environment.Exit(1);
            }
            Log.Instance.Logger.Info("Configuration loaded successfully.");
        }
    }
}
