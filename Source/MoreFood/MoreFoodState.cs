using System.Collections.Generic;
using ONI_Common.Json;

namespace MoreFood
{
    public class MoreFoodState
    {
        public bool Enabled { get; set; } = true;

        public int Parameter { get; set; } = 512;


        public static BaseStateManager<MoreFoodState> StateManager
            = new BaseStateManager<MoreFoodState>("MoreFood");
    }
}