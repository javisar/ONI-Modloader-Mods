namespace MaterialColor
{
    using JetBrains.Annotations;
    using MaterialColor.Data;
    using MaterialColor.Json;
    using ONI_Common.Data;
    using ONI_Common.Json;
    using System.Collections.Generic;
    using UnityEngine;
    using Logger = ONI_Common.IO.Logger;

    public static class State
    {
        [NotNull]
        private static readonly JsonFileLoader JsonLoader = new JsonFileLoader(new JsonManager(), Logger);

        // TODO: load from file instead
        [NotNull]
        public static readonly List<string> TileNames = new List<string>
        {
            "Tile",
            "MeshTile",
            "GlassTile",
            "BunkerTile",
            "InsulationTile",
            "GasPermeableMembrane",
            "TilePOI",
            "PlasticTile",
            "MetalTile"
        };

        private static MaterialColorState _configuratorState;

        private static Dictionary<SimHashes, Color> _materialColors;

        private static Logger _logger;

        [NotNull]
        public static MaterialColorState ConfiguratorState
        {
            get
            {
                if (_configuratorState != null)
                {
                    return _configuratorState;
                }

                JsonLoader.TryLoadConfiguratorState(out _configuratorState);

                return _configuratorState;
            }

            private set
            {
                _configuratorState = value;
			}
		}

        [NotNull]
        public static Dictionary<SimHashes, Color> ElementColorInfos
        {
            get
            {
                if (_materialColors != null)
                {
                    return _materialColors;
                }

                JsonLoader.TryLoadElementColorInfos(out _materialColors);

                return _materialColors;
            }

			private set
			{
				_materialColors = value;
			}
		}

        [NotNull]
        public static Logger Logger => _logger ?? (_logger = new ONI_Common.IO.Logger(Paths.ModsDirectory+ System.IO.Path.DirectorySeparatorChar + "_Logs" + System.IO.Path.DirectorySeparatorChar + Paths.MaterialColorLogFileName));

        public static bool TryReloadConfiguratorState()
        {
            if (!JsonLoader.TryLoadConfiguratorState(out MaterialColorState state))
            {
                return false;
            }

            ConfiguratorState = state;

            return true;
        }

        public static bool TryReloadElementColorInfos()
        {
            if (!JsonLoader.TryLoadElementColorInfos(out Dictionary<SimHashes, Color> colorInfos))
            {
                return false;
            }

            ElementColorInfos = colorInfos;

            return true;
        }
    }
}