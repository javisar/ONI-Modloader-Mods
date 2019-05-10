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
			//Debug.Log(" === GeneratedBuildings Prefix === " + InverseElectrolyzerConfig.ID);

			Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZER.NAME", "Inverse Electrolyzer");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZER.DESC", "Hydrogen and oxygen goes in, water comes out");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZER.EFFECT", "Converts " + STRINGS.UI.FormatAsLink("Oxygen", "OXYGEN") + " and " + STRINGS.UI.FormatAsLink("Hydrogen", "HYDROGEN") + " into " + STRINGS.UI.FormatAsLink("Water", "WATER") + ".\n\nGets oxygen from an input.");

			ModUtil.AddBuildingToPlanScreen("Utilities", InverseElectrolyzerConfig.ID);

		}
   
    }

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class InverseElectrolyzerTechMod
	{
		private static void Prefix(Db __instance)
		{			
			//Debug.Log(" === Database.Techs loaded === " + InverseElectrolyzerConfig.ID);
			List<string> ls2 = new List<string>((string[])Database.Techs.TECH_GROUPING["Combustion"]);
			ls2.Add(InverseElectrolyzerConfig.ID);
			Database.Techs.TECH_GROUPING["Combustion"] = (string[])ls2.ToArray();
		}
	}	
	
}
