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
using static ElementLoader;

namespace UraniumElementMod
{

    internal static class UraniumElement_Prop
    {
        public static Type MyEnum = null;

        //public static Disease Radio = null;
    }


    [HarmonyPatch(typeof(Db), "Initialize")]
    internal static class UraniumElement_Db_Initialize
    {
        private static bool Prefix(Db __instance)
        {
            Debug.Log(" === Db.Initialize Prefix === ");
            //UraniumElement_Prop.Radio = new Radioactivity();
            //var diseases = new Database.Diseases(((ModifierSet)__instance).Root);
            return true;
        }

        private static void Postfix(Db __instance)
        {
            Debug.Log(" === Db.Initialize Postfix === ");
            //var diseases = new Database.Diseases(((ModifierSet)__instance).Root);
            //var diseases = new Database.Diseases(((ModifierSet)__instance).Root);
            //Database.Diseases.
            /*
            Db.Get().Diseases.Add(new Radioactivity());
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["Combustion"]);
            ls.Add(InverseElectrolyzerConfig.ID);
            Database.Techs.TECH_GROUPING["Combustion"] = (string[])ls.ToArray();
            */
            //return true;
            //__instance.Diseases = new Database.Diseases(((ModifierSet)__instance).Root);
        }
    }


    [HarmonyPatch(typeof(InitializeCheck), "Awake")]
	internal static class UraniumElement_InitializeCheck_Awake
	{
		private static bool Prefix()
		{
			Debug.Log(" === InitializeCheck.Awake Prefix === ");

            /*
			Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.NAME", UI.FormatAsLink("Radioactive", "RADIOACTIVE"));       
            Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.DESCRIPTION", "\nThis Duplicant's chest congestion is making it difficult to breathe");
            Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.LEGEND_HOVERTEXT", "Radioactivity Present\n");
            Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.COUGH_SYMPTOM", "Coughing");
            Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.COUGH_SYMPTOM_TOOLTIP", "Duplicants periodically cough up Polluted Oxygen, producing additional germs");
            Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.DESCRIPTIVE_SYMPTOMS", "Lethal without medical care. Duplicants experience coughing and shortness of breath.");
            Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.DISEASE_SOURCE_DESCRIPTOR", "Currently infected with {2}.\n\nThis Duplicant will produce {1} when coughing.");
            Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.DISEASE_SOURCE_DESCRIPTOR_TOOLTIP", "This Duplicant will cough approximately every {0}" + UI.HORIZONTAL_BR_RULE + "Each time they cough, they will release {1}");
             */


            Strings.Add("STRINGS.ELEMENTS." + (int)123123123 + ".NAME", UI.FormatAsLink("Uranium Ore", "URANINITE"));
            Strings.Add("STRINGS.ELEMENTS." + (int)123123123 + ".DESC", "Uranium is a radioactive element.");
            Strings.Add("STRINGS.ELEMENTS." + (int)123123123 + ".BUILD_DESC", "");
            

            //UraniumElement_Prop.MyEnum = Utils.ExtendEnum(typeof(SimHashes),"UraniumOre",(int)123123123);

            return true;
		}
	}
    /*
	[HarmonyPatch(typeof(ProcGenGame.WorldGen))]
	[HarmonyPatch(new Type[] { })]
	internal static class UraniumElement_WorldGen_ctor
	{
		private static void Postfix()
		{
			Debug.Log(" === WorldGen.ctor Postfix === ");
			//UraniumElement_Prop.Radio = new Radioactivity();
			//var diseases = new Database.Diseases(((ModifierSet)__instance).Root);
			ProcGenGame.WorldGen.diseaseIds = new List<string>
			{
				"FoodPoisoning",
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				"SlimeLung",
                "Radioactive"
            };
		}
		
	}
    */
    /*
    [HarmonyPatch(typeof(Enum), "GetValues", new Type[] { typeof(Type)})]
	internal static class UraniumElement_Enum_GetValues
	{
		private static bool Prefix(ref Array __result, Type enumType)
		{
			//Debug.Log(" === Enum.GetValues Prefix Ini === "+ enumType+" "+typeof(SimHashes));
			if (enumType.Equals(typeof(SimHashes)) && UraniumElement_Prop.MyEnum != null) {
				//__result = Enum.GetValues(typeof(SimHashesMod));
				__result = Enum.GetValues(UraniumElement_Prop.MyEnum);
				//Debug.Log(" === Enum.GetValues Prefix End 1 === ");
				return false;
			}
			//Debug.Log(" === Enum.GetValues Prefix End 2 === ");
			return true;
		}
	}

	[HarmonyPatch(typeof(Enum), "Parse", new Type[] { typeof(Type), typeof(string)})]
	internal static class UraniumElement_Enum_Parse
	{
		private static bool Prefix(ref object __result, Type enumType, string value)
		{
			//Debug.Log(" === Enum.Parse Prefix Ini === " + enumType + " " + value);
			if (enumType.Equals(typeof(SimHashes)))
			{
				//__result = Enum.Parse(typeof(SimHashesMod), value);
				__result = Enum.Parse(UraniumElement_Prop.MyEnum, value);
				//Debug.Log(" === Enum.Parse Prefix End 1 === ");
				return false;
			}
			//Debug.Log(" === Enum.Parse Prefix End 2 === ");
			return true;
		}
	}
	*/

	[HarmonyPatch(typeof(Assets), "SubstanceListHookup")]
	internal static class UraniumElementAssetsSubstanceListHookup
	{
		private static bool Prefix(Assets __instance)
		{
			Debug.Log(" === Assets.SubstanceListHookup Prefix === ");
            //Debug.Log(" === Assets.SubstanceListHookup Prefix === "+__instance.substanceTable.GetList().Count);
            //Substance ironOre = __instance.substanceTable.GetSubstance((SimHashes)1608833498);
            Substance ironOre = (Substance) AccessTools.Method(typeof(SubstanceTable), "GetSubstance").Invoke(__instance.substanceTable, new object[] { 1608833498 });
			Substance uraniumOre = new Substance();
			uraniumOre.name = "UraniumOre";
            //uraniumOre.elementID = (SimHashes) SimHashesMod.UraniumOre;
            //uraniumOre.elementID = (SimHashes)123123123;
            AccessTools.Field(typeof(Substance), "elementID").SetValue(uraniumOre, 123123123);
			uraniumOre.colour = ironOre.colour;
			//uraniumOre.colour.r = 0;
			//uraniumOre.colour.g = 0;
			//uraniumOre.colour.b = 255;
			uraniumOre.debugColour = ironOre.debugColour;
			uraniumOre.overlayColour = ironOre.overlayColour;
			uraniumOre.colourMap = ironOre.colourMap;            
            //uraniumOre.colourMap = (Texture2D)Resources.Load(pt);
            //UraniumElement_Prop.Print(uraniumOre.colourMap);
            uraniumOre.shineMask = ironOre.shineMask;
			uraniumOre.normalMap = ironOre.normalMap;
			uraniumOre.hitEffect = ironOre.hitEffect;
			uraniumOre.fallingStartSound = ironOre.fallingStartSound;
			uraniumOre.fallingStopSound = ironOre.fallingStopSound;
			uraniumOre.renderedByWorld = ironOre.renderedByWorld;

			uraniumOre.idx = ironOre.idx;
			uraniumOre.material = ironOre.material;
            //UraniumElement_Prop.Print(uraniumOre.material);
            string pt = "Mods" + Path.DirectorySeparatorChar + "EnumTest"+ Path.DirectorySeparatorChar+ "iron.png";
            uraniumOre.material.mainTexture = Utils.LoadTexture2DFromFile(pt, 1024, 1024);
            //uraniumOre.material.mainTexture = (Texture2D)Resources.Load(pt);
            //UraniumElement_Prop.Print(uraniumOre.material);
            uraniumOre.anim = ironOre.anim;
            //UraniumElement_Prop.Print(uraniumOre.anim);
            uraniumOre.anims = ironOre.anims;
			uraniumOre.hue = ironOre.hue;
			uraniumOre.saturation = ironOre.saturation;
			uraniumOre.propertyBlock = ironOre.propertyBlock;
			uraniumOre.audioConfig = ironOre.audioConfig;
			uraniumOre.showInEditor = ironOre.showInEditor;
			uraniumOre.saturation = ironOre.saturation;

			__instance.substanceTable.GetList().Add(uraniumOre);
			
			//Debug.Log(" === Assets.SubstanceListHookup Prefix === " + __instance.substanceTable.GetList().Count);
			//for (int i = 0; i < __instance.substanceTable.GetList().Count; i++)
			//{
			//	Debug.Log(" === Assets.SubstanceListHookup Prefix === "+ __instance.substanceTable.GetList().ElementAt(i).name+" "+ __instance.substanceTable.GetList().ElementAt(i).idx);
			//}
			
			return true;
		}
	}
	
	
	[HarmonyPatch(typeof(ElementLoader), "SetupElementsTable")]
	internal static class UraniumElementElementLoaderSetupElementsTable
	{
		private static bool Prefix()
		{
			Debug.Log(" === ElementLoader.SetupElementsTable Prefix === ");
			/*
			Strings.Add("STRINGS.ELEMENTS.URANIUMORE.NAME", UI.FormatAsLink("Uranium Ore", "URANIUMORE"));
			Strings.Add("STRINGS.ELEMENTS.URANIUMORE.DESC", "Uranium is a radioactive element.");
			Strings.Add("STRINGS.ELEMENTS.URANIUMORE.BUILD_DESC", "");
			*/

			return true;
		}
	}
	

	[HarmonyPatch(typeof(ElementLoader), "ParseSolid", new Type[] { typeof(SolidEntry[])})]
	internal static class UraniumElementElementLoad
	{
        //private static bool Prefix(ref Hashtable substanceList, ref string textSolid, ref string textLiquid, ref string textGas, SubstanceTable substanceTable)
        //private static bool Prefix(ref Hashtable substanceList, SolidEntry[] solid_entries, LiquidEntry[] liquid_entries, GasEntry[] gas_entries, SubstanceTable substanceTable)
        private static void Postfix(ref SolidEntry[] solid_entries)
        {
			Debug.Log(" === ElementLoader.Load Prefix === ");
			
			//"UraniumOre","0.129","4","1","1","1","0.9","1808","MoltenIron","","","242.15","800","1840","3","25","159.6882","Metal","Ore | BuildableAny","0","# U02 - Uranium Ore, advanced smelter can get the O2","",
			string line = "UraniumOre,0.129,4,1,1,1,0.9,1808,MoltenIron,,,242.15,800,1840,3,25,159.6882,Metal,Ore | BuildableAny,0,\"# U02 - Uranium Ore, advanced smelter can get the O2\"";
            //textSolid += "\n" + line;

            SolidEntry iron = null;
            foreach (SolidEntry s in solid_entries)
            {
                Debug.Log(s.Id);
                if (s.Id.Equals("IronOre"))
                    iron = s;
            }
            SolidEntry se = new SolidEntry();
            se.Id = "UraniumOre";
            se.IdHash = new HashedString(se.Id);
            //se.Guid = new ResourceGuid(se.Id);
            se.defaultMass = iron.defaultMass;
            se.defaultPressure = iron.defaultPressure;
            se.defaultTemperature = iron.defaultTemperature;
            se.Disabled = false;
            //se.elementId = 123123123
            AccessTools.Field(typeof(SolidEntry), "elementID").SetValue(se, 123123123);
            se.gasSurfaceAreaMultiplier = iron.gasSurfaceAreaMultiplier;
            se.hardness = iron.hardness;
            se.highTemp = iron.highTemp;
            se.highTempTransitionTarget = iron.highTempTransitionTarget;
            se.isDisabled = iron.isDisabled;
            se.lightAbsorptionFactor = iron.lightAbsorptionFactor;
            se.liquidSurfaceAreaMultiplier = iron.liquidSurfaceAreaMultiplier;
            se.lowTemp = iron.lowTemp;
            se.lowTempTransitionTarget = iron.lowTempTransitionTarget;
            se.materialCategory = iron.materialCategory;
            se.maxMass = iron.maxMass;
            se.molarMass = iron.molarMass;
            se.Name = "UraniumOre";
            se.notes = iron.notes;
            se.solidSurfaceAreaMultiplier = iron.solidSurfaceAreaMultiplier;
            se.specificHeatCapacity = iron.specificHeatCapacity;
            se.strength = iron.strength;
            se.sublimateFx = iron.sublimateFx;
            se.sublimateId = iron.sublimateId;
            se.tags = iron.tags;
            se.thermalConductivity = iron.thermalConductivity;
            
                
            solid_entries.Add(se);


           // return true;
		}
	}
	
}
