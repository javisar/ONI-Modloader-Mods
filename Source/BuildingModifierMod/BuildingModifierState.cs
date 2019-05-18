using System.Collections.Generic;

namespace BuildingModifierMod
{
	public class BuildingModifierState
	{
		public bool Enabled { get; set; } = true;

		public bool Debug { get; set; } = false;

		public bool DumpBuildingIDList { get; set; } = false;

		public Dictionary<string, Dictionary<string, object>> Modifiers { get; set; } = new Dictionary<string, Dictionary<string, object>>();


		public static AggregateStateManager StateManager
								= new AggregateStateManager("BuildingModifier");
	}
}
