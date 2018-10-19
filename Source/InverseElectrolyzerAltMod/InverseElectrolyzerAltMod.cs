using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;


namespace InverseElectrolyzerAltMod
{

	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class InverseElectrolyzerAltMod
	{
		private static void Prefix()
		{
			Debug.Log(" === GeneratedBuildings Prefix === " + InverseElectrolyzerAltConfig.ID);
			Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZERALT.NAME", "Combustioneer");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZERALT.DESC", "");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZERALT.EFFECT", "");
            /*
			List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			ls.Add(InverseElectrolyzerAltConfig.ID);
			TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(InverseElectrolyzerAltConfig.ID);
            */
            List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Utilities).data;
            category.Add(InverseElectrolyzerAltConfig.ID);

        }
        /*
		private static void Postfix()
		{

			Debug.Log(" === GeneratedBuildings Postfix === " + InverseElectrolyzerAltConfig.ID);
			object obj = Activator.CreateInstance(typeof(InverseElectrolyzerAltConfig));
			BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
		}
        */
	}

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class InverseElectrolyzerAltTechMod
	{
		private static void Prefix(Db __instance)
		{
			Debug.Log(" === Database.Techs loaded === " + InverseElectrolyzerAltConfig.ID);
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["Combustion"]);
			ls.Add(InverseElectrolyzerAltConfig.ID);
			Database.Techs.TECH_GROUPING["Combustion"] = (string[])ls.ToArray();

			//Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
		}
	}

	[HarmonyPatch(typeof(KSerialization.Manager), "GetType", new Type[] { typeof(string) })]
	public static class GasTankMod_Manager_GetType
	{
		[HarmonyPostfix]
		public static void GetType(string type_name, ref Type __result)
		{
			if (type_name == "InverseElectrolyzerAlt")
			{
				__result = typeof(InverseElectrolyzerAlt);
			}
		}
	}
	

}
