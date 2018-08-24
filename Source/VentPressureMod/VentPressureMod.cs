using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace VentPressureMod
{
	[HarmonyPatch(typeof(Game), "OnPrefabInit")]
	internal class VentPressureMod_Game_OnPrefabInit
	{

		private static void Postfix(Game __instance)
		{
			Debug.Log(" === VentPressureMod_Game_OnPrefabInit Postfix === ");

		}
	}


	[HarmonyPatch(typeof(GasVentConfig), "ConfigureBuildingTemplate")]
	internal class VentPressureMod_GasVentConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(GasVentConfig __instance, GameObject go, Tag prefab_tag)
		{
			Debug.Log(" === VentPressureMod_GasVentConfig_ConfigureBuildingTemplate Postfix === ");
			Vent vent = go.GetComponent<Vent>();
			VentPressureState.StateManager.State.MaximumPressure.TryGetValue("GasVent", out vent.overpressureMass);
		}
	}

	[HarmonyPatch(typeof(GasVentHighPressureConfig), "ConfigureBuildingTemplate")]
	internal class VentPressureMod_GasVentHighPressureConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(GasVentHighPressureConfig __instance, GameObject go, Tag prefab_tag)
		{
			Debug.Log(" === VentPressureMod_GasVentConfig_ConfigureBuildingTemplate Postfix === ");
			Vent vent = go.GetComponent<Vent>();
			VentPressureState.StateManager.State.MaximumPressure.TryGetValue("GasVentHighPressure", out vent.overpressureMass);
		}
	}
}
