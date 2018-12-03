using System.Collections.Generic;
using System.IO;
using System.Linq;
using Harmony;
using ONI_Common;
using STRINGS;

namespace AdvancedFluidDistribution
{
    public static class Logger {

        static Logger()
        {

        }

        public static void Log(string message)
        {
#if DEBUG
            AdvFluidDistribState.StateManager.Logger.Log(message);
#endif
        }

        public static void LogFormat(string template, params object[] args)
        {
#if DEBUG
            AdvFluidDistribState.StateManager.Logger.Log(string.Format(template, args));
#endif
        }

    }

    [HarmonyPatch(typeof(Game), "OnPrefabInit")]
	internal class AdvFluidDistrMod_Game_OnPrefabInit
	{

		private static void Postfix(Game __instance)
		{
			if (!AdvFluidDistribState.StateManager.State.Enabled) return;

			Logger.Log(" === AdvFluidDistrMod_Game_OnPrefabInit Postfix === ");

		}
	}

    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class AdvFluidDistrMod_GeneratedBuildings_LoadGeneratedBuildings
    {
        private static void Prefix()
        {
            if (!AdvFluidDistribState.StateManager.State.Enabled) return;
            Logger.Log(" === GeneratedBuildings Prefix === " + GasConduitPressureConfig.ID);
            Strings.Add("STRINGS.BUILDINGS.PREFABS.GASCONDUITPRESSURESENSOR.NAME", "Gas Pipe Pressure Sensor");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.GASCONDUITPRESSURESENSOR.DESC", "Gas Pipe Pressure Sensor can disable buildings when contents reach a certain mass.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.GASCONDUITPRESSURESENSOR.EFFECT", string.Concat(new string[] { "Becomes ", UI.FormatAsLink("Active", "LOGIC"), " or on ", UI.FormatAsLink("Standby", "LOGIC"), " when the pipe contents mass enters the chosen range." }));
            List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.HVAC).data;
            category.Add(GasConduitPressureConfig.ID);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class AdvFluidDistrMod_Db_Initialize
    {
        private static void Prefix(Db __instance)
        {
            if (!AdvFluidDistribState.StateManager.State.Enabled) return;
            Logger.Log(" === Database.Techs loaded === " + GasConduitPressureConfig.ID);
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["ImprovedGasPiping"]);
            ls.Add(GasConduitPressureConfig.ID);
            Database.Techs.TECH_GROUPING["ImprovedGasPiping"] = (string[])ls.ToArray();
        }
    }

}
