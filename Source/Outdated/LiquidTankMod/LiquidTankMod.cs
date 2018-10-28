using System;
using System.Collections.Generic;
using Harmony;

namespace LiquidTankMod
{

	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class LiquidTankMod_GeneratedBuildings_LoadGeneratedBuildings
    { 
        private static void Prefix()
		{
			Debug.Log(" === LiquidTankMod_GeneratedBuildings_LoadGeneratedBuildings Prefix === " + LiquidTankConfig.ID);
			Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDTANK.NAME", "Liquid Tank");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDTANK.DESC", "");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDTANK.EFFECT", "");

			List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			ls.Add(LiquidTankConfig.ID);
			TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(LiquidTankConfig.ID);


		}
		private static void Postfix()
		{

			Debug.Log(" === LiquidTankMod_GeneratedBuildings_LoadGeneratedBuildings Postfix === " + LiquidTankConfig.ID);
			object obj = Activator.CreateInstance(typeof(LiquidTankConfig));
			BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
		}
	}

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class LiquidTankMod_Db_Initialize
    {
		private static void Prefix(Db __instance)
		{
			Debug.Log(" === LiquidTankMod_Db_Initialize loaded === " + LiquidTankConfig.ID);
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["SmartStorage"]);
			ls.Add(LiquidTankConfig.ID);
			Database.Techs.TECH_GROUPING["SmartStorage"] = (string[])ls.ToArray();

			//Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
		}
	}

    [HarmonyPatch(typeof(KSerialization.Manager), "GetType", new Type[] { typeof(string) })]
    public static class LiquidTankMod_Manager_GetType
    {
        [HarmonyPostfix]
        public static void GetType(string type_name, ref Type __result)
        {
            if (type_name == "LiquidTank")
            {
                __result = typeof(LiquidTank);
            }
        }
    }

    /*
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
			result.Add(typeof(LiquidTank));
			root_types = result.ToArray();

			return true;
		}
	}
    */
}
