using System.Collections.Generic;
using ONI_Common.Json;

namespace VentPressureMod
{
    public class VentPressureState
	{
        public bool Enabled { get; set; } = true;

        public Dictionary<string, float> MaximumPressure { get; set; } = new Dictionary<string, float>();


		public static BaseStateManager<VentPressureState> StateManager
			= new BaseStateManager<VentPressureState>("VentPressure");
	}
}