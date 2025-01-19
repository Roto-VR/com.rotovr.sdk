using log4net;

namespace RotoVR
{
    static class Log
    {
        static readonly ILog log = LogManager.GetLogger(typeof(Program));


        public static void Info(string message) {
            log.Info(message);
        }

        public static void Error(string message)
        {
            log.Error(message);
        }

        public static void Debug(string message)
        {
            log.Debug(message);
        }
    }
}
