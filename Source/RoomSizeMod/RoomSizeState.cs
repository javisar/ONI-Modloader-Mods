using System.Collections.Generic;

namespace RoomSizeMod
{
    public class RoomSizeState
    {
        public bool Enabled { get; set; } = true;

        public int OverallMaximumRoomSize { get; set; } = 512;

        public Dictionary<string, int> MaximumRoomSizes { get; set; } = new Dictionary<string,int>();



    }
}