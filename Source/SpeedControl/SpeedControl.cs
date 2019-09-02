using Harmony;
using UnityEngine;


namespace SpeedControl
{


    [HarmonyPatch(typeof(SpeedControlScreen), "OnChanged")]
    internal static class SpeedControl
    {
        private static bool Prefix(SpeedControlScreen __instance)
        {
            if (!SpeedControlConfig.Instance.Enabled)
                return true;

            if (SpeedControlConfig.Instance.Logging)
                Debug.Log(" === SpeedControl INI === "+ Time.timeScale);

            if (__instance.IsPaused)
            {
                Time.timeScale = 0f;
            }
            else if (__instance.GetSpeed() == 0)
            {
                Time.timeScale = SpeedControlConfig.Instance.SpeedMultiplier1;
            }
            else if (__instance.GetSpeed() == 1)
            {
				//Time.timeScale = __instance.fastSpeed;
				Time.timeScale = SpeedControlConfig.Instance.SpeedMultiplier2;
            }
            else if (__instance.GetSpeed() == 2)
            {
                Time.timeScale = SpeedControlConfig.Instance.SpeedMultiplier3;
            }

            //__instance.OnGameSpeedChanged?.Invoke();

            if (SpeedControlConfig.Instance.Logging)
                Debug.Log(" === SpeedControl END === " + Time.timeScale);

            return false;
        }
    }


	/*
    internal class SpeedControlMod_OnLoad
    {
        public static void OnLoad(string modPath)
        {
            SpeedControlConfig.LoadConfig(modPath);
        }
    }
	*/
}