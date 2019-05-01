using Harmony;
using UnityEngine;


namespace SpeedControl
{
    

    internal class SpeedControlMod_OnLoad
    {
        public static void OnLoad(string modPath)
        {
            SpeedControlConfig.LoadConfig(modPath);
        }
    }


    [HarmonyPatch(typeof(SpeedControlScreen), "OnChanged")]
    internal static class SpeedControl
    {
        private static bool Prefix(SpeedControlScreen __instance)
        {
            if (!SpeedControlConfig.Config.Enabled)
                return true;

            if (SpeedControlConfig.Config.Logging)
                Debug.Log(" === SpeedControl INI === "+ Time.timeScale);

            if (__instance.IsPaused)
            {
                Time.timeScale = 0f;
            }
            else if (__instance.GetSpeed() == 0)
            {
                Time.timeScale = SpeedControlConfig.Config.SpeedMultiplier1;
            }
            else if (__instance.GetSpeed() == 1)
            {
				//Time.timeScale = __instance.fastSpeed;
				Time.timeScale = SpeedControlConfig.Config.SpeedMultiplier2;
            }
            else if (__instance.GetSpeed() == 2)
            {
                Time.timeScale = SpeedControlConfig.Config.SpeedMultiplier3;
            }

            //__instance.OnGameSpeedChanged?.Invoke();

            if (SpeedControlConfig.Config.Logging)
                Debug.Log(" === SpeedControl END === " + Time.timeScale);

            return false;
        }
    }
}