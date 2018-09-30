namespace ONI_Common.OnionHooks
{
    using ONI_Common.Data;
    using ONI_Common.IO;
    using ONI_Common.Json;
    using System;
    using System.IO;

    public static class Hooks
    {
        private static readonly Logger _logger = new IO.Logger(Paths.GetLogFileName("ONI-Common"));

        private static OnionState _config;

        //private static ConfiguratorStateManager _stateManager;
        public static BaseStateManager<OnionState> ONICommonState
               = new BaseStateManager<OnionState>("ONI-Common");

        public static OnionState Config
        {
            get
            {
                if (_config != null)
                {
                    return _config;
                }

                try
                {
                    //_stateManager = _stateManager ?? new ConfiguratorStateManager(new JsonManager());

                    TryLoadConfig();
                    StartConfigFileWatcher();
                }
                catch (Exception e)
                {
                    _logger.Log("State load/init failed");
                    _logger.Log(e);
                    _config = new OnionState();
                }

                return _config;
            }

            private set
            {
                _config = value;
            }
        }
        /*
        public static void OnDebugHandlerCtor()
        {
            try
            {
                UpdateDebugHandler();
            }
            catch (Exception e)
            {
                _logger.Log("On debug handler constructor failed");
                _logger.Log(e);
            }
        }
        
        public static void OnDoOfflineWorldGen()
        {
            if (Config.Enabled && Config.CustomWorldSize)
            {
                try
                {
                    ResetGridSettingsChunks(Config.Width, Config.Height);

                    _logger.Log("Custom world dimensions applied");
                }
                catch (Exception e)
                {
                    _logger.Log(e);
                    _logger.Log("On do offline world gen failed");
                }
            }
            else
            {
                OnionState defaultConfig = new OnionState();

                ResetGridSettingsChunks(defaultConfig.Width, defaultConfig.Height);
            }
        }
        

        public static void OnInitRandom(ref int worldSeed, ref int layoutSeed, ref int terrainSeed, ref int noiseSeed)
        {
            if (!Config.Enabled || !Config.CustomSeeds)
            {
                return;
            }

            worldSeed   = Config.WorldSeed   >= 0 ? Config.WorldSeed : worldSeed;
            layoutSeed  = Config.LayoutSeed  >= 0 ? Config.LayoutSeed : layoutSeed;
            terrainSeed = Config.TerrainSeed >= 0 ? Config.TerrainSeed : terrainSeed;
            noiseSeed   = Config.NoiseSeed   >= 0 ? Config.NoiseSeed : noiseSeed;

            if (!Config.LogSeed)
            {
                return;
            }

            string message = string.Format(
                                           $"Size: {Config.Width}x{Config.Height} World Seed: {worldSeed} Layout Seed: {layoutSeed}"
                                         + $"Terrain Seed: {terrainSeed} Noise Seed: {noiseSeed}");

            _logger.Log(message);
        }
        */

        /*
        public static void UpdateDebugHandler()
        {
#if !Patch

            // DebugHandler.enabled = Config.Enabled && Config.Debug;
#endif

            // DebugHandler.FreeCameraMode = Config.Enabled && Config.FreeCamera;
        }
        */

        private static void OnConfigChanged(object sender, FileSystemEventArgs e)
        {
            _logger.Log("Config changed");

            TryLoadConfig();
            //UpdateDebugHandler();
        }

        /*
        private static void ResetGridSettingsChunks(int width, int height)
        {
            GridSettings.Reset(width * 32, height * 32);
        }
        */

        private static void StartConfigFileWatcher()
        {
            FileChangeNotifier.StartFileWatch(Paths.GetStateFileName("ONI-Common"), Paths.GetStatePath("ONI-Common"), OnConfigChanged);

            _logger.Log("Config file watcher started");
        }

        private static bool TryLoadConfig()
        {
            try
            {
                //Config = _stateManager.LoadOnionState();
                Config = ONICommonState.State;

                _logger.Log(Paths.GetStateFileName("ONI-Common")+" loaded");

                return true;
            }
            catch
            {
                Config = new OnionState();

                string Message = Paths.GetStateFileName("ONI-Common")+" loading failed";

                _logger.Log(Message);
                Debug.LogError(Message);

                return false;
            }
        }
    }
}