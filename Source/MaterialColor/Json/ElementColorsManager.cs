namespace MaterialColor.Json
{
    using MaterialColor.Data;
    using ONI_Common.Data;
    using ONI_Common.IO;
    using ONI_Common.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    using Logger = ONI_Common.IO.Logger;

    public class ElementColorsManager : BaseManager
    {
        public ElementColorsManager(JsonManager manager, Logger logger = null)
        : base(manager, logger)
        {
        }

        /// <summary>
        /// Loads ElementColorInfos assoctiated with material from the configuration files
        /// </summary>
        /// <returns></returns>
        public Dictionary<SimHashes, ElementColor> LoadElementColorsDirectory(string directoryPath = null)
        {
            if (directoryPath == null)
            {
                directoryPath = Paths.ElementColorInfosDirectory;
            }

            DirectoryInfo directory = new DirectoryInfo(directoryPath);
            FileInfo[]    files     = directory.GetFiles("*.json");

            Dictionary<SimHashes, ElementColor> result = new Dictionary<SimHashes, ElementColor>();

            foreach (FileInfo file in files)
            {
                string                                  filePath = Path.Combine(directoryPath, file.Name);
                Dictionary<SimHashes, ElementColor> resultFromCurrentFile;

                try
                {
                    resultFromCurrentFile = this.LoadSingleElementColorInfosFile(filePath);
                }
                catch (Exception e)
                {
                    if (this._logger != null)
                    {
                        this._logger.Log($"Error loading {filePath} as ElementColorInfo configuration file.");
                        this._logger.Log(e);
                    }

                    continue;
                }

                foreach (KeyValuePair<SimHashes, ElementColor> entry in resultFromCurrentFile)
                {
                    if (result.ContainsKey(entry.Key))
                    {
                        result[entry.Key] = entry.Value;
                    }
                    else
                    {
                        result.Add(entry.Key, entry.Value);
                    }
                }

                this._logger?.Log($"Loaded {filePath} as ElementColorInfo configuration file.");
            }

            return result;
        }

        public Dictionary<SimHashes, ElementColor> LoadSingleElementColorInfosFile(string filePath)
        {
            return this._manager.Deserialize<Dictionary<SimHashes, ElementColor>>(filePath);
        }
    }
}