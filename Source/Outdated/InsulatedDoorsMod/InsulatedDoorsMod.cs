using Harmony;
using System;
using System.Collections.Generic;


namespace InsulatedDoorsMod
{    
    
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class InsulatedDoorsMod_GeneratedBuildings_LoadGeneratedBuildings
	{
        private static void Prefix()
        {
            Debug.Log(" === InsulatedDoorsMod_GeneratedBuildings_LoadGeneratedBuildings Prefix === " + InsulatedPressureDoorConfig.ID);
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDPRESSUREDOOR.NAME", "Insulated Mechanized Airlock");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDPRESSUREDOOR.DESC", "Insulated Mechanized airlocks have the same function as other doors, but open and close more quickly.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.INSULATEDPRESSUREDOOR.EFFECT", "Blocks <style=\"liquid\">Liquid</style> and <style=\"gas\">Gas</style> flow, maintaining pressure between areas.\n\nSets Duplicant Access Permissions for area restriction.\n\nFunctions as a Manual Airlock when no <style=\"power\">Power</style> is available.");

            List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[0].data);
            ls.Add(InsulatedPressureDoorConfig.ID);            
            TUNING.BUILDINGS.PLANORDER[0].data = (string[]) ls.ToArray();

            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(InsulatedPressureDoorConfig.ID);

        }
        private static void Postfix()
        {
            
            Debug.Log(" === InsulatedDoorsMod_GeneratedBuildings_LoadGeneratedBuildings Postfix === " + InsulatedPressureDoorConfig.ID);
            object obj = Activator.CreateInstance(typeof(InsulatedPressureDoorConfig));
            BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
        }
    }

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class InsulatedDoorsMod_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			Debug.Log(" === InsulatedDoorsMod_Db_Initialize loaded === " + InsulatedPressureDoorConfig.ID);
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["TemperatureModulation"]);
			ls.Add(InsulatedPressureDoorConfig.ID);
			Database.Techs.TECH_GROUPING["TemperatureModulation"] = (string[])ls.ToArray();

			//Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
		}
	}
	/*
	[HarmonyPatch(typeof(BuildingTemplates), "CreateBuildingDef")]
	internal class InsulatedDoorsMod_BuildingTemplates_CreateBuildingDef
	{
		private static void Prefix(string id, ref string[] construction_materials)
		{
			Debug.Log(" === InsulatedDoorsMod_BuildingTemplates_CreateBuildingDef Prefix === ");
			if (	id == "Door"
				||	id == "PressureDoor"
				||	id == "ManualPressureDoor"
				||	id == "BunkerDoor"
			)
			{
				construction_materials = TUNING.MATERIALS.ANY_BUILDABLE;
			}
		}
	}
	*/

}
