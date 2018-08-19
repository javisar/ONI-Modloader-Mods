using JetBrains.Annotations;

namespace ONI_Common.Json
{

    public class BaseStateManager<T>
    {
        private string stateFile;

        private ONI_Common.IO.Logger _logger;

        [NotNull]
        public ONI_Common.IO.Logger Logger => _logger ?? (_logger = new ONI_Common.IO.Logger("RoomSizeLog.txt"));


        [NotNull]
        private readonly JsonFileManager JsonLoader;

        
        private T _configuratorState;

        [NotNull]
        public T ConfiguratorState
        {
            get
            {
                if (_configuratorState != null)
                {
                    return _configuratorState;
                }

                JsonLoader.TryLoadConfiguration(this.stateFile, out _configuratorState);

                return _configuratorState;
            }

            //private set => _configuratorState = value;
            private set
            {
                _configuratorState = value;
            }
        }


        public bool TryReloadConfiguratorState()
        {
            //if (!JsonLoader.TryLoadConfiguratorState(out MaterialColorState state))
            T state;
            if (!JsonLoader.TryLoadConfiguration(this.stateFile, out state))
            {
                return false;
            }

            ConfiguratorState = state;

            return true;
        }


        public BaseStateManager(string path)
        {
            this.stateFile = path;
            JsonLoader = new JsonFileManager(new JsonManager(), Logger);
        }
    }
}
