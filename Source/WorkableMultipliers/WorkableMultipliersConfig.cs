using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace WorkableMultipliersMod
{
    public class WorkableMultipliersConfig
	{
        public bool Enabled { get; set; } = true;
		public bool Logging { get; set; } = false;

		public Dictionary<string, Dictionary<string, float>> Workables { get; set; }
				= new Dictionary<string, Dictionary<string, float>>();



		// Load Config		
		public static WorkableMultipliersConfig Instance
		{
			get
			{
				return ConfigUtils<WorkableMultipliersConfig>.LoadConfig("Config", "WorkableMultipliersConfig.json");
			}
		}

		 
    }
}