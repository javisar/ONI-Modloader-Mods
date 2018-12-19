using System;
using System.IO;
using UnityEngine;

namespace FluidWarpMod
{
    static class Logger
    {
        internal sealed class FileLogHandler : ILogHandler, IDisposable
        {
            private FileStream fileStream;
            private StreamWriter streamWriter;

            public FileLogHandler(string LogFileName)
            {
                fileStream = new FileStream(LogFileName, FileMode.Create, FileAccess.ReadWrite);
                streamWriter = new StreamWriter(fileStream);
            }

            public void Dispose()
            {
                fileStream.Dispose();
            }

            public void LogException(Exception exception, UnityEngine.Object context)
            {
                streamWriter.WriteLine("Exception: {0}", exception.Message);
                streamWriter.WriteLine("Stacktrace: {0}", exception.StackTrace);
                streamWriter.Flush();
            }

            public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
            {
                streamWriter.WriteLine(format, args);
                streamWriter.Flush();
            }
        }
#if DEBUG
        static UnityEngine.Logger u_logger = new UnityEngine.Logger(new FileLogHandler("Mods" + Path.DirectorySeparatorChar + "_Logs" + Path.DirectorySeparatorChar + "FluidWarpMod.txt"));
#endif
        
        public static void Log(string message)
        {
#if DEBUG
            u_logger.Log(LogType.Log, message);
#endif
        }

        public static void LogFormat(string template, params object[] args)
        {
#if DEBUG
            u_logger.LogFormat(LogType.Log, template, args);
#endif
        }

        static Logger()
        {
#if DEBUG
            u_logger.logEnabled = true;
#endif
        }

    }
}
