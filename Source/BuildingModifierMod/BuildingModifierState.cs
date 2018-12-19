using System.Collections.Generic;
using ONI_Common.Json;

namespace BuildingModifierMod
{
	public class BuildingModifierState
	{
		public bool Enabled { get; set; } = true;

		public bool Debug { get; set; } = false;

		public Dictionary<string, Dictionary<string, object>> Modifiers { get; set; } = new Dictionary<string, Dictionary<string, object>>();


		public static AggregateStateManager StateManager
								= new AggregateStateManager("BuildingModifier");
	}
}
