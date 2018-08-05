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
        private static bool Prefix(string id, ref string[] construction_materials)
        {
            Debug.Log(" === SculptureReloadedMod_BuildingTemplates_CreateBuildingDef Prefix === ");
            if (id == "IceSculpture")
            {
                //List<string> cm = new List<string>(TUNING.MATERIALS.);
                //cm.Add("Ice");
                // construction_materials = (string[])cm.ToArray
                construction_materials = new String[1]
                {
                    "SolidOxygen"
                };
            }
            return true;

        }


        [HarmonyPatch(typeof(ElementLoader), "SetupElementsTable")]
        internal static class SculptureReloadedMod_ElementLoader_SetupElementsTable
        {
            private static bool Prefix()
            {
                Debug.Log(" === SculptureReloadedMod_ElementLoader_SetupElementsTable Prefix === ");
                /*
                Strings.Add("STRINGS.ELEMENTS.URANIUMORE.NAME", UI.FormatAsLink("Uranium Ore", "URANIUMORE"));
                Strings.Add("STRINGS.ELEMENTS.URANIUMORE.DESC", "Uranium is a radioactive element.");
                Strings.Add("STRINGS.ELEMENTS.URANIUMORE.BUILD_DESC", "");
                */

                return true;
            }
        }


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
    }
}
