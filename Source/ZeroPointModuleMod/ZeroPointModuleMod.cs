using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroPointModuleMod
{

    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class ZeroPointModuleMod_GeneratedBuildings_LoadGeneratedBuildings
    {
        private static void Prefix()
        {
            Debug.Log(" === ZeroPointModuleMod_GeneratedBuildings_LoadGeneratedBuildings Prefix === ");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.ZEROPOINTMODULE.NAME", "ZP Module");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.ZEROPOINTMODULE.DESC", "Zero Point Energy Module.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.ZEROPOINTMODULE.EFFECT", "The energy of the vacuum feeds this battery.");

			ModUtil.AddBuildingToPlanScreen("Power", ZeroPointModuleConfig.ID);

			/*
            List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[2].data);
            ls.Add(ZeroPointModuleConfig.ID);
            TUNING.BUILDINGS.PLANORDER[2].data = (string[])ls.ToArray();

            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(ZeroPointModuleConfig.ID);
            */
			/*
            List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString) "Power").Equals(po.category)).data;
            category.Add(ZeroPointModuleConfig.ID);
			*/

		}
        /*
        private static void Postfix()
        {

            Debug.Log(" === ZeroPointModuleMod_GeneratedBuildings_LoadGeneratedBuildings Postfix === ");
            object obj = Activator.CreateInstance(typeof(ZeroPointModuleConfig));
            BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
        }
        */
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class ZeroPointModuleMod_Db_Initialize
    {
        private static void Prefix(Db __instance)
        {
            Debug.Log(" ===ZeroPointModuleMod_Db_Initialize loaded === ");
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["RenewableEnergy"]);
            ls.Add(ZeroPointModuleConfig.ID);
            Database.Techs.TECH_GROUPING["RenewableEnergy"] = (string[])ls.ToArray();

            //Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
        }
    }

    [HarmonyPatch(typeof(Battery), "ConsumeEnergy", new Type[] { typeof(float), typeof(bool)})]
    internal class ZeroPointModuleMod_Battery_ConsumeEnergy
    {
        private static bool Prefix(Battery __instance)
        {
            //Debug.Log(" ===ZeroPointModuleMod_Battery_ConsumeEnergy loaded === ");
            if (__instance.gameObject.GetComponent<KPrefabID>().PrefabTag == ZeroPointModuleConfig.ID)
            {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Battery), "OnSpawn")]
    internal class ZeroPointModuleMod_Battery_OnSpawn
    {
        private static void Prefix(Battery __instance)
        {
            Debug.Log(" ===ZeroPointModuleMod_Battery_OnSpawn loaded === ");
            if (__instance.gameObject.GetComponent<KPrefabID>().PrefabTag == ZeroPointModuleConfig.ID)
            {
                AccessTools.Field(typeof(Battery), "joulesAvailable").SetValue(__instance, 40000f);
            }
        }
    }

    /*
    [HarmonyPatch(typeof(KSerialization.Manager), "GetType", new Type[] { typeof(string) })]
    public static class ZeroPointModuleMod_Manager_GetType
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
    */
}
