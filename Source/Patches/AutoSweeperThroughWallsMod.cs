using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoSweeperThroughWallsMod
{
    [HarmonyPatch(typeof(Grid), "IsPhysicallyAccessible", null)]
    internal class AutoSweeperThroughWallsMod_Grid_IsPhysicallyAccessible
    {
        private static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
