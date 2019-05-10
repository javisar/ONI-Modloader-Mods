using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace WorkableEditorMod
{
    public class WorkableEditorConfig
	{
        public bool Enabled { get; set; } = true;
		public bool Logging { get; set; } = false;

		public Dictionary<string, Dictionary<string, float>> Workables { get; set; }
				= new Dictionary<string, Dictionary<string, float>>();



		// Load Config
		private static WorkableEditorConfig _Instance;
		public static WorkableEditorConfig Instance
		{
			get
			{
				if (_Instance == null)
					_Instance = LoadConfig<WorkableEditorConfig>(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(WorkableEditorConfig)).Location), "Config/WorkableEditorConfig.json"));
				if (_Instance == null)
					_Instance = new WorkableEditorConfig();
				return _Instance;
			}
		}	

        protected static T LoadConfig<T>(string path)
        {
			Debug.Log("Loading Config from: "+path);
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
        public static BaseStateManager<NoDamageState> StateManager
                                = new BaseStateManager<NoDamageState>("NoDamage");
        */
    }
}