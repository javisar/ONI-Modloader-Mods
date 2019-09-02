using System.Collections.Generic;

namespace BuildingModifierMod
{
	public class BuildingModifierConfig
	{
		public bool Enabled { get; set; } = true;

		public bool Debug { get; set; } = false;

		public bool DumpBuildingIDList { get; set; } = false;

		public Dictionary<string, Dictionary<string, object>> Modifiers { get; set; } = new Dictionary<string, Dictionary<string, object>>();


		public static AggregateConfigManager Config
								= new AggregateConfigManager("Config","BuildingModifierState");
	}
}
