using System;
using System.IO;

namespace BuildingModifierMod
{

    public class BaseStateManager<T>
    {
		public readonly string StateFilePath;

		//public readonly ONI_Common.IO.Logger Logger;

		public readonly JsonFileManager JsonLoader;


		private T _state;

        public T State
        {
            get
            {
                if (_state != null)
                {
                    return _state;
                }
                Debug.Log("Loading: " +this.StateFilePath);

                if (!File.Exists(this.StateFilePath))
                {
                    Debug.Log(this.StateFilePath+" not found. Creating a default config file...");
                    IOHelper.EnsureDirectoryExists(new FileInfo(this.StateFilePath).Directory.FullName);
                   
                    JsonLoader.TrySaveConfiguration(this.StateFilePath, (T)Activator.CreateInstance(typeof(T)));
                }
                JsonLoader.TryLoadConfiguration(this.StateFilePath, out _state);
                return _state;
            }

            //private set => _configuratorState = value;
            private set
            {
				_state = value;
            }
        }


        public bool TryReloadConfiguratorState()
        {
            //if (!JsonLoader.TryLoadConfiguratorState(out MaterialColorState state))
            T state;
            if (!JsonLoader.TryLoadConfiguration(this.StateFilePath, out state))
            {
                return false;
            }

            State = state;

            return true;
        }


        public BaseStateManager(string name)
        {
            this.StateFilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(BuildingModifierState)).Location) + Path.DirectorySeparatorChar + "Config" + Path.DirectorySeparatorChar + name + "State.json";
            //IOHelper.EnsureDirectoryExists(Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(BuildingModifierState)).Location) + Path.DirectorySeparatorChar + "_Logs");
            //this.Logger = new ONI_Common.IO.Logger(Paths.GetLogsPath() + System.IO.Path.DirectorySeparatorChar + name + "Log.txt");
			this.JsonLoader = new JsonFileManager(new JsonManager());
        }
        
        /*
        public BaseStateManager(string configFileName, string logFilePath)
        {
            this.StateFilePath = configFileName;
			//this.Logger = new ONI_Common.IO.Logger(logFilePath);
			this.JsonLoader = new JsonFileManager(new JsonManager());
        }
        */
    }
}
