using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;

namespace ONIModManager
{

	[HarmonyPatch(typeof(OptionsMenuScreen), "OnAudioOptions")]
	internal class ONIModManager_OptionsMenuScreen_OnAudioOptions
	{

		private static bool Prefix(OptionsMenuScreen __instance)
		{
			Debug.Log(" === ONIModManager_OptionsMenuScreen_OnAudioOptions Postfix === " + ONIModManager_Manager_Initialize.go);
			MethodInfo mi = AccessTools.Method(typeof(KModalButtonMenu), "ActivateChildScreen");
			mi.Invoke(((KModalButtonMenu)__instance), new object[] { ONIModManager_Manager_Initialize.go });
			return false;
		}
	}

	[HarmonyPatch(typeof(KSerialization.Manager), "Initialize")]
	internal class ONIModManager_Manager_Initialize
	{
		public static GameObject go = null;

		private static void Postfix(Global __instance)
		{
			Debug.Log(" === ONIModManager_Manager_Initialize Postfix === ");
			UI.Load();
			//go = AudioOptionsScreen.Load();
		}
	}
}
