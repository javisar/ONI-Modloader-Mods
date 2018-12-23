namespace MaterialColor
{
    using JetBrains.Annotations;
    using MaterialColor.Data;
    using MaterialColor.IO;
    using MaterialColor.IO.Json;
    using ONI_Common.Json;
    using System;
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

        private static Dictionary<SimHashes, ElementColor> _materialColors;

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

                MaterialColorState state;
                JsonLoader.TryLoadConfiguratorState(out state);

                ConfiguratorState = state;

                return state;
            }

            private set
            {
                _configuratorState = value;

                try
                {
                    TypeFilter = new TextFilter(_configuratorState.TypeFilterInfo);
                }
                catch (Exception e)
                {
                    State.Logger.Log("Error while creating new TextFilter object");
                    State.Logger.Log(e);
                }
			}
		}

        public static TextFilter TypeFilter { get; set; }

        [NotNull]
        public static Dictionary<SimHashes, ElementColor> ElementColors
        {
            get
            {
                if (_materialColors != null)
                {
                    return _materialColors;
                }

                JsonLoader.TryLoadElementColors(out _materialColors);

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
            if (!JsonLoader.TryLoadElementColors(out Dictionary<SimHashes, ElementColor> colorInfos))
            {
                return false;
            }

            ElementColors = colorInfos;

            return true;
        }
    }
}