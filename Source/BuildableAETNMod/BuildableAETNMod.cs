using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace BuildableAETNMod
{

    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class Patches_GeneratedBuildings_LoadGeneratedBuildings
    {
        private static void Prefix()
        {
            Debug.Log(" === GeneratedBuildings.LoadGeneratedBuildings Prefix === " + MassiveHeatSinkConfig.ID);

			ModUtil.AddBuildingToPlanScreen("Utilities", MassiveHeatSinkConfig.ID);

			/*
            Strings.Add("STRINGS.BUILDINGS.PREFABS.MASSIVEHEATSINK.NAME", "AETN");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.MASSIVEHEATSINK.DESC", "");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.MASSIVEHEATSINK.EFFECT", "");
            */
			/*
            List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
            ls.Add(MassiveHeatSinkConfig.ID);
            TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(MassiveHeatSinkConfig.ID);
            */
			/*
            List<string> category = (List<string>) TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString) "Utilities").Equals(po.category)).data;
            category.Add(MassiveHeatSinkConfig.ID);
			*/



		}
        /*
        private static void Postfix()
        {

            Debug.Log(" === GeneratedBuildings.LoadGeneratedBuildings Postfix === " + MassiveHeatSinkConfig.ID);
            object obj = Activator.CreateInstance(typeof(MassiveHeatSinkConfig));
            BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
        }
        */
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class InverseElectrolyzerTechMod
    {
        private static void Prefix(Db __instance)
        {
            Debug.Log(" === Db.Initialize loaded === " + MassiveHeatSinkConfig.ID);
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["TemperatureModulation"]);
            ls.Add(MassiveHeatSinkConfig.ID);
            Database.Techs.TECH_GROUPING["TemperatureModulation"] = (string[])ls.ToArray();
            
        }
    }

    [HarmonyPatch(typeof(MassiveHeatSinkConfig), "CreateBuildingDef")]
    internal class MassiveHeatSinkConfigTechMod
    {
        private static void Prefix(MassiveHeatSinkConfig __instance)
        {
            Debug.Log(" === MassiveHeatSinkConfig.CreateBuildingDef loaded === " + MassiveHeatSinkConfig.ID);            

        }

        private static void Postfix(MassiveHeatSinkConfig __instance, ref BuildingDef __result)
        {
            Debug.Log(" === MassiveHeatSinkConfig.CreateBuildingDef loaded === " + MassiveHeatSinkConfig.ID);
            __result.ViewMode = OverlayModes.GasConduits.ID;
            __result.MaterialCategory = new string[] { "RefinedMetal"};
            __result.Mass = new float[] { 20000f };
        }
    }

    [HarmonyPatch(typeof(MassiveHeatSinkConfig), "DoPostConfigureComplete")]
    internal class MassiveHeatSinkConfigDoPostConfigureCompleteMod
    {
     
        private static void Postfix(MassiveHeatSinkConfig __instance, ref GameObject go)
        {
            Debug.Log(" === MassiveHeatSinkConfig.DoPostConfigureComplete loaded === " + MassiveHeatSinkConfig.ID);

            ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
            elementConverter.outputElements = new ElementConverter.OutputElement[] { };
        }
    }
}
