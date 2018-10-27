using System.Collections.Generic;
using ONI_Common.Json;

namespace WorldGenReloadedMod
{
    public class WorldGenReloadedState
	{
        public bool Enabled { get; set; } = true;

        public bool LogGeysersDefaultConfig { get; set; } = true;

        public bool DisableDefaultPoiGeysers { get; set; } = false;

        public bool ForceSpawnGeyserUnsafePlace { get; set; } = false;


        public Dictionary<string, Dictionary<string, object>> Geysers { get; set; } = new Dictionary<string, Dictionary<string, object>>();


        public static BaseStateManager<WorldGenReloadedState> StateManager
                                = new BaseStateManager<WorldGenReloadedState>("WorldGenReloaded");
    }

    public class GeyserState
    {
        public string Id = null;
        public int Minimum { get; set; } = 1;
        public int Maximum { get; set; } = 1;
        public float Probability { get; set; } = 0.2f;
        public string[] SubWorlds { get; set; } = new string[] { };
        public int TotalSpawned { get; set; } = 0;

        public GeyserState()
        {
        }

        public GeyserState(string id)
        {
            this.Id = id;
        }
    }
}