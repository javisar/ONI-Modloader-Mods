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
            Strings.Add("STRINGS.BUILDINGS.PREFABS.ZEROPOINTMODULE.EFFECT", "The energy of the vacuum powers feeds this battery.");

            List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
            ls.Add(ZeroPointModuleConfig.ID);
            TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(ZeroPointModuleConfig.ID);


        }
        private static void Postfix()
        {

            Debug.Log(" === ZeroPointModuleMod_GeneratedBuildings_LoadGeneratedBuildings Postfix === ");
            object obj = Activator.CreateInstance(typeof(ZeroPointModuleConfig));
            BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class LiquidTankMod_Db_Initialize
    {
        private static void Prefix(Db __instance)
        {
            Debug.Log(" ===ZeroPointModuleMod_Db_Initialize loaded === ");
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["GenericSensors"]);
            ls.Add(ZeroPointModuleConfig.ID);
            Database.Techs.TECH_GROUPING["GenericSensors"] = (string[])ls.ToArray();

            //Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
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
