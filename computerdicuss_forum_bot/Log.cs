using System;
using System.Reflection;
using System.IO;
using log4net;
using log4net.Config;

namespace ComputerDiscussForumBot
{
    /// <summary>
    /// Singleton instance that handling logging.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Logger instance that use to output log.
        /// </summary>
        public ILog Logger { get; private set; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the unique instance of Log object.
        /// </summary>
        public static Log Instance { get { return instance; } }

        private static readonly Log instance = new Log();

        /// <summary>
        /// Initialize the global Log instance.
        /// </summary>
        public void Init()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("./log4net.config"));
        }
    }
}
