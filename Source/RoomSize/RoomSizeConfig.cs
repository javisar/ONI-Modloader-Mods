using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RoomSize
{
    public class RoomSizeConfig
    {
        public bool Enabled { get; set; } = true;

        public int OverallMaximumRoomSize { get; set; } = 512;

        public Dictionary<string, int> MaximumRoomSizes { get; set; } = new Dictionary<string,int>();



        public static RoomSizeConfig Config = null;
        public static void LoadConfig(string modPath) { Config = LoadConfig<RoomSizeConfig>(Path.Combine(modPath,"Config/RoomSizeConfig.json"));  }
        

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

        /*
		public static BaseStateManager<RoomSizeState> StateManager
			= new BaseStateManager<RoomSizeState>("RoomSize");
        */
    }
}