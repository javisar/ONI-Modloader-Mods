namespace SpeedControlMod
{
    using Harmony;
    using System;
	using System.Reflection;
	using UnityEngine;
    using Debug = Debug;

	[HarmonyPatch(typeof(SpeedControlScreen), "SetSpeed")]
	internal static class SpeedControlMod_SpeedControlScreen_SetSpeed
	{
		private static FieldInfo speedF = AccessTools.Field(typeof(SpeedControlScreen), "speed");
		private static FieldInfo slowButtonF = AccessTools.Field(typeof(SpeedControlScreen), "slowButton");
		private static FieldInfo mediumButtonF = AccessTools.Field(typeof(SpeedControlScreen), "mediumButton");
		private static FieldInfo fastButtonF = AccessTools.Field(typeof(SpeedControlScreen), "fastButton");

		private static bool Prefix(SpeedControlScreen __instance, int Speed)
		{
			Debug.Log(" === SpeedControlMod_SpeedControlScreen_SetSpeed INI === Speed = " + Speed);

			speedF.SetValue(__instance, Speed % 9);
			switch ((int) speedF.GetValue(__instance))
			{
				case 0:
					((KToggle)slowButtonF.GetValue(__instance)).Select();
					((KToggle)slowButtonF.GetValue(__instance)).isOn = true;
					((KToggle)mediumButtonF.GetValue(__instance)).isOn = false;
					((KToggle)fastButtonF.GetValue(__instance)).isOn = false;					
					break;
				case 1:
					((KToggle)mediumButtonF.GetValue(__instance)).Select();
					((KToggle)slowButtonF.GetValue(__instance)).isOn = false;
					((KToggle)mediumButtonF.GetValue(__instance)).isOn = true;
					((KToggle)fastButtonF.GetValue(__instance)).isOn = false;
					break;
				case 2:
					((KToggle)fastButtonF.GetValue(__instance)).Select();
					((KToggle)slowButtonF.GetValue(__instance)).isOn = false;
					((KToggle)mediumButtonF.GetValue(__instance)).isOn = false;
					((KToggle)fastButtonF.GetValue(__instance)).isOn = true;
					break;
				default:
					break;
			}
			__instance.OnSpeedChange();
			return false;
		}
	}

	[HarmonyPatch(typeof(SpeedControlScreen), "OnChanged", new Type[0])]
    internal static class SpeedControlMod_SpeedControlScreen_OnChanged
	{
        private static bool Prefix(SpeedControlScreen __instance)
        {
            Debug.Log(" === SpeedControlMod_SpeedControlScreen_OnChanged INI === __instance.GetSpeed() = " + __instance.GetSpeed());

            if (__instance.IsPaused)
            {
                Time.timeScale = 0f;
            }
            else if (__instance.GetSpeed() == 0)
            {
                Time.timeScale = 0.06f;
            }
            else if (__instance.GetSpeed() == 1)
            {
				Time.timeScale = 0.5f;
			}
            else if (__instance.GetSpeed() == 2)
            {
                Time.timeScale = 1f;
            }
			else if (__instance.GetSpeed() == 3)
			{
				Time.timeScale = 2f;
			}
			else if (__instance.GetSpeed() == 4)
			{
				Time.timeScale = 3f;
			}
			else if (__instance.GetSpeed() == 5)
			{
				Time.timeScale = 5f;
			}
			else if (__instance.GetSpeed() == 6)
			{
				Time.timeScale = 10f;
			}
			else if (__instance.GetSpeed() == 7)
			{
				Time.timeScale = 15f;
			}
			else if (__instance.GetSpeed() == 8)
			{
				Time.timeScale = 30f;
			}

			//__instance.OnGameSpeedChanged?.Invoke();

			//Debug.Log(" === SpeedControlMod END === ");
			Debug.Log(" === SpeedControlMod_SpeedControlScreen_OnChanged INI === Time.timeScale = " + Time.timeScale);

			return false;
        }
    }
}