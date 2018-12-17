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
    }
}