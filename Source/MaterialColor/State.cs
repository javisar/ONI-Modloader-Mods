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

        [NotNull]
        public static readonly List<Color32> DefaultTemperatureColors = new List<Color32>();

        [NotNull]
        public static readonly List<float> DefaultTemperatures = new List<float>();

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

        private static Dictionary<string, Color32> _typeColorOffsets;

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

			//private set => _configuratorState = value;
			private set {
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

                // Dictionary<SimHashes, ElementColorInfo> colorInfos;
                JsonLoader.TryLoadElementColorInfos(out _materialColors);

                return _materialColors;
            }

			//private set => _elementColorInfos = value;
			private set
			{
				_materialColors = value;
			}
		}

        [NotNull]
        public static Logger Logger => _logger ?? (_logger = new ONI_Common.IO.Logger(Paths.ModsDirectory+ System.IO.Path.DirectorySeparatorChar + "_Logs" + System.IO.Path.DirectorySeparatorChar + Paths.MaterialColorLogFileName));

        [NotNull]
        public static Dictionary<string, Color32> TypeColorOffsets
        {
            get
            {
                if (_typeColorOffsets != null)
                {
                    return _typeColorOffsets;
                }

                JsonLoader.TryLoadTypeColorOffsets(out _typeColorOffsets);

                return _typeColorOffsets;
            }

			//private set => _typeColorOffsets = value;
			private set
			{
				_typeColorOffsets = value;
			}
		}

        public static bool TryReloadConfiguratorState()
        {
			//if (!JsonLoader.TryLoadConfiguratorState(out MaterialColorState state))
			MaterialColorState state;
			if (!JsonLoader.TryLoadConfiguratorState(out state))
			{
				return false;
            }

            ConfiguratorState = state;

            return true;
        }

        public static bool TryReloadElementColorInfos()
        {
			//if (!JsonLoader.TryLoadElementColorInfos(out Dictionary<SimHashes, ElementColorInfo> colorInfos))
			Dictionary<SimHashes, Color> colorInfos;
			if (!JsonLoader.TryLoadElementColorInfos(out colorInfos))
			{
                return false;
            }

            ElementColorInfos = colorInfos;

            return true;
        }

        public static bool TryReloadTypeColorOffsets()
        {
			//if (!JsonLoader.TryLoadTypeColorOffsets(out Dictionary<string, Color32> colorOffsets))
			Dictionary<string, Color32> colorOffsets;
			if (!JsonLoader.TryLoadTypeColorOffsets(out colorOffsets))
			{
                return false;
            }

            TypeColorOffsets = colorOffsets;

            return true;
        }
    }
}