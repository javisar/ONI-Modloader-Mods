using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiWiresMod
{

    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class MultiWiresMod_GeneratedBuildings_LoadGeneratedBuildings
    {
        private static void Prefix()
        {
            Debug.Log(" === MultiWiresMod_GeneratedBuildings_LoadGeneratedBuildings Prefix === " + MultiWireConfig.ID);
            Strings.Add("STRINGS.BUILDINGS.PREFABS.MULTIWIRE.NAME", "Multi Wire");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.MULTIWIRE.DESC", "");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.MULTIWIRE.EFFECT", "");

            List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
            ls.Add(MultiWireConfig.ID);
            TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(MultiWireConfig.ID);


        }
        private static void Postfix()
        {

            Debug.Log(" === MultiWiresMod_GeneratedBuildings_LoadGeneratedBuildings Postfix === " + MultiWireConfig.ID);
            object obj = Activator.CreateInstance(typeof(MultiWireConfig));
            BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class MultiWiresMod_Db_Initialize
    {
        private static void Prefix(Db __instance)
        {
            Debug.Log(" === MultiWiresMod_Db_Initialize loaded === " + MultiWireConfig.ID);
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["SmartStorage"]);
            ls.Add(MultiWireConfig.ID);
            Database.Techs.TECH_GROUPING["SmartStorage"] = (string[])ls.ToArray();
        }
    }
    
}
