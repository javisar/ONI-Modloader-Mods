using System.Collections.Generic;
using ONI_Common.Json;

namespace AdvancedFluidDistribution
{
    public class AdvFluidDistribState
    {
        public bool Enabled { get; set; } = true;

		public static BaseStateManager<AdvFluidDistribState> StateManager
			= new BaseStateManager<AdvFluidDistribState>("AdvFluidDistr");
	}
}