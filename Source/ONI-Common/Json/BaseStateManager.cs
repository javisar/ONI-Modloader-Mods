

namespace ONI_Common.Json
{

    public class BaseStateManager<T>
    {
		public readonly string StateFilePath;

		public readonly ONI_Common.IO.Logger Logger;

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
            this.StateFilePath = ONI_Common.Paths.GetStateFilePath(name);
			this.Logger = new ONI_Common.IO.Logger(name+"Log.txt");
			this.JsonLoader = new JsonFileManager(new JsonManager(), Logger);
        }
    }
}
