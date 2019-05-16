using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RoomSize
{
    public class RoomSizeConfig
    {
        public bool Enabled { get; set; } = true;

        public int OverallMaximumRoomSize { get; set; } = 512;

        public Dictionary<string, int> MaximumRoomSizes { get; set; } = new Dictionary<string,int>();




		// Load Config		
		public static RoomSizeConfig Instance
		{
			get {
				return ConfigUtils<RoomSizeConfig>.LoadConfig("Config", "RoomSizeConfig.json");
			}
		}
	}
}