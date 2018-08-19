namespace MaterialColor.Json
{
    using MaterialColor.Data;
    using ONI_Common.Data;
    using ONI_Common.IO;
    using ONI_Common.Json;

    public class ConfiguratorStateManager : BaseManager
    {
        public ConfiguratorStateManager(JsonManager manager, Logger logger = null)
        : base(manager, logger)
        {
        }

        public MaterialColorState LoadMaterialColorState()
        {
            return this._manager.Deserialize<MaterialColorState>(Paths.MaterialColorStatePath);
        }

        /*
        public OnionState LoadOnionState()
        {
            return this._manager.Deserialize<OnionState>(Paths.OnionStatePath);
        }
        */

        public TemperatureOverlayState LoadTemperatureState()
        {
            return this._manager.Deserialize<TemperatureOverlayState>(Paths.TemperatureStatePath);
        }

        public void SaveMaterialColorState(MaterialColorState state)
        {
            this._manager.Serialize(state, Paths.MaterialColorStatePath);
        }

        /*
        public void SaveOnionState(OnionState state)
        {
            this._manager.Serialize(state, Paths.OnionStatePath);
        }
        */

        public void SaveTemperatureState(TemperatureOverlayState state)
        {
            this._manager.Serialize(state, Paths.TemperatureStatePath);
        }
    }
}