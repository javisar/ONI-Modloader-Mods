using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ElementLoader;

namespace SculptureReloadedMod
{
    [HarmonyPatch(typeof(BuildingTemplates), "CreateBuildingDef")]
    internal class SculptureReloadedMod_BuildingTemplates_CreateBuildingDef
    {

		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		internal class SculptureReloadedMod_GeneratedBuildings_LoadGeneratedBuildings
		{
			private static void Prefix()
			{
				Debug.Log(" === SculptureReloadedMod_GeneratedBuildings_LoadGeneratedBuildings Prefix === " + SculptureReloadedConfig.ID);
				Strings.Add("STRINGS.BUILDINGS.PREFABS.SCULPTURERELOADED.NAME", "Sculpture");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.SCULPTURERELOADED.DESC", "");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.SCULPTURERELOADED.EFFECT", "");

				List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[8].data);
				ls.Add(SculptureReloadedConfig.ID);
				TUNING.BUILDINGS.PLANORDER[8].data = (string[])ls.ToArray();

				TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(SculptureReloadedConfig.ID);


			}
			private static void Postfix()
			{

				Debug.Log(" === GeneratedBuildings.LoadGeneratedBuildings Postfix === " + SculptureReloadedConfig.ID);
				object obj = Activator.CreateInstance(typeof(SculptureReloadedConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		internal class SculptureReloadedMod_Db_Initialize
		{
			private static void Prefix(Db __instance)
			{
				Debug.Log(" === SculptureReloadedMod_Db_Initialize Prefix === " + SculptureReloadedConfig.ID);
				List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["Artistry"]);
				ls.Add(SculptureReloadedConfig.ID);
				Database.Techs.TECH_GROUPING["Artistry"] = (string[])ls.ToArray();
			}
		}

		/*
        private static bool Prefix(string id, ref string[] construction_materials)
        {
            Debug.Log(" === SculptureReloadedMod_BuildingTemplates_CreateBuildingDef Prefix === ");
            if (id == "IceSculpture")
            {
				construction_materials = TUNING.MATERIALS.ANY_BUILDABLE;
			}
            return true;

        }
		*/



		/*
        [HarmonyPatch(typeof(ElementLoader), "Load")]
        internal static class SculptureReloadedMod_ElementLoader_Load
        {
            private static bool Prefix(ref Hashtable substanceList, ref SolidEntry[] solid_entries, ref LiquidEntry[] liquid_entries, ref GasEntry[] gas_entries, ref SubstanceTable substanceTable)
            {
                Debug.Log(" === SculptureReloadedMod_ElementLoader_Load Prefix === ");

                //"UraniumOre","0.129","4","1","1","1","0.9","1808","MoltenIron","","","242.15","800","1840","3","25","159.6882","Metal","Ore | BuildableAny","0","# U02 - Uranium Ore, advanced smelter can get the O2","",
                //string line = "UraniumOre,0.129,4,1,1,1,0.9,1808,MoltenIron,,,242.15,800,1840,3,25,159.6882,Metal,Ore | BuildableAny,0,\"# U02 - Uranium Ore, advanced smelter can get the O2\"";
                //textSolid += "\n" + line;
                //solid_entries = textSolid.Replace("1,1,2,50,,,0", "1,1,2,50,Liquifiable,IceOre,0");
                foreach (SolidEntry solid_entry in solid_entries)
                {
                    if (solid_entry.Id == "SolidOxygen")
                    {
                        solid_entry.materialCategory = "Liquifiable";
                        solid_entry.tags = solid_entry.tags + " | IceOre";
                    }
                }
                return true;
            }
        }
		*/
	}
}
