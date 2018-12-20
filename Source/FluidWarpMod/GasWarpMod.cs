using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace FluidWarpMod
{
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class GasWarpMod_GeneratedBuildings_LoadGeneratedBuildings
	{
        private static void Prefix()
        {
            Logger.Log(" === GeneratedBuildings Prefix === " + GasWarpConfig.ID);
            Strings.Add("STRINGS.BUILDINGS.PREFABS.GASWARP.NAME", "Gas Stargate");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.GASWARP.DESC", "Gas Stargates provides an easy way to transport gases from one place to another.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.GASWARP.EFFECT", "Place one providing input fluid, and another one with an output pipe. Sintonize your stargates using the same channel.");

			ModUtil.AddBuildingToPlanScreen("HVAC", GasWarpConfig.ID);

			/*
            List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => po.category == new HashedString("HVAC")).data;
            category.Add(GasWarpConfig.ID);
			*/


		}
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class GasWarpMod_Db_Initialize
	{
        private static void Prefix(Db __instance)
        {
            Logger.Log(" === Database.Techs loaded === " + GasWarpConfig.ID);
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["ImprovedGasPiping"]);
            ls.Add(GasWarpConfig.ID);
            Database.Techs.TECH_GROUPING["ImprovedGasPiping"] = (string[])ls.ToArray();
        }
    }
}
