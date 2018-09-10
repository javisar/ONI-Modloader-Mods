using System.Collections.Generic;
using ONI_Common.Json;

namespace BuildingModifierMod
{
    public class BuildingModifierState
    {
        public bool Enabled { get; set; } = true;

        public Dictionary<string, Dictionary<string, Dictionary<string, int>>> Modifiers { get; set; } = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();


		public static BaseStateManager<BuildingModifierState> StateManager
			                    = new BaseStateManager<BuildingModifierState>("BuildingModifier");
	}
}