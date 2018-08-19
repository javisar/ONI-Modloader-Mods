namespace ONI_Common.Json
{
    using ONI_Common.Data;
    using ONI_Common.IO;

    public class ConfiguratorStateManager : BaseManager
    {
        public ConfiguratorStateManager(JsonManager manager, Logger logger = null)
        : base(manager, logger)
        {
        }
		
        public OnionState LoadOnionState()
        {
            return this._manager.Deserialize<OnionState>(Paths.OnionStatePath);
        }

        public void SaveOnionState(OnionState state)
        {
            this._manager.Serialize(state, Paths.OnionStatePath);
        }
		
    }
}