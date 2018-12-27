using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace StirlingGeneratorMod
{

	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class StirlingGenerator_GeneratedBuildings_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Debug.Log(" === StirlingGenerator_GeneratedBuildings_LoadGeneratedBuildings Prefix === " + StirlingGeneratorConfig.ID);

			Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZER.NAME", "Stirling Generator");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZER.DESC", "Generates power based in the temperaure difference of two liquids");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.INVERSEELECTROLYZER.EFFECT", "Converts " + STRINGS.UI.FormatAsLink("Oxygen", "OXYGEN") + " and " + STRINGS.UI.FormatAsLink("Hydrogen", "HYDROGEN") + " into " + STRINGS.UI.FormatAsLink("Water", "WATER") + ".\n\nGets oxygen from the environment.");

			ModUtil.AddBuildingToPlanScreen("Utilities", StirlingGeneratorConfig.ID);



		}
	
	}

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class StirlingGenerator_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			Debug.Log(" === Database.Techs StirlingGenerator_Db_Initialize loaded === " + StirlingGeneratorConfig.ID);
			List<string> ls1 = new List<string>((string[])Database.Techs.TECH_GROUPING["Combustion"]);
			ls1.Add(StirlingGeneratorConfig.ID);
			Database.Techs.TECH_GROUPING["Combustion"] = (string[])ls1.ToArray();

		}
	}
	
}
