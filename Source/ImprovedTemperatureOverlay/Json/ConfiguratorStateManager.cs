namespace ImprovedTemperatureOverlay.Json
{
    //using MaterialColor.Data;
    using ONI_Common.Data;
    using ONI_Common.IO;
    using ONI_Common.Json;

    public class ConfiguratorStateManager
    {
		protected Logger _logger;

		protected JsonManager _manager;

		public ConfiguratorStateManager(JsonManager manager, Logger logger = null)
        {
			this._logger = logger;
			this._manager = manager;
		}
		

        public ImprovedTemperatureOverlayState LoadTemperatureState()
        {
            return this._manager.Deserialize<ImprovedTemperatureOverlayState>(Paths.TemperatureStatePath);
        }
        

        public void SaveTemperatureState(ImprovedTemperatureOverlayState state)
        {
            this._manager.Serialize(state, Paths.TemperatureStatePath);
        }
    }
}