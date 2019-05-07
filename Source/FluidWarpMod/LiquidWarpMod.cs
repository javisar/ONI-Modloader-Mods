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
            Logger.LogFormat(" === GeneratedBuildings Prefix === {0}", WarpProviderLiquid.ID);
            Strings.Add("STRINGS.BUILDINGS.PREFABS.WARPPROVIDERLIQUID.NAME", "Liquid Warp Provider");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.WARPPROVIDERLIQUID.DESC", "Liquid Warp Providers provides an easy way to push liquids to a Receiver.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.WARPPROVIDERLIQUID.EFFECT", "Place one liquid warp provider, and place one liquid warp requester. Synchronize your warpgates using the same channel.");

            Strings.Add("STRINGS.BUILDINGS.PREFABS.WARPREQUESTERLIQUID.NAME", "Liquid Warp Requester");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.WARPREQUESTERLIQUID.DESC", "Liquid Warp Requesters provides an easy way to request liquids from a Provider");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.WARPREQUESTERLIQUID.EFFECT", "Place one liquid warp provider, and place one liquid warp requester. Synchronize your warpgates using the same channel.");

            ModUtil.AddBuildingToPlanScreen("Plumbing", WarpProviderLiquid.ID);
            ModUtil.AddBuildingToPlanScreen("Plumbing", WarpRequesterLiquid.ID);

        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class LiquidWarpMod_Db_Initialize
	{
        private static void Prefix(Db __instance)
        {
            Logger.LogFormat(" === Database.Techs loaded === {0}", WarpProviderLiquid.ID);
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["ImprovedLiquidPiping"]);
            ls.Add(WarpProviderLiquid.ID);
            ls.Add(WarpRequesterLiquid.ID);
            Database.Techs.TECH_GROUPING["ImprovedLiquidPiping"] = (string[])ls.ToArray();
        }
    }
}
