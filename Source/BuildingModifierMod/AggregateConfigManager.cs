using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BuildingModifierMod
{

	public class AggregateConfigManager
	{
		private String path;
		private String name;

		/*		
        public readonly string StateFilePath;

        public readonly JsonManager JsonLoader;
		*/

		public AggregateConfigManager(string path, string name)
		{
			this.path = path;
			this.name = name;
			/*			
			this.StateFilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(BuildingModifierState)).Location) + Path.DirectorySeparatorChar + "Config" + Path.DirectorySeparatorChar + name + "State.json";
			this.JsonLoader = new JsonManager();
			*/

			//IOHelper.EnsureDirectoryExists(Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(BuildingModifierState)).Location) + Path.DirectorySeparatorChar + "_Logs");
			//this.Logger = new ONI_Common.IO.Logger(Paths.GetLogsPath() + System.IO.Path.DirectorySeparatorChar + name + "Log.txt");
		}


		private BuildingModifierConfig _instance;

		public BuildingModifierConfig Instance
		{
			get
			{

				if (_instance != null) return _instance;


				/*
				Debug.Log("Loading: " + this.StateFilePath);
				if (!File.Exists(this.StateFilePath))
				{
                    Debug.Log(this.StateFilePath + " not found. Creating a default config file...");
					EnsureDirectoryExists(new FileInfo(this.StateFilePath).Directory.FullName);

					JsonLoader.TrySaveConfiguration(this.StateFilePath, (BuildingModifierState)Activator.CreateInstance(typeof(BuildingModifierState)));
				}
				JsonLoader.TryLoadConfiguration(this.StateFilePath, out _state);
				*/
				_instance = ConfigUtils<BuildingModifierConfig>.LoadConfig(this.path, this.name+".json",true);

				// Load all json configs
				string path = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(BuildingModifierConfig)).Location) + Path.DirectorySeparatorChar + this.path;

                foreach (string file in GetJsonFiles(path))
				{
					if (Path.GetFileNameWithoutExtension(file).Equals(this.name)) continue;

					try
					{
						/*
						BuildingModifierState config = JsonLoader.Deserialize<BuildingModifierState>(file);
						Debug.Log("Loading: " + file);
						*/
						BuildingModifierConfig config = ConfigUtils<BuildingModifierConfig>.LoadConfig(Path.GetDirectoryName(file),Path.GetFileName(file), true);

						//Debug.Log("config.Modifiers.Count =" + config.Modifiers.Count);
						// Append config, ignore duplicates
						/*
						_state.Modifiers = _state.Modifiers.Concat(config.Modifiers).GroupBy(d => d.Key)
									.ToDictionary(d => d.Key, d => d.First().Value);
                        */


						foreach (KeyValuePair<string, Dictionary<string, object>> entry in config.Modifiers)
						{
                            //Debug.Log("entry.Key = "+ entry.Key);
                            //Debug.Log("entry.Value = " + entry.Value);

                            if (_instance.Modifiers.ContainsKey(entry.Key))
							{
                                //Debug.Log("Contains");
                                Dictionary<string, object> currentModifier = _instance.Modifiers[entry.Key];
                                //Debug.Log("currentModifier = "+ currentModifier.Count);
                                currentModifier = currentModifier.Concat(entry.Value).GroupBy(d => d.Key)
										.ToDictionary(d => d.Key, d => d.First().Value);
                                //Debug.Log("currentModifier = " + currentModifier.Count);
                                _instance.Modifiers[entry.Key] = currentModifier;
                            }
							else
							{
								_instance.Modifiers[entry.Key] = entry.Value;
							}

                            //Debug.Log("_state.Modifiers[entry.Key].Count = " + _state.Modifiers[entry.Key].Count); 
                        }						
                        //Debug.Log("_state.Modifiers.Count ="+_state.Modifiers.Count);
                    }
					catch (Exception ex)
					{
						Debug.LogError(ex);
					}
				}
				return _instance;
			}

			private set
			{
				_instance = value;
			}
		}
	

        public static IEnumerable<string> GetJsonFiles(string path)
		{
			string[] files = null;
			Queue<string> queue = new Queue<string>();
			queue.Enqueue(path);

			while (queue.Count > 0)
			{
				path = queue.Dequeue();
				
				try
				{
					foreach (string subDir in Directory.GetDirectories(path))
					{
						if (Path.GetFileNameWithoutExtension(subDir).Equals("Examples")) continue;

						queue.Enqueue(subDir);
					}
					files = Directory.GetFiles(path);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
				}				
				if (files != null)
				{
					for (int i = 0; i < files.Length; i++)
					{
						if (!Path.GetExtension(files[i]).Equals(".json")) continue;						

						yield return files[i];
					}
				}
			}
		}


		/*
        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
		*/
    }

}
