using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace BuildingModifierMod
{


	// Executed every time ONI loads a Building
	[HarmonyPatch(typeof(BuildingConfigManager), "RegisterBuilding")]
	internal class BuildingModifierMod_BuildingConfigManager_RegisterBuilding
	{
        private static FieldInfo configTableF = AccessTools.Field(typeof(BuildingConfigManager), "configTable");
        private static FieldInfo baseTemplateF = AccessTools.Field(typeof(BuildingConfigManager), "baseTemplate");
        private static FieldInfo NonBuildableBuildingsF = AccessTools.Field(typeof(BuildingConfigManager), "NonBuildableBuildings");

        private static bool Hooked = false;

        private static bool Prefix(BuildingConfigManager __instance, IBuildingConfig config)
        {
            bool result = false;
            if (Hooked) return true;

            //CreateBuildingDef
            BuildingDef buildingDef = config.CreateBuildingDef();
            Hooked = true;
            try
            {
                
                result = RegisterBuilding(__instance, config, buildingDef);
              
            }
            catch (Exception e) {          
                Debug.Log(" === [BuildingModifier] ERROR registering building ["+ buildingDef.PrefabID+ "]. Incompatible building config.");
                Debug.Log(e.StackTrace);
                // __instance.RegisterBuilding(config);         
                //throw e;
                Application.Quit();
            }
            Hooked = false;
            return result;
        }

        private static bool RegisterBuilding(BuildingConfigManager __instance, IBuildingConfig config, BuildingDef buildingDef)
		{
            //Debug.Log(config.CreateBuildingDef().PrefabID);
			if (!Helper.Config.Enabled)
                return true;
            
            Helper.Log(" === [BuildingModifier] BuildingModifierMod_BuildingConfigManager_RegisterBuilding Prefix === ");

            //CreateBuildingDef
            //BuildingDef buildingDef = config.CreateBuildingDef();
            // Check if there is a config for the current building
            if (!Helper.Config.Modifiers.ContainsKey(buildingDef.PrefabID) )
            {
                Helper.Log(" === [BuildingModifier] Ignoring: " + buildingDef.PrefabID);
				return true;
            }

            Debug.Log(" === [BuildingModifier] Processing: " + buildingDef.PrefabID);

            Helper.Log(" === [BuildingModifier] CreateBuildingDef === ");
            Helper.Process(buildingDef, null);
            buildingDef.GenerateOffsets();  // Remake the offsets after modification


            // Create gameobject
            //configTable[config] = buildingDef;
            ((Dictionary < IBuildingConfig, BuildingDef > )configTableF.GetValue(__instance))[config] = buildingDef;
			//GameObject gameObject = Object.Instantiate(baseTemplate);
			GameObject gameObject = UnityEngine.Object.Instantiate((GameObject)baseTemplateF.GetValue(__instance));
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.GetComponent<KPrefabID>().PrefabTag = buildingDef.Tag;
			gameObject.name = buildingDef.PrefabID + "Template";
			gameObject.GetComponent<Building>().Def = buildingDef;
			gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets = buildingDef.PlacementOffsets;
			if (buildingDef.Deprecated)
			{
				gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent);
			}

            //ConfigureBuildingTemplate
            config.ConfigureBuildingTemplate(gameObject, buildingDef.Tag);
            Helper.Log(" === [BuildingModifier] ConfigureBuildingTemplate === ");
            Helper.Process(buildingDef, gameObject, Helper.BuildingType.Building);

            //ConfigureBuildingTemplate
            buildingDef.BuildingComplete = BuildingLoader.Instance.CreateBuildingComplete(gameObject, buildingDef);
            Helper.Log(" === [BuildingModifier] CreateBuildingComplete === ");
            Helper.Process(buildingDef, gameObject, Helper.BuildingType.Building);

            bool flag = true;
			//for (int i = 0; i < NonBuildableBuildings.Length; i++)
			for (int i = 0; i < ((string[])NonBuildableBuildingsF.GetValue(__instance)).Length; i++)
			{
				//if (buildingDef.PrefabID == NonBuildableBuildings[i])
				if (buildingDef.PrefabID == ((string[])NonBuildableBuildingsF.GetValue(__instance))[i])
				{
					flag = false;
					break;
				}
			}

            // Previews
			if (flag)
			{
				buildingDef.BuildingUnderConstruction = BuildingLoader.Instance.CreateBuildingUnderConstruction(buildingDef);				
				buildingDef.BuildingUnderConstruction.name = BuildingConfigManager.GetUnderConstructionName(buildingDef.BuildingUnderConstruction.name);
				buildingDef.BuildingPreview = BuildingLoader.Instance.CreateBuildingPreview(buildingDef);				
				buildingDef.BuildingPreview.name += "Preview";
			}			

			buildingDef.PostProcess();

            // Try to avoid errors if the gameobject doesn't have RequiereInputs
            /*
            if (gameObject.GetComponent<RequireInputs>() == null)
                gameObject.AddOrGet<RequireInputs>();
            */

            //DoPostConfigureComplete
            config.DoPostConfigureComplete(buildingDef.BuildingComplete);
            Helper.Log(" === [BuildingModifier] DoPostConfigureComplete === ");
            Helper.Process(buildingDef, gameObject, Helper.BuildingType.BuildingComplete);

            // Previews
			if (flag)
			{
				config.DoPostConfigurePreview(buildingDef, buildingDef.BuildingPreview);
                Helper.Log(" === [BuildingModifier] DoPostConfigurePreview === ");
                Helper.Process(buildingDef, gameObject, Helper.BuildingType.BuildingPreview);

				config.DoPostConfigureUnderConstruction(buildingDef.BuildingUnderConstruction);
                Helper.Log(" === [BuildingModifier] DoPostConfigureUnderConstruction === ");
                Helper.Process(buildingDef, gameObject, Helper.BuildingType.BuildingUnderConstruction);
			}

			Assets.AddBuildingDef(buildingDef);

			return false;
		}

	}

	// Executed one at the end when ONI has loaded all buildings
	[HarmonyPatch(typeof(BuildingConfigManager), "ConfigurePost")]
	internal class BuildingModifierMod_BuildingConfigManager_ConfigurePost
	{
        private static FieldInfo configTableF = AccessTools.Field(typeof(BuildingConfigManager), "configTable");

        private static void Postfix(BuildingConfigManager __instance)
		{
			if (!Helper.Config.Enabled)
                return;

            Helper.Log(" === [BuildingModifier] BuildingModifierMod_BuildingConfigManager_ConfigurePost Postfix === ");

            // After the execution of BuildingConfigManager.ConfigurePost, this iterates
            // over all buildings and tries the last attribute modifications
            foreach (KeyValuePair<IBuildingConfig, BuildingDef> item in (Dictionary <IBuildingConfig, BuildingDef>)configTableF.GetValue(__instance))
			{
                // Check if building has a mod config
                if (!Helper.Config.Modifiers.ContainsKey(item.Value.PrefabID))
                    continue;

                Helper.Log(" === [BuildingModifier] ConfigurePost === ");
				Helper.Process(item.Value, item.Value.BuildingComplete);
			}

            // List all config attributes not found or that throw an error
			if (Helper.ModifiersAll.Count() != Helper.ModifiersFound.Count())
			{
				Debug.Log(" === [BuildingModifier] ERROR: Not found modifiers:");
				foreach (string modifier in Helper.ModifiersAll)
				{
					if (!Helper.ModifiersFound.Contains(modifier))
					{
						Debug.Log(modifier);
					}
				}
			}
            Debug.Log(" === [BuildingModifier] Finished.");
        }
	}

    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class BuildingModifierMod_GeneratedBuildings_LoadGeneratedBuildings
    {
        private static FieldInfo configTableF = AccessTools.Field(typeof(BuildingConfigManager), "configTable");

        private static void Postfix()
        {
            if (!Helper.Config.Enabled)
                return;

            Helper.Log(" === [BuildingModifier] BuildingModifierMod_GeneratedBuildings_LoadGeneratedBuildings Postfix === ");
            Helper.Log(" === [BuildingModifier] Building ID List === ");
            Comparison<BuildingDef> comparison = (x, y) => x.PrefabID.CompareTo(y.PrefabID);
            BuildingDef[] list = (BuildingDef[])Assets.BuildingDefs.ToArray();
            Array.Sort(list, delegate (BuildingDef x, BuildingDef y) {
                return x.PrefabID.CompareTo(y.PrefabID);
            });
            foreach (BuildingDef building in list)
            {
                Helper.Log(building.PrefabID);
            }

        }
    }

    /*
    [HarmonyPatch(typeof(BuildingDef), "PostProcess")]
    internal class BuildingModifierMod_BuildingDef_PostProcess
    {


        private static void Prefix(BuildingDef __instance)
        {
            //Debug.Log(" === BuildingModifierMod_BuildingDef_PostProcess Prefix === ");
            
            //var harmony = HarmonyInstance.Create("Assembly-CSharp");
            //harmony.PatchAll(Assembly.GetExecutingAssembly());
            //var harmony = HarmonyInstance.Create("ONI-Modloader");
            var harmony = HarmonyInstance.Create("Assembly-CSharp");
            //harmony.PatchAll(Assembly.GetExecutingAssembly());
            var methods = harmony.GetPatchedMethods();
            foreach (var method in methods)
            {
                Debug.Log(method.ToString());
            }
            //harmony.PatchAll(Assembly.LoadFrom("Assembly-CSharp"));

            //MethodInfo postfix = AccessTools.Method(typeof(PostProcessClass), "PostProcess");

            foreach (var method in Assets.BuildingDefs)
            {
                Debug.Log(method.PrefabID);
            }
            

            //PostProcessClass.Dummy(__instance);

        }

    }
	*/

    /*
	[HarmonyPatch(typeof(IBuildingConfig), "CreateBuildingDef")]
	internal class BuildingModifierMod_IBuildingConfig_CreateBuildingDef
	{

		private static void Postfix(BuildingDef __result)
		{
			
			PostProcessClass.Dummy(__result);
		}
	}

	[HarmonyPatch(typeof(IBuildingConfig), "ConfigureBuildingTemplate")]
	internal class BuildingModifierMod_IBuildingConfig_ConfigureBuildingTemplate
	{

		private static void Postfix(GameObject go, Tag prefab_tag)
		{
			PostProcessClass.Dummy(go.GetComponent<Building>().Def);
		}
	}

	[HarmonyPatch(typeof(IBuildingConfig), "DoPostConfigureUnderConstruction")]
	internal class BuildingModifierMod_IBuildingConfig_DoPostConfigureUnderConstruction
	{

		private static void Postfix(GameObject go)
		{
			PostProcessClass.Dummy(go.GetComponent<Building>().Def);
		}
	}

	[HarmonyPatch(typeof(IBuildingConfig), "DoPostConfigurePreview")]
	internal class BuildingModifierMod_IBuildingConfig_DoPostConfigurePreview
	{

		private static void Postfix(BuildingDef def, GameObject go)
		{
			PostProcessClass.Dummy(def);
		}
	}

	[HarmonyPatch(typeof(IBuildingConfig), "DoPostConfigureComplete")]
	internal class BuildingModifierMod_IBuildingConfig_DoPostConfigureComplete
	{

		private static void Postfix(GameObject go)
		{
			PostProcessClass.Dummy(go.GetComponent<Building>().Def);
		}
	}

	[HarmonyPatch(typeof(IBuildingConfig), "ConfigurePost")]
	internal class BuildingModifierMod_IBuildingConfig_ConfigurePost
	{

		private static void Postfix(BuildingDef def)
		{
			PostProcessClass.Dummy(def);
		}
	}
	*/

    /*
	[HarmonyPatch(typeof(GasReservoirConfig), "ConfigureBuildingTemplate")]
	internal class BuildingModifierMod_GasReservoirConfig_ConfigureBuildingTemplate
	{
		private static void Postfix(ref GameObject go, Tag prefab_tag)
		{
			Debug.Log(" === BuildingModifierMod_GasReservoirConfig_ConfigureBuildingTemplate Postfix === ");
			Storage storage = go.GetComponent<Storage>();
			storage.capacityKg = 200;
			ConduitConsumer conduitConsumer = go.GetComponent<ConduitConsumer>();
			conduitConsumer.capacityKG = 200;
		}
	}
	*/
}
