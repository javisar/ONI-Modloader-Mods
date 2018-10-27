using ProcGen;
using ProcGenGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GeyserGenericConfig;

namespace WorldGenReloadedMod
{
    public class WorldGenReloadedData
    {
        public static List<GeyserPrefabParams> GeyserPrefabParams = new List<GeyserPrefabParams>();
        public static WorldGenReloadedState Config = WorldGenReloadedState.StateManager.State;
        public static ProcGen.World World = null;
        public static List<GeyserPrefabParams> Configs = null;
        public static Dictionary<string, GeyserState> GeyserConfig = new Dictionary<string, GeyserState>();
        public static int GeyserCount = 0;

        public static Dictionary<string, Dictionary<string, int>> CalculatedGeysers = new Dictionary<string, Dictionary<string, int>>();

        public static GeyserPrefabParams? FindGeyserPrefab(string key)
        {
            foreach (GeyserPrefabParams prefab in GeyserPrefabParams)
            {
                if (prefab.geyserType.id.Equals(key))
                    return prefab;
            }
            return null;
        }


        public static SubWorld GetSubWorldFromType(SubWorld.ZoneType type)
        {
            if (World.Zones == null) return null;
            foreach (var sub in World.Zones)
            {
                if (sub.Value.zoneType.Equals(type))
                    return sub.Value;
            }
            return null;
        }


        public static void CalculateGeysers(SeededRandom rnd, WorldGen worldgen)
        {
            Dictionary<string, Dictionary<string, int>> calculatedGeysers = new Dictionary<string, Dictionary<string, int>>();
            foreach (KeyValuePair<string, GeyserState> data in WorldGenReloadedData.GeyserConfig)
            {

                int geyserCount = rnd.RandomRange(data.Value.Minimum, data.Value.Maximum + 1);
                Debug.Log("geyserCount ["+ data.Key +"] = " + geyserCount);
                if (geyserCount <= 0) continue;
                List<string> _subworlds = new List<string>(data.Value.SubWorlds);

                // Remove invalid subworlds
                for (int i = _subworlds.Count - 1; i >= 0; i--)
                {
                    List<TerrainCell> terrainCellsForTag2 = WorldGen.GetTerrainCellsForTag(_subworlds[i]);
                    
                    for (int num = terrainCellsForTag2.Count - 1; num >= 0; num--)
                    {
                        if (!worldgen.IsSafeToSpawnPOI(terrainCellsForTag2[num]))
                        {
                            terrainCellsForTag2.RemoveAt(num);
                        }                      
                    }
                    Debug.Log("Available cells ["+ _subworlds[i] + "] = " + terrainCellsForTag2.Count);
                    if (terrainCellsForTag2.Count <= 0 || terrainCellsForTag2.Count / geyserCount < 3)
                    {
                        Debug.Log("Invalid subworld: " + _subworlds[i]);
                        _subworlds.RemoveAt(i);
                    }
                }
               
                _subworlds.ShuffleSeeded(rnd.RandomSource());
                string[] subworlds = _subworlds.ToArray();                


                for (int i = 0; i < geyserCount; i++)
                {
                    string subworld = subworlds[i % subworlds.Length];

                    if (!calculatedGeysers.ContainsKey(subworld))
                        calculatedGeysers[subworld] = new Dictionary<string, int>();
                    if (!calculatedGeysers[subworld].ContainsKey(data.Key))
                    {
                        calculatedGeysers[subworld][data.Key] = 0;
                    }

                    calculatedGeysers[subworld][data.Key]++;
                }

            }

            Debug.Log("Calculated Geysers: ");
            foreach (KeyValuePair<string, Dictionary<string, int>> subworld in calculatedGeysers)
            {
                Debug.Log("Subworld: "+subworld.Key);
                foreach (KeyValuePair<string, int> geyser in subworld.Value)
                {
                    Debug.Log("\t"+geyser.Key + " = " + geyser.Value);
                }
            }
            WorldGenReloadedData.CalculatedGeysers = calculatedGeysers;
        }

       
        public static void LogGeysersDefaults(List<GeyserPrefabParams> geyserList)
        {
            Debug.Log(" === WorldGenReloadedMod_LogGeysersDefaults Prefix === ");
            foreach (GeyserPrefabParams geyser in geyserList)
            {
                GeyserConfigurator.GeyserType type = geyser.geyserType;
                Debug.Log("Geyser id: " + type.id);
                Debug.Log("\t temperature: " + type.temperature);
                Debug.Log("\t minRatePerCycle: " + type.minRatePerCycle);
                Debug.Log("\t maxRatePerCycle: " + type.maxRatePerCycle);
                Debug.Log("\t maxPressure: " + type.maxPressure);
                Debug.Log("\t minIterationLength: " + type.minIterationLength);
                Debug.Log("\t maxIterationLength: " + type.maxIterationLength);
                Debug.Log("\t minIterationPercent: " + type.minIterationPercent);
                Debug.Log("\t maxIterationPercent: " + type.maxIterationPercent);
                Debug.Log("\t minYearLength: " + type.minYearLength);
                Debug.Log("\t maxYearLength: " + type.maxYearLength);
                Debug.Log("\t minYearPercent: " + type.minYearPercent);
                Debug.Log("\t minYearPercent: " + type.maxYearPercent);
            }
        }
        
    }
}
