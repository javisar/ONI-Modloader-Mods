using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace NoFixedTemp
{

    internal class NoFixedTemp_Utils
    {

        public static void ChangeMinTemperature(GameObject go)
        {
            ElementConverter[] elementConverters = go.GetComponents<ElementConverter>();
            if (elementConverters != null)
            {
                foreach (ElementConverter elementConverter in elementConverters)
                {
                    for (int i = 0; i < elementConverter.outputElements.Length; i++)
                    {                        
                        elementConverter.outputElements[i].minOutputTemperature = 0;
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(AirFilterConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_AirFilterConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(AirFilterConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }



    [HarmonyPatch(typeof(AlgaeDistilleryConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_AlgaeDistilleryConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(AlgaeDistilleryConfig __instance, GameObject go, Tag prefab_tag)
        {       
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }


    [HarmonyPatch(typeof(AlgaeHabitatConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_AlgaeHabitatConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(AlgaeHabitatConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }


    [HarmonyPatch(typeof(CO2ScrubberConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_CO2ScrubberConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(CO2ScrubberConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }


    [HarmonyPatch(typeof(CompostConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_CompostConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(CompostConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }

    [HarmonyPatch(typeof(DesalinatorConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_DesalinatorConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(AlgaeDistilleryConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }


    [HarmonyPatch(typeof(ElectrolyzerConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_ElectrolyzerConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(ElectrolyzerConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }


    [HarmonyPatch(typeof(EthanolDistilleryConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_EthanolDistilleryConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(EthanolDistilleryConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }


    [HarmonyPatch(typeof(FertilizerMakerConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_FertilizerMakerConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(FertilizerMakerConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }


    [HarmonyPatch(typeof(GourmetCookingStationConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_GourmetCookingStationConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(GourmetCookingStationConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }



    [HarmonyPatch(typeof(MineralDeoxidizerConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_MineralDeoxidizerConfigg_ConfigureBuildingTemplate
    {

        private static void Postfix(MineralDeoxidizerConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }


    [HarmonyPatch(typeof(OilRefineryConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_OilRefineryConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(OilRefineryConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }


    [HarmonyPatch(typeof(OilWellCapConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_OilWellCapConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(OilWellCapConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }


    [HarmonyPatch(typeof(OxyliteRefineryConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_OxyliteRefineryConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(OxyliteRefineryConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }

    /*
    [HarmonyPatch(typeof(PacuCleanerConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_PacuCleanerConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(PacuCleanerConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }
    */

    [HarmonyPatch(typeof(PolymerizerConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_PolymerizerConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(PolymerizerConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }



    [HarmonyPatch(typeof(RustDeoxidizerConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_RustDeoxidizerConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(RustDeoxidizerConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }


    [HarmonyPatch(typeof(ShowerConfig), "ConfigureBuildingTemplate")]
	internal class NoFixedTemp_ShowerConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(ShowerConfig __instance, GameObject go, Tag prefab_tag)
		{
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
	}
   
    [HarmonyPatch(typeof(WaterPurifierConfig), "ConfigureBuildingTemplate")]
    internal class NoFixedTemp_WaterPurifierConfig_ConfigureBuildingTemplate
    {

        private static void Postfix(WaterPurifierConfig __instance, GameObject go, Tag prefab_tag)
        {
            NoFixedTemp_Utils.ChangeMinTemperature(go);
        }
    }

}
