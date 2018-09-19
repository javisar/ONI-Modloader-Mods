using System.Collections.Generic;
using ONI_Common.Json;

namespace WorldGenReloadedMod
{
    public class WorldGenReloadedState
	{
        public bool Enabled { get; set; } = true;

        public int SteamGeysers { get; set; } = 512;

       // public Dictionary<string, int> MaximumRoomSizes { get; set; } = new Dictionary<string,int>();


		public static BaseStateManager<WorldGenReloadedState> StateManager
			= new BaseStateManager<WorldGenReloadedState>("WorldGenReloaded");
	}
}