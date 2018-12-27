using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace MoreFood
{
    [HarmonyPatch(typeof(Game), "OnPrefabInit")]
    internal class MoreFood_Game_OnPrefabInit
    {

        private static void Postfix(Game __instance)
        {
            if (!MoreFoodState.StateManager.State.Enabled) return;

            Debug.Log(" === MoreFood_Game_OnPrefabInit Postfix === ");

        }
    }
}
