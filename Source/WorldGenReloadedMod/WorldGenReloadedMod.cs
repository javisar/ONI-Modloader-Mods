using Harmony;
using Klei;
using ProcGen;
using ProcGen.Noise;
using ProcGenGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WorlGenReloadedMod
{
	[HarmonyPatch(typeof(WorldGen), "PlaceTemplateSpawners")]
	internal class WorlGenReloadedMod_WorldGen_PlaceTemplateSpawners
	{

		private static void Postfix(Vector2I position, TemplateContainer template)
		{
			Debug.Log(" === WorlGenReloadedMod_WorldGen_PlaceTemplateSpawners Postfix === ");
			Debug.Log(position);
			Debug.Log(template.name);
			if (template.otherEntities != null)
				Debug.Log(template.otherEntities.Count);
			if (template.info != null && template.info.tags != null && template.info.tags.Length > 0)
				Debug.Log(template.info.tags[0]);
		}
	}

	[HarmonyPatch(typeof(TemplateCache), "CollectBaseTemplateAssets")]
	internal class WorlGenReloadedMod_TemplateCache_CollectBaseTemplateAssets
	{

		private static void Postfix(List<TemplateContainer> __result)
		{
			Debug.Log(" === WorlGenReloadedMod_TemplateCache_CollectBaseTemplateAssets Postfix === ");
			foreach (TemplateContainer template in __result)
			{
				Debug.Log(template.name);
			}
		}
	}

	[HarmonyPatch(typeof(ProcGen.World), "LoadZones")]
    internal class WorlGenReloadedMod_World_LoadZones
    {
        //private static FieldInfo ZonesF = AccessTools.Field(typeof(ProcGen.World), "Zones");
        //private static FieldInfo ZoneFilesF = AccessTools.Field(typeof(ProcGen.World), "ZoneFiles");
        private static  FieldInfo ZoneLookupTableF = AccessTools.Field(typeof(ProcGen.World), "ZoneLookupTable");

        private static bool Prefix(ProcGen.World __instance, NoiseTreeFiles noise, string path) {
            Debug.Log(" === WorlGenReloadedMod_World_LoadZones Prefix === ");
            Debug.Log(path);

            foreach (WeightedName zoneFile in __instance.ZoneFiles)
            {
                SubWorld subWorld = null;
                string text = WorldGenSettings.GetSimpleName(zoneFile.name);
                Debug.Log(text);
                



                if (zoneFile.overrideName != null && zoneFile.overrideName.Length > 0)
                {
                    text = zoneFile.overrideName;
                }
                //if (!ZoneLookupTable.ContainsKey(text))
                if (!((Dictionary < string, SubWorld > )ZoneLookupTableF.GetValue(__instance)).ContainsKey(text))
                {
                    SubWorldFile subWorldFile = YamlIO<SubWorldFile>.LoadFile(path + zoneFile.name + ".yaml");
                    if (subWorldFile != null)
                    {
                        subWorld = subWorldFile.zone;
						//
						if (text.Equals("Space") || text.Equals("Bottom")
							|| text.Equals("Surface") || text.Equals("Impenetrable")
							|| text.Equals("OilField") || text.Equals("Default")
							|| text.Equals("StartWorld") || text.Equals("TheVoid"))
						{
						}
						else
						{
							subWorld.featureTemplates["feature_geyser_generic"] = 3;

							KeyValuePair<string, string[]> leftOne = new KeyValuePair<string, string[]>();

							foreach (KeyValuePair<string, string[]> poi in subWorld.pointsOfInterest)
							{
								Debug.Log("[] " + poi.Key);
								foreach (string po in poi.Value)
								{
									Debug.Log("[] " + po);
									if (!po.Contains("geyser"))
									{
										leftOne = poi;
									}

								}
							}

							subWorld.pointsOfInterest.Clear();
							//subWorld.pointsOfInterest.Add(leftOne);
							/*
							subWorld.pointsOfInterest["geysers_a"] = new string[] { "poi_jungle_geyser_steam" };
							subWorld.pointsOfInterest["geysers_b"] = new string[] { "poi_jungle_geyser_steam" };
							subWorld.pointsOfInterest["geysers_c"] = new string[] { "poi_jungle_geyser_steam" };
							subWorld.pointsOfInterest["geysers_d"] = new string[] { "poi_jungle_geyser_steam" };
							subWorld.pointsOfInterest["geysers_e"] = new string[] { "poi_jungle_geyser_steam" };
							*/
						}
						//
						subWorld.name = text;
                        subWorld.pdWeight = zoneFile.weight;
                        //ZoneLookupTable[text] = subWorld;
                        ((Dictionary<string, SubWorld>)ZoneLookupTableF.GetValue(__instance))[text] = subWorld;
                        noise.LoadTree(subWorld.biomeNoise, path);
                        noise.LoadTree(subWorld.densityNoise, path);
                        noise.LoadTree(subWorld.overrideNoise, path);
                    }
                    else
                    {
                        Debug.LogWarning("WorldGen: Attempting to load zone: " + zoneFile.name + " failed");
                    }
                }
                else
                {
                    subWorld = ((Dictionary<string, SubWorld>)ZoneLookupTableF.GetValue(__instance))[text];
                }
                __instance.Zones[text] = subWorld;
            }
            return false;
        }

    }
}
