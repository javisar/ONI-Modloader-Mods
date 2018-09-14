using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NoOverheatMod
{
    [HarmonyPatch(typeof(EventExtensions), "Trigger")]
    internal class NoOverloadedWiresMod_EventExtensions_Trigger
    {

        private static bool Prefix(GameObject go, GameHashes hash, object data)
        {
            //Debug.Log(" === NoOverloadedWiresMod_EventExtensions_Trigger Prefix === ");
            switch (hash) {
                case GameHashes.ConduitContentsBoiling:
                case GameHashes.ConduitContentsFrozen:
                //case GameHashes.DoBuildingDamage:
                    return false;
                default:
                    break;
            }
            return true;
        }
    }

    
    [HarmonyPatch(typeof(StructureTemperatureComponents), "DoOverheat")]
    internal class NoOverloadedWiresMod_StructureTemperatureComponents_DoOverheat
    {

        private static bool Prefix(int sim_handle)
        {
            //Debug.Log(" === NoOverloadedWiresMod_StructureTemperatureComponents_DoOverheat Prefix === ");

            return false;
        }
    }
    /*
    [HarmonyPatch(typeof(ConduitFlow), "FreezeConduitContents")]
    internal class NoOverloadedWiresMod_ConduitFlow_FreezeConduitContents
    {

        private static bool Prefix(int conduit_idx)
        {
            //Debug.Log(" === NoOverloadedWiresMod_ConduitFlow_FreezeConduitContents Prefix === ");

            return false;
        }
    }

    [HarmonyPatch(typeof(ConduitFlow), "MeltConduitContents")]
    internal class NoOverloadedWiresMod_ConduitFlow_MeltConduitContents
    {

        private static bool Prefix(int conduit_idx)
        {
            //Debug.Log(" === NoOverloadedWiresMod_ConduitFlow_FreezeConduitContents Prefix === ");

            return false;
        }
    }
    */
}
