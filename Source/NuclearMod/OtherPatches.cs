using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Harmony;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using STRINGS;
using UnityEngine;
using System.ComponentModel;
using static Klei.AI.Disease;
using VoronoiTree;
using ProcGenGame;
using static ProcGenGame.TerrainCell;
using NuclearMod;

namespace UraniumElementMod
{
    
    [HarmonyPatch(typeof(GeneratedOre), "LoadGeneratedOre")]
    internal static class UraniumElementNameMod
    {
        private static bool Prefix()
        {
            Debug.Log(" === GeneratedOre.LoadGeneratedOre Prefix === ");
            /*
			Strings.Add("STRINGS.ELEMENTS.URANIUMORE.NAME", UI.FormatAsLink("UraniumOre", "URANIUMORE"));
			Strings.Add("STRINGS.ELEMENTS.URANIUMORE.DESC", "Uranium is a radioactive element.");
			Strings.Add("STRINGS.ELEMENTS.URANIUMORE.BUILD_DESC", "");
			*/
            /*
			Strings.Add("STRINGS.ELEMENTS."+UraniumOreMod.ID+".NAME", UI.FormatAsLink("UraniumOre", "URANIUMORE"));
			Strings.Add("STRINGS.ELEMENTS."+UraniumOreMod.ID+".DESC", "Uranium is a radioactive element.");
			Strings.Add("STRINGS.ELEMENTS."+UraniumOreMod.ID+".BUILD_DESC", "");
			*/
            /*
			Element element = new Element();
			element.id = (SimHashes)1111111001;
			element.name = Strings.Get("STRINGS.ELEMENTS." + "URANIUMORE" + ".NAME");
			element.nameUpperCase = element.name.ToUpper();
			ElementLoader.elements.Add(element);
			ElementLoader.elementTable[(int)element.id] = element;
			*/
            return true;
        }
    }


    [HarmonyPatch(typeof(App), "LoadScene")]
    internal static class UraniumElementAppLoadScene
    {
        private static bool Prefix(string scene_name)
        {
            Debug.Log(" === App.LoadScene Prefix === " + scene_name);
            return true;
        }
    }

    [HarmonyPatch(typeof(Assets), "OnPrefabInit")]
    internal static class UraniumElementAssetsOnPrefabInit
    {
        private static bool Prefix(Assets __instance)
        {
            Debug.Log(" === Assets.OnPrefabInit Prefix === ");
            return true;
        }
    }


    [HarmonyPatch(typeof(Klei.AI.Disease))]
    [HarmonyPatch(new Type[] { typeof(string), typeof(DiseaseType), typeof(Severity), typeof(float), typeof(List<InfectionVector>), typeof(float), typeof(byte), typeof(RangeInfo), typeof(RangeInfo), typeof(RangeInfo), typeof(RangeInfo) })]
    internal static class UraniumElement_Disease_ctor
    {
        private static bool Prefix(Klei.AI.Disease __instance, string id, DiseaseType type, Severity severity, float immune_attack_strength, List<InfectionVector> infection_vectors, float sickness_duration, byte strength, RangeInfo temperature_range, RangeInfo temperature_half_lives, RangeInfo pressure_range, RangeInfo pressure_half_lives)
        {
            Debug.Log(" === Disease.ctor Prefix === ");
            return true;
            /*
			//__instance.name = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".NAME");
			FieldInfo fi = AccessTools.Field(typeof(Klei.AI.Disease), "name");
			fi.SetValue(__instance, new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".NAME"));
			Debug.Log("1");
			__instance.id = id;
			__instance.diseaseType = type;
			__instance.severity = severity;
			__instance.immuneAttackStrength = immune_attack_strength;
			__instance.infectionVectors = infection_vectors;
			Debug.Log("2");
			//__instance.sicknessDuration = sickness_duration;
			fi = AccessTools.Field(typeof(Klei.AI.Disease), "sicknessDuration");
			fi.SetValue(__instance,  sickness_duration );
			Debug.Log("3");
			DiseaseVisualization.Info info = Assets.instance.DiseaseVisualization.GetInfo(id);
			__instance.overlayColour = info.overlayColour;
			__instance.temperatureRange = temperature_range;
			__instance.temperatureHalfLives = temperature_half_lives;
			__instance.pressureRange = pressure_range;
			__instance.pressureHalfLives = pressure_half_lives;
			Debug.Log("4");
			//__instance.descriptiveSymptoms = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".DESCRIPTIVE_SYMPTOMS");
			fi = AccessTools.Field(typeof(Klei.AI.Disease), "descriptiveSymptoms");
			fi.SetValue(__instance, new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".DESCRIPTIVE_SYMPTOMS"));
			Debug.Log("5");
			//__instance.PopulateElemGrowthInfo();
			MethodInfo mi = AccessTools.Method(typeof(Klei.AI.Disease), "PopulateElemGrowthInfo", new Type[] { });
			mi.Invoke(__instance, new object[] { });
			//__instance.ApplyRules();
			mi = AccessTools.Method(typeof(Klei.AI.Disease), "ApplyRules", new Type[] { });
			mi.Invoke(__instance, new object[] { });
			Debug.Log("6");
			string str = Strings.Get("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".LEGEND_HOVERTEXT").ToString();
			foreach (Descriptor qualitativeDescriptor in __instance.GetQualitativeDescriptors())
			{
				str = str + string.Empty + qualitativeDescriptor.IndentedText() + "\n";
			}
			Debug.Log("7");
			__instance.overlayLegendHovertext = str + DUPLICANTS.DISEASES.LEGEND_POSTAMBLE;
			Klei.AI.Attribute attribute = new Klei.AI.Attribute(id + "Min", "Minimum" + id.ToString(), string.Empty, string.Empty, 0f, Klei.AI.Attribute.Display.Normal, false);
			Klei.AI.Attribute attribute2 = new Klei.AI.Attribute(id + "Max", "Maximum" + id.ToString(), string.Empty, string.Empty, 1E+07f, Klei.AI.Attribute.Display.Normal, false);
			__instance.amountDeltaAttribute = new Klei.AI.Attribute(id + "Delta", id.ToString(), string.Empty, string.Empty, 0f, Klei.AI.Attribute.Display.Normal, false);
			__instance.amount = new Amount(id, id + " " + DUPLICANTS.DISEASES.GERMS, id + " " + DUPLICANTS.DISEASES.GERMS, attribute, attribute2, __instance.amountDeltaAttribute, false, Units.Flat, 0.01f, true);
			Debug.Log("8"+ Db.Get().Attributes);
			Db.Get().Attributes.Add(attribute);
			Debug.Log("8a");
			Db.Get().Attributes.Add(attribute2);
			Debug.Log("8b");
			Db.Get().Attributes.Add(__instance.amountDeltaAttribute);
			Debug.Log("9");
			__instance.cureSpeedBase = new Klei.AI.Attribute(id + "CureSpeed", false, Klei.AI.Attribute.Display.Normal, false, 0f);
			__instance.cureSpeedBase.BaseValue = 1f;
			__instance.cureSpeedBase.SetFormatter(new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None));
			Debug.Log("10");
			Db.Get().Attributes.Add(__instance.cureSpeedBase);
			Debug.Log("11");
			return false;
			*/
        }

    }

    /*
	[HarmonyPatch(typeof(Database.Diseases), "GetIndex", new Type[] { typeof(int)})]
	internal static class UraniumElement_Diseases_GetIndex
	{
		private static bool Prefix(Db __instance, ref byte __result, int hash)
		{
			Debug.Log(" === Diseases.GetIndex Prefix === ");
			if (hash == UraniumElement_Prop.Radio.id.GetHashCode())
			{
				__result = (byte)(Db.Get().Diseases.Count + 1);
				return false;
			}
			return true;
		}
		
	}

	[HarmonyPatch(typeof(Database.Diseases), "IsValidDiseaseID", new Type[] { typeof(int) })]
	internal static class UraniumElement_Diseases_GetIndex
	{
		private static bool Prefix(Db __instance, ref byte __result, int hash)
		{
			Debug.Log(" === Diseases.GetIndex Prefix === ");
			if (hash == UraniumElement_Prop.Radio.id.GetHashCode())
			{
				__result = (byte)(Db.Get().Diseases.Count + 1);
				return false;
			}
			return true;
		}

	}
	*/
    /*
	[HarmonyPatch(typeof(Database.Diseases))]
	[HarmonyPatch(new Type[] { typeof(ResourceSet) })]
	internal static class UraniumElement_Diseases_Ctor
	{		

		private static bool Prefix(ref Database.Diseases __result, ResourceSet parent)
		{
			Debug.Log(" === Diseases.Ctor Prefix === ");
			__result = new Database.Diseases2(parent);
			return true;
		}
	}
	*/

    /*
    [HarmonyPatch(typeof(CodexEntryGenerator), "GenerateDiseaseEntries")]
    internal static class UraniumElement_CodexEntryGenerator_GenerateDiseaseEntries
    {
        private static bool Prefix(ref Dictionary<string, CodexEntry> __result)
        {
            Debug.Log(" === CodexEntryGenerator.GenerateDiseaseEntries Postfix === ");
            Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
            foreach (Disease resource in Db.Get().Diseases.resources)
            {
                if (!resource.Disabled)
                {
                    Debug.Log(" === "+ resource.Id+" === ");
                    UraniumElement_Prop.Print(resource);
                    List<ContentContainer> list = new List<ContentContainer>();
                    //CodexEntryGenerator.GenerateTitleContainers(resource.Name, list);
                    MethodInfo mi1 = AccessTools.Method(typeof(CodexEntryGenerator), "GenerateTitleContainers", new Type[] { typeof(string), typeof(List<ContentContainer>)});
                    mi1.Invoke(null, new object[] { resource.Name, list });
                    //CodexEntryGenerator.GenerateDiseaseDescriptionContainers(resource, list);
                    MethodInfo mi2 = AccessTools.Method(typeof(CodexEntryGenerator), "GenerateDiseaseDescriptionContainers", new Type[] { typeof(Disease), typeof(List<ContentContainer>) });
                    mi2.Invoke(null, new object[] { resource, list });

                    CodexEntry codexEntry = new CodexEntry("DISEASE", list, resource.Name);
                    codexEntry.parentId = "DISEASE";
                    dictionary.Add(resource.Id, codexEntry);
                    CodexCache.AddEntry(resource.Id, codexEntry, null);
                }
            }
            __result = dictionary;
            return false;
        }
    }
    */

     /*
	[HarmonyPatch(typeof(ProcGenGame.TerrainCell), "ApplyBackground")]
	internal static class UraniumElement_TerrainCell_ApplyBackground
	{
		private static bool Prefix(ProcGenGame.TerrainCell __instance, Chunk world, SetValuesFunction SetValues, float temperatureMin, float temperatureRange, SeededRandom rnd)
		{
			Debug.Log(" === TerrainCell.ApplyBackground Prefix === ");

			float defaultFloat = WorldGen.Settings.GetDefaultFloat("CaveOverrideMaxValue");
			float defaultFloat2 = WorldGen.Settings.GetDefaultFloat("CaveOverrideSliverValue");
			Leaf leafForTerrainCell = WorldGen.GetLeafForTerrainCell(__instance);
			bool flag = leafForTerrainCell.tags.Contains(WorldGenTags.IgnoreCaveOverride);
			bool flag2 = leafForTerrainCell.tags.Contains(WorldGenTags.CaveVoidSliver);
			bool flag3 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToCentroid);
			bool flag4 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToCentroidInv);
			bool flag5 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToEdge);
			bool flag6 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToEdgeInv);
			bool flag7 = leafForTerrainCell.tags.Contains(WorldGenTags.DistFunctionPointCentroid);
			bool flag8 = leafForTerrainCell.tags.Contains(WorldGenTags.DistFunctionPointEdge);
			Sim.DiseaseCell diseaseCell = default(Sim.DiseaseCell);
			diseaseCell.diseaseIdx = 255;
			if (__instance.node.tags.Contains(WorldGenTags.Infected))
			{
				diseaseCell.diseaseIdx = (byte)rnd.RandomRange(0, WorldGen.diseaseIds.Count);
				Debug.Log(" === TerrainCell.ApplyBackground Prefix === "+ diseaseCell.diseaseIdx);
				Debug.Log(" === TerrainCell.ApplyBackground Prefix === " + "Infected:" + WorldGen.diseaseIds[diseaseCell.diseaseIdx]);
				__instance.node.tags.Add(new Tag("Infected:" + WorldGen.diseaseIds[diseaseCell.diseaseIdx]));
				diseaseCell.elementCount = rnd.RandomRange(10000, 1000000);
			}
			//foreach (Vector2I availableTerrainPoint in __instance.availableTerrainPoints)
			FieldInfo fi = AccessTools.Field(typeof(TerrainCell), "availableTerrainPoints");
			foreach (Vector2I availableTerrainPoint in (HashSet < Vector2I > )fi.GetValue(__instance)) 
			{
				Vector2I current = availableTerrainPoint;
				int num = Grid.XYToCell(current.x, current.y);
				float num2 = world.overrides[num];
				if (!flag && num2 >= 100f)
				{
					if (num2 >= 300f)
					{
						SetValues(num, WorldGen.voidElement, WorldGen.voidElement.defaultValues, Sim.DiseaseCell.Invalid);
					}
					else if (num2 >= 200f)
					{
						SetValues(num, WorldGen.unobtaniumElement, WorldGen.unobtaniumElement.defaultValues, Sim.DiseaseCell.Invalid);
					}
					else
					{
						SetValues(num, WorldGen.katairiteElement, WorldGen.katairiteElement.defaultValues, Sim.DiseaseCell.Invalid);
					}
				}
				else
				{
					float num3 = 1f;
					Vector2 vector = new Vector2((float)current.x, (float)current.y);
					if (flag3 || flag4)
					{
						float num4 = 15f;
						if (flag8)
						{
							float d = 0f;
							MathUtil.Pair<Vector2, Vector2> closestEdge = __instance.poly.GetClosestEdge(vector, ref d);
							Vector2 a = closestEdge.First + (closestEdge.Second - closestEdge.First) * d;
							num4 = Vector2.Distance(a, vector);
						}
						num3 = Vector2.Distance(__instance.poly.Centroid(), vector) / num4;
						num3 = Mathf.Max(0f, Mathf.Min(1f, num3));
						if (flag4)
						{
							num3 = 1f - num3;
						}
					}
					if (flag6 || flag5)
					{
						float d2 = 0f;
						MathUtil.Pair<Vector2, Vector2> closestEdge2 = __instance.poly.GetClosestEdge(vector, ref d2);
						Vector2 a2 = closestEdge2.First + (closestEdge2.Second - closestEdge2.First) * d2;
						float num5 = 15f;
						if (flag7)
						{
							num5 = Vector2.Distance(__instance.poly.Centroid(), vector);
						}
						num3 = Vector2.Distance(a2, vector) / num5;
						num3 = Mathf.Max(0f, Mathf.Min(1f, num3));
						if (flag6)
						{
							num3 = 1f - num3;
						}
					}
					Element element = default(Element);
					Sim.PhysicsData defaultValues = default(Sim.PhysicsData);
					Sim.DiseaseCell dc = default(Sim.DiseaseCell);
					WorldGen.GetElementForBiome(world, __instance.node.type, current, out element, out defaultValues, out dc, num3);
					if (!element.IsVacuum && element.id != SimHashes.Katairite && element.id != SimHashes.Unobtanium)
					{
						if (element.lowTempTransition != null && temperatureMin < element.lowTemp)
						{
							temperatureMin = element.lowTemp + 20f;
						}
						defaultValues.temperature = temperatureMin + world.heatOffset[num] * temperatureRange;
					}
					if (element.IsSolid)
					{
						//defaultValues.mass = __instance.GetDensityMassForCell(world, num, defaultValues.mass);
						MethodInfo mi1 = AccessTools.Method(typeof(TerrainCell), "GetDensityMassForCell");
						defaultValues.mass = (float)mi1.Invoke(__instance, new object[] { world, num, defaultValues.mass });
						if (!flag && num2 > defaultFloat && num2 < 100f)
						{
							element = ((!flag2 || !(num2 > defaultFloat2)) ? WorldGen.vacuumElement : WorldGen.voidElement);
							defaultValues = element.defaultValues;
						}
					}
					if (dc.diseaseIdx == 255)
					{
						dc = diseaseCell;
					}
					SetValues(num, element, defaultValues, dc);
				}
			}
			MethodInfo mi = AccessTools.Method(typeof(TerrainCell), "HandleSprinkleOfElement");
			if (__instance.node.tags.Contains(WorldGenTags.SprinkleOfOxyRock))
			{
				//__instance.HandleSprinkleOfElement(WorldGenTags.SprinkleOfOxyRock, world, SetValues, temperatureMin, temperatureRange, rnd);
				mi.Invoke(__instance, new object[] { WorldGenTags.SprinkleOfOxyRock, world, SetValues, temperatureMin, temperatureRange, rnd });
			}
			if (__instance.node.tags.Contains(WorldGenTags.SprinkleOfMetal))
			{
				//__instance.HandleSprinkleOfElement(WorldGenTags.SprinkleOfMetal, world, SetValues, temperatureMin, temperatureRange, rnd);
				mi.Invoke(__instance, new object[] { WorldGenTags.SprinkleOfMetal, world, SetValues, temperatureMin, temperatureRange, rnd });
			}
			return false;
		}

	}
    */

}
