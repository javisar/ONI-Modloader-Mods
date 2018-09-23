using ProcGen;
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

        public static void CalculateGeysers(SeededRandom rnd)
        {
            Dictionary<string, Dictionary<string, int>> calculatedGeysers = new Dictionary<string, Dictionary<string, int>>();
            foreach (KeyValuePair<string, GeyserState> data in WorldGenReloadedData.GeyserConfig)
            {

                int geyserCount = rnd.RandomRange(data.Value.Minimum, data.Value.Maximum + 1);

                string[] subworlds = data.Value.SubWorlds;
                subworlds.ShuffleSeeded(rnd.RandomSource());

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
                Debug.Log(subworld.Key);
                foreach (KeyValuePair<string, int> geyser in subworld.Value)
                {
                    Debug.Log(geyser.Key + " = " + geyser.Value);
                }
            }
            WorldGenReloadedData.CalculatedGeysers = calculatedGeysers;
        }
    }
}
