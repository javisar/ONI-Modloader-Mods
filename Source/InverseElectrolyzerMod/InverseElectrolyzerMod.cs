using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;


namespace InverseElectrolyzerMod
{    
    
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class InverseElectrolyzerMod
	{
        private static void Prefix()
        {
            Debug.Log(" === GeneratedBuildings Prefix === "+ InverseElectrolyzerConfig.ID);			

			Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZER.NAME", "Burner 1IN");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZER.DESC", "Hydrogen and oxygen goes in, water comes out");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZER.EFFECT", "Converts " + STRINGS.UI.FormatAsLink("Oxygen", "OXYGEN") + " and " + STRINGS.UI.FormatAsLink("Hydrogen", "HYDROGEN") + " into " + STRINGS.UI.FormatAsLink("Water", "WATER") + ".\n\nGets oxygen from the environment.");

			ModUtil.AddBuildingToPlanScreen("Utilities", InverseElectrolyzerConfig.ID);


			Debug.Log(" === GeneratedBuildings Prefix === " + InverseElectrolyzerAltConfig.ID);

			Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZERALT.NAME", "Burner 2IN");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZERALT.DESC", "Hydrogen and oxygen goes in, water comes out");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZERALT.EFFECT", "Converts " + STRINGS.UI.FormatAsLink("Oxygen", "OXYGEN") + " and " + STRINGS.UI.FormatAsLink("Hydrogen", "HYDROGEN") + " into " + STRINGS.UI.FormatAsLink("Water", "WATER") + ".\n\nGets oxygen from an input.");

			ModUtil.AddBuildingToPlanScreen("Utilities", InverseElectrolyzerAltConfig.ID);

			/*
            List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
            ls.Add(InverseElectrolyzerConfig.ID);            
            TUNING.BUILDINGS.PLANORDER[10].data = (string[]) ls.ToArray();

            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(InverseElectrolyzerConfig.ID);
            */
			/*
            List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => po.category == (HashedString) "Utilities").data;
            category.Add(InverseElectrolyzerConfig.ID);
			*/

		}
        /*
        private static void Postfix()
        {
            
            Debug.Log(" === GeneratedBuildings Postfix === " + InverseElectrolyzerConfig.ID);
            object obj = Activator.CreateInstance(typeof(InverseElectrolyzerConfig));
            BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
        }
        */
    }

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class InverseElectrolyzerTechMod
	{
		private static void Prefix(Db __instance)
		{
			Debug.Log(" === Database.Techs loaded === " + InverseElectrolyzerConfig.ID);
			List<string> ls1 = new List<string>((string[])Database.Techs.TECH_GROUPING["Combustion"]);
			ls1.Add(InverseElectrolyzerConfig.ID);
			Database.Techs.TECH_GROUPING["Combustion"] = (string[])ls1.ToArray();

			Debug.Log(" === Database.Techs loaded === " + InverseElectrolyzerAltConfig.ID);
			List<string> ls2 = new List<string>((string[])Database.Techs.TECH_GROUPING["Combustion"]);
			ls2.Add(InverseElectrolyzerAltConfig.ID);
			Database.Techs.TECH_GROUPING["Combustion"] = (string[])ls2.ToArray();

		}
	}
	/*
	[HarmonyPatch(typeof(ConduitConsumer), "Consume", new Type[] { typeof(float), typeof(ConduitFlow)})]
	internal class ConduitConsumerMod
	{
		private static void Prefix(ConduitConsumer __instance, float dt, ConduitFlow conduit_mgr)
		{
			
			if (__instance.IsConnected && __instance.capacityTag == InverseElectrolyzerConfig.gasTag)
			{
				ConduitFlow.ConduitContents contents = conduit_mgr.GetContents((int)AccessTools.Field(typeof(ConduitConsumer), "utilityCell").GetValue(__instance));
				Element element = ElementLoader.FindElementByHash(contents.element);
				if (element.tag == ElementLoader.FindElementByHash(SimHashes.Oxygen).tag)
				{
					__instance.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
				}
				else if (element.tag == ElementLoader.FindElementByHash(SimHashes.Hydrogen).tag)
				{
					__instance.capacityTag = ElementLoader.FindElementByHash(SimHashes.Hydrogen).tag;
				}
				else
				{
					__instance.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
				}
			}
		}

		private static void Postfix(ConduitConsumer __instance)
		{
			__instance.capacityTag = InverseElectrolyzerConfig.gasTag;
		}
	}
	*/
	
}
