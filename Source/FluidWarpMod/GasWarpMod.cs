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
            Logger.Log(" === GeneratedBuildings Prefix === " + WarpProviderGas.ID);
            Strings.Add("STRINGS.BUILDINGS.PREFABS.WARPPROVIDERGAS.NAME", "Gas Warp Provider");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.WARPPROVIDERGAS.DESC", "Gas Warp Providers provides an easy way to transport gases from one place to another.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.WARPPROVIDERGAS.EFFECT", "Place one providing input fluid, and another one with an output pipe. Sintonize your stargates using the same channel.");

            Strings.Add("STRINGS.BUILDINGS.PREFABS.WARPREQUESTERGAS.NAME", "Gas Warp Requester");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.WARPREQUESTERGAS.DESC", "Gas Warp Requesters provides an easy way to transport gases from one place to another.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.WARPREQUESTERGAS.EFFECT", "Place one providing input fluid, and another one with an output pipe. Sintonize your stargates using the same channel.");

            ModUtil.AddBuildingToPlanScreen("HVAC", WarpProviderGas.ID);
            ModUtil.AddBuildingToPlanScreen("HVAC", WarpRequesterGas.ID);

        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class GasWarpMod_Db_Initialize
	{
        private static void Prefix(Db __instance)
        {
            Logger.Log(" === Database.Techs loaded === " + WarpProviderGas.ID);
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["ImprovedGasPiping"]);
            ls.Add(WarpProviderGas.ID);
            ls.Add(WarpRequesterGas.ID);
            Database.Techs.TECH_GROUPING["ImprovedGasPiping"] = (string[])ls.ToArray();
        }
    }
}
