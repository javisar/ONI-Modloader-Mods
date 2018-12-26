using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace FluidWarpMod
{
          
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class LiquidWarpMod_GeneratedBuildings_LoadGeneratedBuildings
	{
        private static void Prefix()
        {
            Logger.LogFormat(" === GeneratedBuildings Prefix === {0}", LiquidWarpConfig.ID);
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDWARP.NAME", "Liquid Stargate");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDWARP.DESC", "Liquid Stargates provides an easy way to transport liquids from one place to another.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDWARP.EFFECT", "Place one providing input fluid, and another one with an output pipe. Sintonize your stargates using the same channel.");

			ModUtil.AddBuildingToPlanScreen("Plumbing", LiquidWarpConfig.ID);

			/*
            List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => po.category == new HashedString("Plumbing")).data;
            category.Add(LiquidWarpConfig.ID);
			*/

		}
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class LiquidWarpMod_Db_Initialize
	{
        private static void Prefix(Db __instance)
        {
            Logger.LogFormat(" === Database.Techs loaded === {0}",  LiquidWarpConfig.ID);
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["ImprovedLiquidPiping"]);
            ls.Add(LiquidWarpConfig.ID);
            Database.Techs.TECH_GROUPING["ImprovedLiquidPiping"] = (string[])ls.ToArray();
        }
    }
}
