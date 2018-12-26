

using ONI_Common;
using ONI_Common.IO;
using ONI_Common.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BuildingModifierMod
{

	public class AggregateStateManager : BaseStateManager<BuildingModifierState>
	{
		private String name;

		private BuildingModifierState _state;

		new public BuildingModifierState State
		{
			get
			{
				if (_state != null)
				{
					return _state;
				}
				Logger.Log("Loading: " + this.StateFilePath);
				
				if (!File.Exists(this.StateFilePath))
				{
					Logger.Log(this.StateFilePath + " not found. Creating a default config file...");
					IOHelper.EnsureDirectoryExists(new FileInfo(this.StateFilePath).Directory.FullName);

					JsonLoader.TrySaveConfiguration(this.StateFilePath, (BuildingModifierState)Activator.CreateInstance(typeof(BuildingModifierState)));
				}
				JsonLoader.TryLoadConfiguration(this.StateFilePath, out _state);
				
				// Load all json configs
				foreach (string file in GetJsonFiles(Paths.GetStatePath(name)))
				{
					if (Path.GetFileNameWithoutExtension(file).Equals(name+"State")) continue;

					Logger.Log("Loading: " + file);

					try
					{
						BuildingModifierState config = JsonLoader.GetJsonManager().Deserialize<BuildingModifierState>(file);
						//Debug.Log(config.Modifiers.Count);

						// Append config, ignore duplicates
						_state.Modifiers = _state.Modifiers.Concat(config.Modifiers).GroupBy(d => d.Key)
									.ToDictionary(d => d.Key, d => d.First().Value);
						//Debug.Log(_state.Modifiers.Count);
					}
					catch (Exception ex)
					{
						Debug.LogError(ex);
					}
				}
				return _state;
			}

			private set
			{
				_state = value;
			}
		}


		new public bool TryReloadConfiguratorState()
		{
			BuildingModifierState state;
			if (!JsonLoader.TryLoadConfiguration(this.StateFilePath, out state))
			{
				return false;
			}

			State = state;
			return true;
		}


		public AggregateStateManager(string name) : base(name)
		{
			this.name = name;
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

	}
}
