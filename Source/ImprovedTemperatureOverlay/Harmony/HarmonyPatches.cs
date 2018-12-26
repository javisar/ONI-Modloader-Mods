namespace ImprovedTemperatureOverlay
{
    using Harmony;
    //using Extensions;
    //using Helpers;
    using TemperatureOverlay;    
    using ONI_Common.Core;
    using ONI_Common.IO;
    using Rendering;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using JetBrains.Annotations;

    using UnityEngine;
    using Action = Action;
	using static KInputController;
	using System.Reflection;
    //using MaterialColor.Data;

    public static class HarmonyPatches
    {

        private static bool _initialized;


        private static void Initialize()
        {
            SubscribeToFileChangeNotifier();
            _initialized = true;
        }
		
        // TODO: log failed reload on other eventhandlers
        private static void OnTemperatureStateChanged(object sender, FileSystemEventArgs e)
        {
            string message;

            if (State.TryReloadTemperatureState())
            {
                UpdateTemperatureThresholds();
                message = "Temperature overlay state changed.";
            }
            else
            {
                message = "Temperature overlay state load failed.";
            }

            State.Logger.Log(message);
            Debug.LogError(message);
        }
		
        private static void SaveTemperatureThresholdsAsDefault()
        {
            if (SimDebugView.Instance?.temperatureThresholds != null)
            {
                foreach (SimDebugView.ColorThreshold threshold in SimDebugView.Instance?.temperatureThresholds)
                {
                    State.DefaultTemperatureColors.Add(threshold.color);
                    State.DefaultTemperatures.Add(threshold.value);
                }
            }
        }
		
		
		private static object GetField(object _instance, string name)
		{
			FieldInfo fi = AccessTools.Field(_instance.GetType(), name);
			return fi.GetValue(_instance);
		}

		private static void SetField(object _instance, string name, object value)
		{
			FieldInfo fi = AccessTools.Field(_instance.GetType(), name);
			fi.SetValue(_instance, value);
		}

		private static object Invoke(object _instance, string name)
		{
			MethodInfo mi = AccessTools.Method(_instance.GetType(), name);
			return mi.Invoke(_instance, new object[] { });
		}

        private static void SubscribeToFileChangeNotifier()
        {
            const string jsonFilter = "*.json";

            try
            {
                
                if (State.TemperatureOverlayState.CustomRangesEnabled)
                {
                    FileChangeNotifier.StartFileWatch(
                                                      Paths.TemperatureStateFileName,
                                                      Paths.MaterialConfigPath,
                                                      OnTemperatureStateChanged);
                }
            }
            catch (Exception e)
            {
                State.Logger.Log("SubscribeToFileChangeNotifier failed");
                State.Logger.Log(e);
            }
        }
		
        private static void LogTemperatureThresholds()
        {
            for (int i = 0; i < SimDebugView.Instance.temperatureThresholds.Length; i++)
            {
                string message = SimDebugView.Instance.temperatureThresholds[i].value.ToString();
                Color32 color = SimDebugView.Instance.temperatureThresholds[i].color;

                State.Logger.Log("Temperature Color " + i + " at " + message + " K: " + color);
            }
        }

        private static void UpdateTemperatureThresholds()
        {
            List<float> newTemperatures = State.TemperatureOverlayState.CustomRangesEnabled
                                          ? State.TemperatureOverlayState.Temperatures
                                          : State.DefaultTemperatures;

            List<Color32> newColors = State.TemperatureOverlayState.CustomRangesEnabled
                                    ? State.TemperatureOverlayState.Colors
                                    : State.DefaultTemperatureColors;

            //State.Logger.Log("CustomRangesEnabled " + State.TemperatureOverlayState.CustomRangesEnabled);
            for (int i = 0; i < newTemperatures.Count; i++)
            {
                //State.Logger.Log("newTemperatures[i] " + newTemperatures[i]);
                if (SimDebugView.Instance.temperatureThresholds != null)
                {
                    //State.Logger.Log("SimDebugView.Instance.temperatureThresholds[i] " + SimDebugView.Instance.temperatureThresholds[i].value);
                    SimDebugView.Instance.temperatureThresholds[i] =
                    new SimDebugView.ColorThreshold { color = newColors[i], value = newTemperatures[i] };
                    //State.Logger.Log("SimDebugView.Instance.temperatureThresholds[i] " + SimDebugView.Instance.temperatureThresholds[i].value);
                }
            }

            Array.Sort(SimDebugView.Instance.temperatureThresholds, new ColorThresholdTemperatureSorter());
        }
		

        [HarmonyPatch(typeof(Game), "Update")]
        public static class Game_Update_EnterEveryUpdate_CoreUpdateQueueManager
        {
            public static void Prefix()
            {
                UpdateQueueManager.OnGameUpdate();
            }
        }
		        
		

        /// <summary>
        /// Material + element color
        /// </summary>
        [HarmonyPatch(typeof(Game), "OnPrefabInit")]
        public static class SimDebugView_OnPrefabInit_EnterOnce
        {
            [HarmonyPostfix]
            private static void Postfix()
            {
                              

                // Temp col overlay
                try
                {
                    SaveTemperatureThresholdsAsDefault();
                    if (State.TemperatureOverlayState.LogThresholds)
                    {
                        State.Logger.Log("Before: ");
                        LogTemperatureThresholds();
                    }
                    /*
                    if (!State.TryReloadTemperatureState())
                    {
                        State.Logger.Log("Error loading temperatures config file. ");
                    }
                    */
                    UpdateTemperatureThresholds();

                    if (State.TemperatureOverlayState.LogThresholds)
                    {
                        State.Logger.Log("After: ");
                        LogTemperatureThresholds();
                    }
                }
                catch (Exception e)
                {
                    State.Logger.Log("Custom temperature overlay init error");
                    State.Logger.Log(e);
                }
                
            }
        }
    }
}