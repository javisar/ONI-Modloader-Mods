using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PowerWarpMod
{

    internal class PowerWarpMod_Utils
    {
        public static ConduitType GetConduitType(ValveSideScreen __instance)
        {
            FieldInfo fi1 = AccessTools.Field(typeof(ValveSideScreen), "targetValve");
            FieldInfo fi2 = AccessTools.Field(typeof(Valve), "valveBase");

            ConduitType type = ((ValveBase)fi2.GetValue(fi1.GetValue(__instance))).conduitType;
            return type;
        }
    }
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class PowerWarpMod_GeneratedBuildings_LoadGeneratedBuildings
    {
        private static void Prefix()
        {
            Debug.Log(" === PowerWarpMod_GeneratedBuildings_LoadGeneratedBuildings Prefix === " + PowerWarpConfig.ID);
            Strings.Add("STRINGS.BUILDINGS.PREFABS.POWERWARP.NAME", "Power Stargate");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.POWERWARP.DESC", "Power Stargates provides an easy way to transport power from one place to another.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.POWERWARP.EFFECT", "Place one providing input wire, and another one with an output wire. Sintonize your stargates using the same channel.");

            List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
            ls.Add(PowerWarpConfig.ID);
            TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(PowerWarpConfig.ID);


        }
        private static void Postfix()
        {

            Debug.Log(" === PowerWarpMod_GeneratedBuildings_LoadGeneratedBuildings Postfix === " + PowerWarpConfig.ID);
            object obj = Activator.CreateInstance(typeof(PowerWarpConfig));
            BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class PowerWarpMod_Db_Initialize
    {
        private static void Prefix(Db __instance)
        {
            Debug.Log(" === PowerWarpMod_Db_Initialize loaded === " + PowerWarpConfig.ID);
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["AdvancedPowerRegulation"]);
            ls.Add(PowerWarpConfig.ID);
            Database.Techs.TECH_GROUPING["AdvancedPowerRegulation"] = (string[])ls.ToArray();            
        }
    }

    [HarmonyPatch(typeof(ValveBase), "ConduitUpdate")]
    internal class PowerWarpMod_ValveBase_ConduitUpdate
    {
        private static bool Prefix(ValveBase __instance, float dt)
        {
            


            return true;
        }
    }


    [HarmonyPatch(typeof(ValveSideScreen), "OnSpawn")]
    internal class PowerWarpMod_ValveSideScreen_OnSpawn
    {
        private static void Postfix(ValveSideScreen __instance)
        {
            Debug.Log(" === PowerWarpMod_ValveSideScreen_OnSpawn Postfix === ");

            FieldInfo fi0 = AccessTools.Field(typeof(ValveSideScreen), "unitsLabel");
            ConduitType type = PowerWarpMod_Utils.GetConduitType(__instance);
            if (type == (ConduitType)102)
            {
                ((LocText)fi0.GetValue(__instance)).text = "Ch.";
            }

        }

    }

    [HarmonyPatch(typeof(SideScreenContent), "GetTitle")]
    internal class PowerWarpMod_SideScreenContent_GetTitle
    {
        private static bool Prefix(SideScreenContent __instance, ref string __result)
        {
            Debug.Log(" === PowerWarpMod_SideScreenContent_GetTitle Postfix === ");

            if (!(__instance is ValveSideScreen)) return true;

            ConduitType type = PowerWarpMod_Utils.GetConduitType((ValveSideScreen)__instance);
            if (type == (ConduitType)102)
            {
                __result = "Channel";
                return false;
            }
            else
            {
                return true;
            }

        }

    }

    [HarmonyPatch(typeof(ValveSideScreen), "SetTarget")]
    internal class PowerWarpMod_ValveSideScreen_SetTarget
    {
        private static void Postfix(ValveSideScreen __instance)
        {
            Debug.Log(" === PowerWarpMod_ValveSideScreen_SetTarget Postfix === ");

            FieldInfo fi3 = AccessTools.Field(typeof(ValveSideScreen), "minFlowLabel");
            FieldInfo fi4 = AccessTools.Field(typeof(ValveSideScreen), "maxFlowLabel");

            ConduitType type = PowerWarpMod_Utils.GetConduitType(__instance);
            if (type == (ConduitType)102)
            {
                ((LocText)fi3.GetValue(__instance)).text = "Ch.";
                ((LocText)fi4.GetValue(__instance)).text = "Ch.";
            }

        }

    }
}
