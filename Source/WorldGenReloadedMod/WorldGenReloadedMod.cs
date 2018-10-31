using Harmony;
using Klei;
using Newtonsoft.Json.Linq;
using ProcGen;
using ProcGen.Noise;
using ProcGenGame;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using WorldGenReloadedMod;
using static GeyserGenericConfig;

namespace WorlGenReloadedMod
{


    [HarmonyPatch(typeof(GeyserGenericConfig), "GenerateConfigs")]
    internal class WorldGenReloadedMod_GeyserGenericConfig_GenerateConfigs
    {

        private static void Postfix(ref List<GeyserPrefabParams> __result)
        {
            Debug.Log(" === WorldGenReloadedMod_GeyserGenericConfig_GenerateConfigs Prefix === ");
            WorldGenReloadedData.GeyserPrefabParams = new List<GeyserPrefabParams>(__result);

            if (WorldGenReloadedData.Config.LogGeysersDefaultConfig)
            {
                WorldGenReloadedData.LogGeysersDefaults(__result);

            }

            //__result.Clear();

            List<GeyserPrefabParams> list = new List<GeyserPrefabParams>();

            // foreach config geyser
            foreach (KeyValuePair<string, Dictionary<string, object>> geyser in WorldGenReloadedData.Config.Geysers)
            {
                Dictionary<string, object> geyserData = geyser.Value;
                GeyserPrefabParams? geyserPrefab = WorldGenReloadedData.FindGeyserPrefab(geyser.Key);
                //int min = 1, max = 1;
                //float probability = 0;
                //string[] subworlds = null;
                GeyserState geyserConfig = new GeyserState(geyser.Key);

                // foreach config attribute
                foreach (KeyValuePair<string, object> attribute in geyserData)
                {
                    //Debug.Log(attribute.Key.ToLower());
                    //Debug.Log(attribute.Value.GetType());
                    //Debug.Log(attribute.Value);


                    switch (attribute.Key.ToLower())
                    {
                        case "properties":
                            //Debug.Log(attribute.Value.GetType());
                            //Debug.Log("attribute.Value: "+attribute.Value);

                            // Set geyser basic properties
                            foreach (JProperty property in (JToken)attribute.Value)
                            {                                
                                //Debug.Log(property.Name);
                                FieldInfo fi = AccessTools.Field(typeof(GeyserConfigurator.GeyserType), property.Name);
                                //Debug.Log(fi);
                                fi.SetValue(((GeyserPrefabParams)geyserPrefab).geyserType, (float)property.Value);
                                //Debug.Log(property.Name);
                            }
                            break;

                        case "minimum":
                            geyserConfig.Minimum = Convert.ToInt32(attribute.Value);
                            break;
                        case "maximum":
                            geyserConfig.Maximum = Convert.ToInt32(attribute.Value);
                            break;
                        case "probability":
                            geyserConfig.Probability = (float)Convert.ToDouble(attribute.Value);
                            break;
                        case "subworlds":
                            geyserConfig.SubWorlds = ((JArray)attribute.Value).ToObject<string[]>();
                            break;

                        default:
                            break;
                    }

                }
                WorldGenReloadedData.GeyserConfig.Add(geyser.Key, geyserConfig);

            }

            __result = WorldGenReloadedData.GeyserPrefabParams;

        }
        
    }    


    [HarmonyPatch(typeof(WorldGen), "RenderOffline")]
    internal class WorldGenReloadedMod_WorldGen_RenderOffline
    {
        private static FieldInfo successCallbackFnF = AccessTools.Field(typeof(WorldGen), "successCallbackFn");
        private static FieldInfo myRandomF = AccessTools.Field(typeof(WorldGen), "myRandom");
        private static FieldInfo runningF = AccessTools.Field(typeof(WorldGen), "running");
        private static FieldInfo dataF = AccessTools.Field(typeof(WorldGen), "data");
        private static FieldInfo errorCallbackF = AccessTools.Field(typeof(WorldGen), "errorCallback");
        private static MethodInfo SpawnMobsAndTemplatesM = AccessTools.Method(typeof(WorldGen), "SpawnMobsAndTemplates");
        private static MethodInfo PlaceTemplateSpawnersM = AccessTools.Method(typeof(WorldGen), "PlaceTemplateSpawners");

        private static bool Prefix(WorldGen __instance, ref Sim.Cell[] __result, bool doSettle, ref Sim.DiseaseCell[] dc)
        {
            Debug.Log(" === WorldGenReloadedMod_WorldGen_RenderOffline ===");
            WorldGen.OfflineCallbackFunction successCallbackFn = ((WorldGen.OfflineCallbackFunction)successCallbackFnF.GetValue(__instance));
            SeededRandom myRandom = ((SeededRandom)myRandomF.GetValue(__instance));
            Data data = ((Data)dataF.GetValue(null));
            Action<OfflineWorldGen.ErrorInfo> errorCallback = ((Action<OfflineWorldGen.ErrorInfo>)errorCallbackF.GetValue(__instance));


            Sim.Cell[] cells = null;
            float[] bgTemp = null;
            dc = null;
            HashSet<int> borderCells = new HashSet<int>();
            //CompleteLayout(successCallbackFn);
            __instance.CompleteLayout(successCallbackFn);
            //WriteOverWorldNoise(successCallbackFn);
            WorldGen.WriteOverWorldNoise(successCallbackFn);
            if (!WorldGen.RenderToMap(successCallbackFn, ref cells, ref bgTemp, ref dc, ref borderCells))
            {
                successCallbackFn(UI.WORLDGEN.FAILED.key, -100f, WorldGenProgressStages.Stages.Failure);
                __result = null;
                return false;
            }
            WorldGen.EnsureEnoughAlgaeInStartingBiome(cells);
            List<KeyValuePair<Vector2I, TemplateContainer>> list = new List<KeyValuePair<Vector2I, TemplateContainer>>();
            TemplateContainer baseStartingTemplate = TemplateCache.GetBaseStartingTemplate();
            List<TerrainCell> terrainCellsForTag = WorldGen.GetTerrainCellsForTag(WorldGenTags.StartLocation);
            foreach (TerrainCell item5 in terrainCellsForTag)
            {
                List<KeyValuePair<Vector2I, TemplateContainer>> list2 = list;
                Vector2 vector = item5.poly.Centroid();
                int a = (int)vector.x;
                Vector2 vector2 = item5.poly.Centroid();
                list2.Add(new KeyValuePair<Vector2I, TemplateContainer>(new Vector2I(a, (int)vector2.y), baseStartingTemplate));
            }

            List<TemplateContainer> list3 = TemplateCache.CollectBaseTemplateAssets("poi/");
            foreach (SubWorld subWorld in WorldGen.Settings.GetSubWorldList())
            {
                if (subWorld.pointsOfInterest != null)
                {
                    //// Disable default POI geysers
                    if (WorldGenReloadedData.Config.DisableDefaultPoiGeysers)
                    {
                        SubWorld _subWorld = subWorld;
                        DisableDefaultPoiGeysers(ref _subWorld);
                        
                    }
                    ////
                    foreach (KeyValuePair<string, string[]> item6 in subWorld.pointsOfInterest)
                    {
                        List<TerrainCell> terrainCellsForTag2 = WorldGen.GetTerrainCellsForTag(subWorld.name.ToTag());
                        for (int num = terrainCellsForTag2.Count - 1; num >= 0; num--)
                        {
                            if (!__instance.IsSafeToSpawnPOI(terrainCellsForTag2[num]))
                            {
                                terrainCellsForTag2.Remove(terrainCellsForTag2[num]);
                            }
                        }
                        if (terrainCellsForTag2.Count > 0)
                        {
                            string template = null;
                            TemplateContainer templateContainer = null;
                            int num2 = 0;
                            while (templateContainer == null && num2 < item6.Value.Length)
                            {
                                template = item6.Value[myRandom.RandomRange(0, item6.Value.Length)];
                                templateContainer = list3.Find((TemplateContainer value) => value.name == template);
                                num2++;
                            }
                            if (templateContainer != null)
                            {
                                list3.Remove(templateContainer);
                                for (int i = 0; i < terrainCellsForTag2.Count; i++)
                                {
                                    TerrainCell terrainCell = terrainCellsForTag2[myRandom.RandomRange(0, terrainCellsForTag2.Count)];
                                    if (!terrainCell.node.tags.Contains(WorldGenTags.POI))
                                    {
                                        if (!(templateContainer.info.size.Y > terrainCell.poly.MaxY - terrainCell.poly.MinY))
                                        {
                                            List<KeyValuePair<Vector2I, TemplateContainer>> list4 = list;
                                            Vector2 vector3 = terrainCell.poly.Centroid();
                                            int a2 = (int)vector3.x;
                                            Vector2 vector4 = terrainCell.poly.Centroid();
                                            list4.Add(new KeyValuePair<Vector2I, TemplateContainer>(new Vector2I(a2, (int)vector4.y), templateContainer));
                                            terrainCell.node.tags.Add(template.ToTag());
                                            terrainCell.node.tags.Add(WorldGenTags.POI);
                                            break;
                                        }
                                        float num3 = templateContainer.info.size.Y - (terrainCell.poly.MaxY - terrainCell.poly.MinY);
                                        float num4 = templateContainer.info.size.X - (terrainCell.poly.MaxX - terrainCell.poly.MinX);
                                        if (terrainCell.poly.MaxY + num3 < (float)Grid.HeightInCells && terrainCell.poly.MinY - num3 > 0f && terrainCell.poly.MaxX + num4 < (float)Grid.WidthInCells && terrainCell.poly.MinX - num4 > 0f)
                                        {
                                            List<KeyValuePair<Vector2I, TemplateContainer>> list5 = list;
                                            Vector2 vector5 = terrainCell.poly.Centroid();
                                            int a3 = (int)vector5.x;
                                            Vector2 vector6 = terrainCell.poly.Centroid();
                                            list5.Add(new KeyValuePair<Vector2I, TemplateContainer>(new Vector2I(a3, (int)vector6.y), templateContainer));
                                            terrainCell.node.tags.Add(template.ToTag());
                                            terrainCell.node.tags.Add(WorldGenTags.POI);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            ////        
            ProcessGeysers(__instance, ref list, myRandom);
            ////

            // Generation of geyser Overwrited in the previous line
            /*
            List<TemplateContainer> list6 = TemplateCache.CollectBaseTemplateAssets("features/");
            foreach (SubWorld subWorld2 in WorldGen.Settings.GetSubWorldList())
            {
                if (subWorld2.featureTemplates != null && subWorld2.featureTemplates.Count > 0)
                {
                    List<string> list7 = new List<string>();
                    foreach (KeyValuePair<string, int> featureTemplate in subWorld2.featureTemplates)
                    {
                        for (int j = 0; j < featureTemplate.Value; j++)
                        {
                            list7.Add(featureTemplate.Key);
                        }
                    }
                    list7.ShuffleSeeded(myRandom.RandomSource());
                    List<TerrainCell> terrainCellsForTag3 = WorldGen.GetTerrainCellsForTag(subWorld2.name.ToTag());
                    terrainCellsForTag3.ShuffleSeeded(myRandom.RandomSource());
                    foreach (TerrainCell item7 in terrainCellsForTag3)
                    {
                        if (list7.Count == 0)
                        {
                            break;
                        }
                        if (__instance.IsSafeToSpawnFeatureTemplate(item7))
                        {
                            string template2 = list7[list7.Count - 1];
                            list7.RemoveAt(list7.Count - 1);
                            TemplateContainer templateContainer2 = list6.Find((TemplateContainer value) => value.name == template2);
                            if (templateContainer2 != null)
                            {
                                List<KeyValuePair<Vector2I, TemplateContainer>> list8 = list;
                                Vector2 vector7 = item7.poly.Centroid();
                                int a4 = (int)vector7.x;
                                Vector2 vector8 = item7.poly.Centroid();
                                list8.Add(new KeyValuePair<Vector2I, TemplateContainer>(new Vector2I(a4, (int)vector8.y), templateContainer2));
                                item7.node.tags.Add(template2.ToTag());
                                item7.node.tags.Add(WorldGenTags.POI);
                            }
                        }
                    }
                }
            }
            */
            foreach (int item8 in borderCells)
            {
                cells[item8].SetValues(WorldGen.unobtaniumElement, ElementLoader.elements);
            }
            if (doSettle)
            {
                //running = WorldGenSimUtil.DoSettleSim(cells, bgTemp, dc, successCallbackFn, data, list, errorCallback, delegate (Sim.Cell[] updatedCells, float[] updatedBGTemp, Sim.DiseaseCell[] updatedDisease)
                runningF.SetValue(null, WorldGenSimUtil.DoSettleSim(cells, bgTemp, dc, successCallbackFn, data, list, errorCallback, delegate (Sim.Cell[] updatedCells, float[] updatedBGTemp, Sim.DiseaseCell[] updatedDisease)
                {
                    //SpawnMobsAndTemplates(updatedCells, updatedBGTemp, updatedDisease, borderCells);
                    SpawnMobsAndTemplatesM.Invoke(__instance, new object[] { updatedCells, updatedBGTemp, updatedDisease, borderCells });
                }));
            }
            foreach (KeyValuePair<Vector2I, TemplateContainer> item9 in list)
            {
                //PlaceTemplateSpawners(item9.Key, item9.Value);
                PlaceTemplateSpawnersM.Invoke(__instance, new object[] { item9.Key, item9.Value });
            }
            for (int num5 = data.gameSpawnData.buildings.Count - 1; num5 >= 0; num5--)
            {
                int item = Grid.XYToCell(data.gameSpawnData.buildings[num5].location_x, data.gameSpawnData.buildings[num5].location_y);
                if (borderCells.Contains(item))
                {
                    data.gameSpawnData.buildings.RemoveAt(num5);
                }
            }
            for (int num6 = data.gameSpawnData.elementalOres.Count - 1; num6 >= 0; num6--)
            {
                int item2 = Grid.XYToCell(data.gameSpawnData.elementalOres[num6].location_x, data.gameSpawnData.elementalOres[num6].location_y);
                if (borderCells.Contains(item2))
                {
                    data.gameSpawnData.elementalOres.RemoveAt(num6);
                }
            }
            for (int num7 = data.gameSpawnData.otherEntities.Count - 1; num7 >= 0; num7--)
            {
                int item3 = Grid.XYToCell(data.gameSpawnData.otherEntities[num7].location_x, data.gameSpawnData.otherEntities[num7].location_y);
                if (borderCells.Contains(item3))
                {
                    data.gameSpawnData.otherEntities.RemoveAt(num7);
                }
            }
            for (int num8 = data.gameSpawnData.pickupables.Count - 1; num8 >= 0; num8--)
            {
                int item4 = Grid.XYToCell(data.gameSpawnData.pickupables[num8].location_x, data.gameSpawnData.pickupables[num8].location_y);
                if (borderCells.Contains(item4))
                {
                    data.gameSpawnData.pickupables.RemoveAt(num8);
                }
            }
            WorldGen.SaveWorldGen();
            successCallbackFn(UI.WORLDGEN.COMPLETE.key, 101f, WorldGenProgressStages.Stages.Complete);
            //running = false;
            runningF.SetValue(null, false);
            __result = cells;
            return false;
        }

        private static void DisableDefaultPoiGeysers(ref SubWorld subWorld)
        {
            Debug.Log("Point of Interest: " + subWorld.name);
            Dictionary<string, string[]> finalPois = new Dictionary<string, string[]>(subWorld.pointsOfInterest);
            foreach (string poi in subWorld.pointsOfInterest.Keys)
            {
                
                if (poi.ToLower().Contains("geyser"))
                {
                    finalPois.Remove(poi);
                    Debug.Log("[" + poi.ToLower()+ "] disabled");
                }
                else
                {
                    Debug.Log("[" + poi.ToLower()+"]");
                }
            }
            AccessTools.Property(typeof(SubWorld), "pointsOfInterest").SetValue(subWorld, finalPois, null);
        }

        private static TemplateContainer GetGeyserTemplate(TemplateContainer _template, string geyserId)
        {

            TemplateContainer template = Newtonsoft.Json.JsonConvert.DeserializeObject<TemplateContainer>(
                    Newtonsoft.Json.JsonConvert.SerializeObject(_template)
                    /*
                    ,new Newtonsoft.Json.JsonSerializerSettings {
                        ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace
                    }
                    */
            );

            foreach (TemplateClasses.Prefab pref in template.otherEntities)
            {
                if (pref.id.Contains("GeyserGeneric"))
                {
                    pref.id = "GeyserGeneric_"+geyserId;
                    pref.location_x = 0;
                    pref.location_y = 0;
                }
            }
            return template;
        }

        private static void ProcessGeysers(WorldGen __instance, ref List<KeyValuePair<Vector2I, TemplateContainer>> templateList, SeededRandom myRandom)
        {
            WorldGenReloadedData.CalculateGeysers(myRandom, __instance);
            List<TemplateContainer> featuresList = TemplateCache.CollectBaseTemplateAssets("features/");
            foreach (SubWorld subWorld in WorldGen.Settings.GetSubWorldList())
            {
                Debug.Log("Processing zone: " + subWorld.name);
                if (!WorldGenReloadedData.CalculatedGeysers.ContainsKey(subWorld.name))
                {
                    continue;
                }
                Dictionary<string, int> subworldConfig = WorldGenReloadedData.CalculatedGeysers[subWorld.name];

                if (subWorld.pointsOfInterest != null)
                {

                    foreach (KeyValuePair<string, int> item6 in subworldConfig)
                    {
                        Debug.Log("Processing geyser: [" + item6.Key + "," + item6.Value + "]");
                        for (int numGeysers = 0; numGeysers < item6.Value; numGeysers++)
                        {
                            List<TerrainCell> terrainCellsForTag2 = WorldGen.GetTerrainCellsForTag(subWorld.name.ToTag());
                            for (int num = terrainCellsForTag2.Count - 1; num >= 0; num--)
                            {
                                if (!__instance.IsSafeToSpawnPOI(terrainCellsForTag2[num]))
                                {
                                    terrainCellsForTag2.Remove(terrainCellsForTag2[num]);
                                }
                            }
                            /*
                            if (terrainCellsForTag2.Count <= 0 && WorldGenReloadedData.Config.ForceSpawnGeyserUnsafePlace)
                            {
                                terrainCellsForTag2 = WorldGen.GetTerrainCellsForTag(subWorld.name.ToTag());
                            }
                            */
                            Debug.Log("Available cells = " + terrainCellsForTag2.Count);
                           
                            if (terrainCellsForTag2.Count > 0)
                            {
                                string template = null;
                                TemplateContainer templateContainer = null;
                                int num2 = 0;
                                /*
                                while (templateContainer == null && num2 < item6.Value.Length)
                                {
                                    template = item6.Value[myRandom.RandomRange(0, item6.Value.Length)];
                                    templateContainer = list3.Find((TemplateContainer value) => value.name == template);
                                    num2++;
                                }
                                */

                                // Constructs a template using the already existing feature generic geyser. first() cause there is only one feature in the folder. TODO
                                templateContainer = GetGeyserTemplate(featuresList.First(), item6.Key);

                                Debug.Log("Adding geyser: " + templateContainer.name + " [" + item6.Key + "]");
                                if (templateContainer != null)
                                {
                                    //list3.Remove(templateContainer);
                                    bool geyserPlaced = false;
                                    for (int i = 0; i < terrainCellsForTag2.Count; i++)
                                    {
                                        TerrainCell terrainCell = terrainCellsForTag2[myRandom.RandomRange(0, terrainCellsForTag2.Count)];
                                        if (!terrainCell.node.tags.Contains(WorldGenTags.POI))
                                        {
                                            if (!(templateContainer.info.size.Y > terrainCell.poly.MaxY - terrainCell.poly.MinY))
                                            {
                                                List<KeyValuePair<Vector2I, TemplateContainer>> list4 = templateList;
                                                Vector2 vector3 = terrainCell.poly.Centroid();
                                                int a2 = (int)vector3.x;
                                                Vector2 vector4 = terrainCell.poly.Centroid();
                                                list4.Add(new KeyValuePair<Vector2I, TemplateContainer>(new Vector2I(a2, (int)vector4.y), templateContainer));
                                                terrainCell.node.tags.Add(template.ToTag());
                                                terrainCell.node.tags.Add(WorldGenTags.POI);
                                                geyserPlaced = true;
                                                break;
                                            }
                                            float num3 = templateContainer.info.size.Y - (terrainCell.poly.MaxY - terrainCell.poly.MinY);
                                            float num4 = templateContainer.info.size.X - (terrainCell.poly.MaxX - terrainCell.poly.MinX);
                                            if (terrainCell.poly.MaxY + num3 < (float)Grid.HeightInCells && terrainCell.poly.MinY - num3 > 0f && terrainCell.poly.MaxX + num4 < (float)Grid.WidthInCells && terrainCell.poly.MinX - num4 > 0f)
                                            {
                                                List<KeyValuePair<Vector2I, TemplateContainer>> list5 = templateList;
                                                Vector2 vector5 = terrainCell.poly.Centroid();
                                                int a3 = (int)vector5.x;
                                                Vector2 vector6 = terrainCell.poly.Centroid();
                                                list5.Add(new KeyValuePair<Vector2I, TemplateContainer>(new Vector2I(a3, (int)vector6.y), templateContainer));
                                                terrainCell.node.tags.Add(template.ToTag());
                                                terrainCell.node.tags.Add(WorldGenTags.POI);
                                                geyserPlaced = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log("Cannot find a place for geyser. POI in the way: " + item6.Key);
                                        }
                                    }
                                    if (!geyserPlaced)
                                    {
                                        Debug.Log("Cannot find a place for geyser. Not enought space: " + item6.Key);
                                    }
                                }
                                else
                                {
                                    Debug.Log("Cannot build geyser template: " + item6.Key);
                                }
                            }
                            else
                            {
                                Debug.Log("Cannot find a place for geyser. Empty space: " + item6.Key);
                            }
                        }
                    }
                }

            }
        }
    }


}
