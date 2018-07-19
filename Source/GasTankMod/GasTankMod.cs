using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace GasTankMod
{

	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class Patches_GeneratedBuildings_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Debug.Log(" === GeneratedBuildings.LoadGeneratedBuildings Prefix === " + GasTankConfig.ID);
			Strings.Add("STRINGS.BUILDINGS.PREFABS.GASTANK.NAME", "Gas Tank");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.GASTANK.DESC", "");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.GASTANK.EFFECT", "");

			List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			ls.Add(GasTankConfig.ID);
			TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(GasTankConfig.ID);


		}
		private static void Postfix()
		{

			Debug.Log(" === GeneratedBuildings.LoadGeneratedBuildings Postfix === " + GasTankConfig.ID);
			object obj = Activator.CreateInstance(typeof(GasTankConfig));
			BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
		}
	}

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class InverseElectrolyzerTechMod
	{
		private static void Prefix(Db __instance)
		{
			Debug.Log(" === Db.Initialize loaded === " + GasTankConfig.ID);
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["SmartStorage"]);
			ls.Add(GasTankConfig.ID);
			Database.Techs.TECH_GROUPING["SmartStorage"] = (string[])ls.ToArray();

			//Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
		}
	}

	[HarmonyPatch(typeof(KSerialization.Manager), "Initialize")]
	internal class KSerialization_Manager_Initialize
	{
		private static bool Prefix(ref IList<Type> root_types)
		{
			Debug.Log(" === KSerialization_Manager_Initialize === ");

			List<Type> result = new List<Type>();
			for (int i = 0; i < root_types.Count; i++)
			{
				result.Add(root_types[i]);
			}
			result.Add(typeof(GasTank));
			root_types = result.ToArray();

			return true;
		}
	}
}
