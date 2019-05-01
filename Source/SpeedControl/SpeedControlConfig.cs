using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SpeedControl
{
    public class SpeedControlConfig
    {
        public bool Enabled { get; set; } = true;
        public bool Logging { get; set; } = false;

        public float SpeedMultiplier1 { get; set; } = 1f;
        public float SpeedMultiplier2 { get; set; } = 3f;
        public float SpeedMultiplier3 { get; set; } = 10f;



        public static SpeedControlConfig Config = null;
        public static void LoadConfig(string modPath) { Config = LoadConfig<SpeedControlConfig>(Path.Combine(modPath, "Config/SpeedControlConfig.json"));  }
        

        protected static T LoadConfig<T>(string path)
        {
            JsonSerializer serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings { Formatting = Formatting.Indented, ObjectCreationHandling = ObjectCreationHandling.Replace });
            T result;
            using (StreamReader streamReader = new StreamReader(path))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                {
                    result = serializer.Deserialize<T>(jsonReader);
                    jsonReader.Close();
                }
                streamReader.Close();
            }
            return result;            
        }

    }
}