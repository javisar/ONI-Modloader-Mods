using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace NoFixedTempMod
{
	[HarmonyPatch(typeof(AlgaeDistilleryConfig), "ConfigureBuildingTemplate")]
	internal class NoFixedTemp_AlgaeDistilleryConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(AlgaeDistilleryConfig __instance, GameObject go, Tag prefab_tag)
		{
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.outputElements = new ElementConverter.OutputElement[2]
			{
                //new ElementConverter.OutputElement(0.2f, SimHashes.Algae, 303.15f, true, 0f, 1f, false, 1f, 255, 0),
			    //new ElementConverter.OutputElement(0.400000036f, SimHashes.DirtyWater, 303.15f, true, 0f, 0.5f, false, 1f, 255, 0)

                new ElementConverter.OutputElement(0.2f, SimHashes.Algae, 0f, true, 0f, 1f, true, 1f, 255, 0),
				new ElementConverter.OutputElement(0.4f, SimHashes.DirtyWater, 0f, true, 0f, 0.5f, true, 1f, 255, 0)
			};
		}
	}

    /*
	[HarmonyPatch(typeof(AlgaeHabitatConfig), "ConfigureBuildingTemplate")]
	internal class NoFixedTemp_AlgaeHabitatConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(AlgaeHabitatConfig __instance, GameObject go, Tag prefab_tag)
		{
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.outputElements = new ElementConverter.OutputElement[1]
			{
                //new ElementConverter.OutputElement(0.0400000028f, SimHashes.Oxygen, 303.15f, false, 0f, 1f, false, 1f, 255, 0)
                new ElementConverter.OutputElement(0.0400000028f, SimHashes.Oxygen, 0f, false, 0f, 1f, true, 1f, 255, 0)
			};
			ElementConverter elementConverter2 = go.AddComponent<ElementConverter>();
			elementConverter2.outputElements = new ElementConverter.OutputElement[1]
			{
                //new ElementConverter.OutputElement(0.290333331f, SimHashes.DirtyWater, 303.15f, true, 0f, 1f, false, 1f, 255, 0)
                new ElementConverter.OutputElement(0.290333331f, SimHashes.DirtyWater, 0f, true, 0f, 1f, true, 1f, 255, 0)
			};
		}
	}
    */

	[HarmonyPatch(typeof(CO2ScrubberConfig), "ConfigureBuildingTemplate")]
	internal class NoFixedTemp_CO2ScrubberConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(CO2ScrubberConfig __instance, GameObject go, Tag prefab_tag)
		{
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.outputElements = new ElementConverter.OutputElement[1]
			{
                //new ElementConverter.OutputElement(1f, SimHashes.DirtyWater, 313.15f, true, 0f, 0.5f, false, 1f, 255, 0)
                new ElementConverter.OutputElement(1f, SimHashes.DirtyWater, 0f, true, 0f, 0.5f, true, 1f, 255, 0)
			};
		}
	}

	[HarmonyPatch(typeof(CompostConfig), "ConfigureBuildingTemplate")]
	internal class NoFixedTemp_CompostConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(CompostConfig __instance, GameObject go, Tag prefab_tag)
		{
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.outputElements = new ElementConverter.OutputElement[1]
			{
                //new ElementConverter.OutputElement(0.1f, SimHashes.Dirt, 348.15f, true, 0f, 0.5f, false, 1f, 255, 0)
				new ElementConverter.OutputElement(0.1f, SimHashes.Dirt, 0f, true, 0f, 0.5f, true, 1f, 255, 0)
			};
		}
	}

	[HarmonyPatch(typeof(ElectrolyzerConfig), "ConfigureBuildingTemplate")]
	internal class NoFixedTemp_ElectrolyzerConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(ElectrolyzerConfig __instance, GameObject go, Tag prefab_tag)
		{
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.outputElements = new ElementConverter.OutputElement[2]
			{
                //new ElementConverter.OutputElement(0.888f, SimHashes.Oxygen, 343.15f, false, 0f, 1f, false, 1f, 255, 0),
			    //new ElementConverter.OutputElement(0.111999989f, SimHashes.Hydrogen, 343.15f, false, 0f, 1f, false, 1f, 255, 0)

                new ElementConverter.OutputElement(0.888f, SimHashes.Oxygen, 0f, false, 0f, 1f, true, 1f, 255, 0),
				new ElementConverter.OutputElement(0.111999989f, SimHashes.Hydrogen, 0f, false, 0f, 1f, true, 1f, 255, 0)
			};
		}
	}

	[HarmonyPatch(typeof(FertilizerMakerConfig), "ConfigureBuildingTemplate")]
	internal class NoFixedTemp_FertilizerMakerConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(FertilizerMakerConfig __instance, GameObject go, Tag prefab_tag)
		{
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.outputElements = new ElementConverter.OutputElement[1]
			{
                //new ElementConverter.OutputElement(0.12f, SimHashes.Fertilizer, 323.15f, true, 0f, 0.5f, false, 1f, 255, 0)
                new ElementConverter.OutputElement(0.12f, SimHashes.Fertilizer, 0f, true, 0f, 0.5f, true, 1f, 255, 0)
			};
		}
	}

	[HarmonyPatch(typeof(MineralDeoxidizerConfig), "ConfigureBuildingTemplate")]
	internal class NoFixedTemp_MineralDeoxidizerConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(MineralDeoxidizerConfig __instance, GameObject go, Tag prefab_tag)
		{
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.outputElements = new ElementConverter.OutputElement[1]
			{
                //new ElementConverter.OutputElement(0.5f, SimHashes.Oxygen, 303.15f, false, 0f, 1f, false, 1f, 255, 0)
				new ElementConverter.OutputElement(0.5f, SimHashes.Oxygen, 0f, false, 0f, 1f, true, 1f, 255, 0)
			};
		}
	}

	[HarmonyPatch(typeof(OilRefineryConfig), "ConfigureBuildingTemplate")]
	internal class NoFixedTemp_OilRefineryConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(OilRefineryConfig __instance, GameObject go, Tag prefab_tag)
		{
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.outputElements = new ElementConverter.OutputElement[2]
			{
                //new ElementConverter.OutputElement(5f, SimHashes.Petroleum, 348.15f, true, 0f, 1f, false, 1f, 255, 0),
			    //new ElementConverter.OutputElement(0.09f, SimHashes.Methane, 348.15f, false, 0f, 3f, false, 1f, 255, 0)

                new ElementConverter.OutputElement(5f, SimHashes.Petroleum, 0f, true, 0f, 1f, true, 1f, 255, 0),
				new ElementConverter.OutputElement(0.09f, SimHashes.Methane, 0f, false, 0f, 3f, true, 1f, 255, 0)
			};
		}
	}

	[HarmonyPatch(typeof(OilWellCapConfig), "ConfigureBuildingTemplate")]
	internal class NoFixedTemp_OilWellCapConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(OilWellCapConfig __instance, GameObject go, Tag prefab_tag)
		{
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.outputElements = new ElementConverter.OutputElement[1]
			{
                //new ElementConverter.OutputElement(3.33333325f, SimHashes.CrudeOil, 363.15f, false, 2f, 1.5f, false, 0f, 255, 0)
				new ElementConverter.OutputElement(3.33333325f, SimHashes.CrudeOil, 0f, false, 2f, 1.5f, true, 0f, 255, 0)
			};
		}
	}
    /*
	[HarmonyPatch(typeof(PacuCleanerConfig), "ConfigureBuildingTemplate")]
	internal class NoFixedTemp_PacuCleanerConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(PacuCleanerConfig __instance, GameObject go, Tag prefab_tag)
		{
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.outputElements = new ElementConverter.OutputElement[1]
			{
               
                new ElementConverter.OutputElement(PacuCleanerConfig.POLLUTED_WATER_CONVERTED_PER_CYCLE / 600f, SimHashes.Water, 0f, true, 0f, 0.5f, true, 1f, 255, 0)
			};
		}
	}
    */
	[HarmonyPatch(typeof(PolymerizerConfig), "ConfigureBuildingTemplate")]
	internal class NoFixedTemp_PolymerizerConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(PolymerizerConfig __instance, GameObject go, Tag prefab_tag)
		{
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.outputElements = new ElementConverter.OutputElement[3]
			{
                
                //new ElementConverter.OutputElement(0.5f, SimHashes.Polypropylene, 348.15f, true, 0f, 0.5f, false, 1f, 255, 0),
			    //new ElementConverter.OutputElement(0.008333334f, SimHashes.Steam, 473.15f, true, 0f, 0.5f, false, 1f, 255, 0),
			    //new ElementConverter.OutputElement(0.008333334f, SimHashes.CarbonDioxide, 423.15f, true, 0f, 0.5f, false, 1f, 255, 0)
                
				new ElementConverter.OutputElement(0.5f, SimHashes.Polypropylene, 0f, true, 0f, 0.5f, true, 1f, 255, 0),
				new ElementConverter.OutputElement(0.008333334f, SimHashes.Steam, 0f, true, 0f, 0.5f, true, 1f, 255, 0),
				new ElementConverter.OutputElement(0.008333334f, SimHashes.CarbonDioxide, 0f, true, 0f, 0.5f, true, 1f, 255, 0)
			};
		}
	}
    /*
	[HarmonyPatch(typeof(ShowerConfig), "ConfigureBuildingTemplate")]
	internal class NoFixedTemp_ShowerConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(ShowerConfig __instance, GameObject go, Tag prefab_tag)
		{
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.outputElements = new ElementConverter.OutputElement[1]
			{
                
                //new ElementConverter.OutputElement(1f, SimHashes.DirtyWater, 0f, true, 0f, 0.5f, true, 1f, 255, 0)
                 
				new ElementConverter.OutputElement(1f, SimHashes.DirtyWater, 0f, true, 0f, 0.5f, true, 1f, 255, 0)
			};
		}
	}
    */
    [HarmonyPatch(typeof(WaterPurifierConfig), "ConfigureBuildingTemplate")]
	internal class NoFixedTemp_SWaterPurifierConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(WaterPurifierConfig __instance, GameObject go, Tag prefab_tag)
		{
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.outputElements = new ElementConverter.OutputElement[2]
			{
                
                //new ElementConverter.OutputElement(5f, SimHashes.Water, 313.15f, true, 0f, 0.5f, false, 0.75f, 255, 0),
			    //new ElementConverter.OutputElement(0.2f, SimHashes.ToxicSand, 313.15f, true, 0f, 0.5f, false, 0.25f, 255, 0)
                
				new ElementConverter.OutputElement(5f, SimHashes.Water, 0f, true, 0f, 0.5f, true, 0.75f, 255, 0),
				new ElementConverter.OutputElement(0.2f, SimHashes.ToxicSand, 0f, true, 0f, 0.5f, true, 0.25f, 255, 0)                
			};
		}
	}
}
