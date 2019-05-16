using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SpeedControl
{
    public class SpeedControlConfig
    {
        public bool Enabled { get; set; } = true;
        public bool Logging { get; set; } = false;

        public float SpeedMultiplier1 { get; set; } = 1f;
        public float SpeedMultiplier2 { get; set; } = 3f;
        public float SpeedMultiplier3 { get; set; } = 10f;



		// Load Config		
		public static SpeedControlConfig Instance
		{
			get
			{
				return ConfigUtils<SpeedControlConfig>.LoadConfig("Config", "SpeedControlConfig.json");
			}
		}
		
	}
}