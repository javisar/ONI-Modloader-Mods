namespace ImprovedTemperatureOverlay
{
    using JetBrains.Annotations;
    //using MaterialColor.Data;
    using ImprovedTemperatureOverlay.Json;
    using ONI_Common.Data;
    using ONI_Common.Json;
    using System;
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
        
        private static Logger _logger;

        private static ImprovedTemperatureOverlayState _temperatureOvelayState;

        [NotNull]
        public static Logger Logger => _logger ?? (_logger = new ONI_Common.IO.Logger(Paths.ModsDirectory+ System.IO.Path.DirectorySeparatorChar + "_Logs" + System.IO.Path.DirectorySeparatorChar + Paths.MaterialColorLogFileName));

        [NotNull]
        public static ImprovedTemperatureOverlayState TemperatureOverlayState
        {
            get
            {
                if (_temperatureOvelayState != null)
                {
                    return _temperatureOvelayState;
                }

                JsonLoader.TryLoadTemperatureState(out _temperatureOvelayState);

                return _temperatureOvelayState;
            }

			//private set => _temperatureOvelayState = value;
			private set
			{
				_temperatureOvelayState = value;
			}
		}


        public static bool TryReloadTemperatureState()
        {
			//if (!JsonLoader.TryLoadTemperatureState(out TemperatureOverlayState temperatureState))
			ImprovedTemperatureOverlayState temperatureState;
			if (!JsonLoader.TryLoadTemperatureState(out temperatureState))
			{
                return false;
            }

            TemperatureOverlayState = temperatureState;

            return true;
        }
		
    }
}