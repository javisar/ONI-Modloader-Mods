using System.Collections.Generic;
using ONI_Common.Json;

namespace RoomSizeMod
{
    public class RoomSizeState
    {
        public bool Enabled { get; set; } = true;

        public int OverallMaximumRoomSize { get; set; } = 512;

        public Dictionary<string, int> MaximumRoomSizes { get; set; } = new Dictionary<string,int>();


		public static BaseStateManager<RoomSizeState> StateManager
			= new BaseStateManager<RoomSizeState>("RoomSize");
	}
}