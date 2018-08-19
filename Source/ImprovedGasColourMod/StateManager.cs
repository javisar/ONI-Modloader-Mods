namespace ImprovedGasColourMod
{
    using JetBrains.Annotations;
    using ONI_Common;
    using ONI_Common.Json;
    using Logger = ONI_Common.IO.Logger;

    public static class StateManager
    {
        [NotNull]
        private static readonly JsonFileManager JsonLoader = new JsonFileManager(new JsonManager(), Logger);        

        private static Logger _logger;

        [NotNull]
        public static Logger Logger => _logger ?? (_logger = new ONI_Common.IO.Logger("ImprovedGasOverlayLog.txt"));

        private static ImprovedGasOverlayState _configuratorState;

        [NotNull]
        public static ImprovedGasOverlayState ConfiguratorState
        {
            get
            {
                if (_configuratorState != null)
                {
                    return _configuratorState;
                }

                JsonLoader.TryLoadConfiguration(Paths.ModsDirectory + System.IO.Path.DirectorySeparatorChar+ "ImprovedGasOverlay" + System.IO.Path.DirectorySeparatorChar+"Config" + System.IO.Path.DirectorySeparatorChar+ "ImprovedGasOverlayState.json", out _configuratorState);

                return _configuratorState;
            }

			//private set => _configuratorState = value;
			private set {
				_configuratorState = value;
			}
		}
        

        public static bool TryReloadConfiguratorState()
        {
			//if (!JsonLoader.TryLoadConfiguratorState(out MaterialColorState state))
			ImprovedGasOverlayState state;
			if (!JsonLoader.TryLoadConfiguration(ONI_Common.Paths.ModsDirectory + System.IO.Path.DirectorySeparatorChar + "ImprovedGasOverlay" + System.IO.Path.DirectorySeparatorChar +"Config"+ System.IO.Path.DirectorySeparatorChar +"ImprovedGasOverlayState.json",out state))
			{
				return false;
            }

            ConfiguratorState = state;

            return true;
        }
        
    }
}