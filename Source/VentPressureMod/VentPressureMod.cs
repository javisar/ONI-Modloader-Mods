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


	[HarmonyPatch(typeof(IBuildingConfig), "ConfigureBuildingTemplate")]
	internal class VentPressureMod_IBuildingConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(IBuildingConfig __instance, GameObject go, Tag prefab_tag)
		{
			//Debug.Log(" === VentPressureMod_IBuildingConfig_ConfigureBuildingTemplate Postfix === ");
			Vent vent = go.GetComponent<Vent>();
			//FieldInfo fi = AccessTools.Field(typeof(IBuildingConfig), "ID");
			foreach (KeyValuePair<string, float> entry in VentPressureState.StateManager.State.MaximumPressure)
			{
				Debug.Log(" === VentPressureMod_IBuildingConfig_ConfigureBuildingTemplate Postfix === "+ prefab_tag);
				if (prefab_tag == entry.Key)
				{
					Debug.Log(" === VentPressureMod_IBuildingConfig_ConfigureBuildingTemplate("+entry.Key+"+"+entry.Value+") === ");
					vent.overpressureMass = entry.Value;
				}
			}
		}
	}
	

}
