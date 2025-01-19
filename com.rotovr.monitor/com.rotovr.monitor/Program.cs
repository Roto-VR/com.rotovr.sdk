using RotoVR.Monitor;
using System.Reflection;
using log4net.Config;
using log4net;

namespace RotoVR
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            Log.Info("|===== Start new Session =====|");         

            ApplicationConfiguration.Initialize();
            Application.Run(new MonitorForm());
        }
    }
}