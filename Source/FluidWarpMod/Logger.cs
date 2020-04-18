using System;
using System.IO;
using UnityEngine;

namespace FluidWarpMod
{
    static class Logger
    {
#if DEBUG    
        static UnityEngine.Logger u_logger;
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
            String logPath = "Mods" + Path.DirectorySeparatorChar + "_Logs" + Path.DirectorySeparatorChar + "FluidWarpMod.txt";
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    u_logger = new UnityEngine.Logger(UnityEngine.Debug.unityLogger.logHandler);
                }
                catch (DirectoryNotFoundException ex)
                {
                    Debug.LogError(ex.ToString() + $"\nUnity logger is not started since the direcrtory {logPath} doesn't exist");
                    Directory.CreateDirectory(Path.GetDirectoryName(logPath));
                }
            }
            u_logger.logEnabled = true;
#endif
        }

    }
}
