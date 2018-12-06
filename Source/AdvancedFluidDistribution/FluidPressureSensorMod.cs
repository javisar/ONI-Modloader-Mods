using System.Collections.Generic;
using System.IO;
using System.Linq;
using Harmony;
using ONI_Common;
using STRINGS;

namespace FluidPressureSensor
{
    public static class Logger {
#if DEBUG
        private static ONI_Common.IO.Logger logger;
#endif
        static Logger()
        {
#if DEBUG
            logger = new ONI_Common.IO.Logger(Paths.GetLogsPath() + Path.DirectorySeparatorChar + Paths.GetLogFileName("FluidPressureSensor"));
#endif
        }

        public static void Log(string message)
        {
#if DEBUG
            logger.Log(message);
#endif
        }

        public static void LogFormat(string template, params object[] args)
        {
#if DEBUG
            logger.Log(string.Format(template, args));
#endif
        }

    }

    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class AdvFluidDistrMod_GeneratedBuildings_LoadGeneratedBuildings
    {
        private static void Prefix()
        {
            Logger.Log("Entry AdvFluidDistrMod_GeneratedBuildings_LoadGeneratedBuildings.Prefix ");
            Strings.Add("STRINGS.BUILDINGS.PREFABS." + GasConduitPressureConfig.ID.ToUpper() + ".NAME", "Gas Pipe Pressure Sensor");
            Strings.Add("STRINGS.BUILDINGS.PREFABS." + GasConduitPressureConfig.ID.ToUpper() + ".DESC", "Gas Pipe Pressure Sensor can disable buildings when contents reach a certain mass.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS." + GasConduitPressureConfig.ID.ToUpper() + ".EFFECT", string.Concat(new string[] { "Becomes ", UI.FormatAsLink("Active", "LOGIC"), " or on ", UI.FormatAsLink("Standby", "LOGIC"), " when the pipe contents mass enters the chosen range." }));
            ModUtil.AddBuildingToPlanScreen(new HashedString("HVAC"), GasConduitPressureConfig.ID);

            Strings.Add("STRINGS.BUILDINGS.PREFABS." + LiquidConduitPressureConfig.ID.ToUpper() + ".NAME", "Liquid Pipe Pressure Sensor");
            Strings.Add("STRINGS.BUILDINGS.PREFABS." + LiquidConduitPressureConfig.ID.ToUpper() + ".DESC", "Liquid Pipe Pressure Sensor can disable buildings when contents reach a certain mass.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS." + LiquidConduitPressureConfig.ID.ToUpper() + ".EFFECT", string.Concat(new string[] { "Becomes ", UI.FormatAsLink("Active", "LOGIC"), " or on ", UI.FormatAsLink("Standby", "LOGIC"), " when the pipe contents mass enters the chosen range." }));
            ModUtil.AddBuildingToPlanScreen(new HashedString("Plumbing"), LiquidConduitPressureConfig.ID);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class AdvFluidDistrMod_Db_Initialize
    {
        private static void Prefix(Db __instance)
        {
            Logger.Log("Entry AdvFluidDistrMod_Db_Initialize.Prefix");
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["ImprovedGasPiping"]);
            ls.Add(GasConduitPressureConfig.ID);
            ls.Add(LiquidConduitPressureConfig.ID);
            Database.Techs.TECH_GROUPING["ImprovedGasPiping"] = (string[])ls.ToArray();
            Logger.Log("Exit AdvFluidDistrMod_Db_Initialize.Prefix");
        }
    }

}
