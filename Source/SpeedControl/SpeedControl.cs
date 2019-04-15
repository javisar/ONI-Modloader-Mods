namespace SpeedControl
{
    using Harmony;
    using System;
    using UnityEngine;
    using Debug = Debug;

    [HarmonyPatch(typeof(SpeedControlScreen), "OnChanged", new Type[0])]
    internal static class SpeedControl
    {
        private static bool Prefix(SpeedControlScreen __instance)
        {
            //Debug.Log(" === SpeedControl INI === "+ __instance.ultraSpeed);

            if (__instance.IsPaused)
            {
                Time.timeScale = 0f;
            }
            else if (__instance.GetSpeed() == 0)
            {
                Time.timeScale = 1f;
            }
            else if (__instance.GetSpeed() == 1)
            {
				//Time.timeScale = __instance.fastSpeed;
				Time.timeScale = 3f;
			}
            else if (__instance.GetSpeed() == 2)
            {
                Time.timeScale = 10f;
            }

            //__instance.OnGameSpeedChanged?.Invoke();

            //Debug.Log(" === SpeedControl END === ");

            return false;
        }
    }
}