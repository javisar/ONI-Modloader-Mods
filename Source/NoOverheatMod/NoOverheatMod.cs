using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoOverheatMod
{
    [HarmonyPatch(typeof(StructureTemperatureComponents), "DoOverheat")]
    internal class NoOverloadedWiresMod_StructureTemperatureComponents_DoOverheat
    {

        private static bool Prefix(int sim_handle)
        {
            //Debug.Log(" === NoOverloadedWiresMod_StructureTemperatureComponents_DoOverheat Prefix === ");

            return false;
        }
    }
}
