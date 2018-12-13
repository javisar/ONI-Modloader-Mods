using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace TemperatureFilterMod
{
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class TemperatureFilterMod_GeneratedBuildings_LoadGeneratedBuildings
    {
        private static LocString LIQUIDTEMPERATUREFILTER_NAME = new LocString("Liquid Temperature filter", "STRINGS.BUILDINGS.PREFABS." + LiquidTemperatureFilterConfig.ID.ToUpper() + ".NAME");
        private static LocString LIQUIDTEMPERATUREFILTER_DESC = new LocString("Liquid Temperature filters send liquid with specific temperature into a special pipe, expelling everything else through a second output.", "STRINGS.BUILDINGS.PREFABS." + LiquidTemperatureFilterConfig.ID.ToUpper() + ".DESC");
        private static LocString LIQUIDTEMPERATUREFILTER_EFFECT = new LocString("Extracts liquid with defined temperature out from a stream and put it into output pipe", "STRINGS.BUILDINGS.PREFABS." + LiquidTemperatureFilterConfig.ID.ToUpper() + ".EFFECT");
        private static LocString GASTEMPERATUREFILTER_NAME = new LocString("Gas Temperature filter", "STRINGS.BUILDINGS.PREFABS." + GasTemperatureFilterConfig.ID.ToUpper() + ".NAME");
        private static LocString GASTEMPERATUREFILTER_DESC = new LocString("Gas Temperature filters send gas with specific temperature into a special pipe, expelling everything else through a second output.", "STRINGS.BUILDINGS.PREFABS." + GasTemperatureFilterConfig.ID.ToUpper() + ".DESC");
        private static LocString GASTEMPERATUREFILTER_EFFECT = new LocString("Extracts gas with defined temperature out from a stream and put it into output pipe", "STRINGS.BUILDINGS.PREFABS." + GasTemperatureFilterConfig.ID.ToUpper() + ".EFFECT");
        private static void Prefix()
        {
            Debug.Log("Entry TemperatureFilterMod_GeneratedBuildings_LoadGeneratedBuildings.Prefix");      
            Strings.Add(LIQUIDTEMPERATUREFILTER_NAME.key.String,   LIQUIDTEMPERATUREFILTER_NAME.text);
            Strings.Add(LIQUIDTEMPERATUREFILTER_DESC.key.String,   LIQUIDTEMPERATUREFILTER_DESC.text);
            Strings.Add(LIQUIDTEMPERATUREFILTER_EFFECT.key.String, LIQUIDTEMPERATUREFILTER_EFFECT.text);
            ModUtil.AddBuildingToPlanScreen(new HashedString("Plumbing"), LiquidTemperatureFilterConfig.ID);

            Strings.Add(GASTEMPERATUREFILTER_NAME.key.String,   GASTEMPERATUREFILTER_NAME.text);
            Strings.Add(GASTEMPERATUREFILTER_DESC.key.String,   GASTEMPERATUREFILTER_DESC.text);
            Strings.Add(GASTEMPERATUREFILTER_EFFECT.key.String, GASTEMPERATUREFILTER_EFFECT.text);
            ModUtil.AddBuildingToPlanScreen(new HashedString("HVAC"), GasTemperatureFilterConfig.ID);

            Debug.Log("Exit TemperatureFilterMod_GeneratedBuildings_LoadGeneratedBuildings.Prefix");
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class TemperatureFilterMod_Db_Initialize
    {
        private static void Prefix(Db __instance)
        {
            Debug.Log("Entry TemperatureFilterMod_Db_Initialize.Prefix");
            List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["LiquidTemperature"]);
            ls.Add(LiquidTemperatureFilterConfig.ID);
            Database.Techs.TECH_GROUPING["LiquidTemperature"] = ls.ToArray();

            ls = new List<string>(Database.Techs.TECH_GROUPING["HVAC"]);
            ls.Add(GasTemperatureFilterConfig.ID);
            Database.Techs.TECH_GROUPING["HVAC"] = ls.ToArray();

            Debug.Log("Exit TemperatureFilterMod_Db_Initialize.Prefix");
        }

    }
}
