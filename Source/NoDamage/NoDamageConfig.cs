
using Newtonsoft.Json;
using System;
using System.IO;

namespace NoDamageMod
{
    public class NoDamageConfig
    {
        public bool Enabled { get; set; } = true;

        public bool DisableAllDamage { get; set; } = true;

        public bool NoBuildingDamage { get; set; } = true;

        public bool NoCircuitOverload { get; set; } = true;
        public bool NoBuildingOverheat { get; set; } = true;
        public bool NoConduitContentsBoiling { get; set; } = true;
        public bool NoConduitContentsFrozen { get; set; } = true;




        public static NoDamageConfig Config = null;
        public static void LoadConfig(string modPath) { Config = LoadConfig<NoDamageConfig>(Path.Combine(modPath, "Config/NoDamageConfig.json")); }


        protected static T LoadConfig<T>(string path)
        {
			Debug.Log("Loading Config from: " + path);
			try
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
			catch (Exception ex)
			{
				Debug.LogError(ex);
				return (T)Activator.CreateInstance(typeof(T));
			}
		}
        /*
        public static BaseStateManager<NoDamageState> StateManager
                                = new BaseStateManager<NoDamageState>("NoDamage");
        */
    }
}