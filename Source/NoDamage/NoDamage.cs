using Harmony;
using UnityEngine;

namespace NoDamageMod
{


    internal class RoomSizeMod_OnLoad
    {
        public static void OnLoad(string modPath)
        {
            NoDamageConfig.LoadConfig(modPath);
        }
    }



    [HarmonyPatch(typeof(EventExtensions), "Trigger")]
    internal class NoOverloadedWiresMod_EventExtensions_Trigger
    {

        private static bool Prefix(GameObject go, GameHashes hash, object data)
        {
            //Debug.Log(" === NoOverloadedWiresMod_EventExtensions_Trigger Prefix === ");
            if (!NoDamageConfig.Config.Enabled)
                return true;

            if (hash == GameHashes.ConduitContentsBoiling
                && (NoDamageConfig.Config.DisableAllDamage 
                        || NoDamageConfig.Config.NoConduitContentsBoiling))
            {
                return false;
            }
            else if (hash == GameHashes.ConduitContentsFrozen
                && (NoDamageConfig.Config.DisableAllDamage
                        || NoDamageConfig.Config.NoConduitContentsFrozen))
            {
                return false;
            }
            else if (hash == GameHashes.DoBuildingDamage
                && (NoDamageConfig.Config.DisableAllDamage
                        || NoDamageConfig.Config.NoBuildingDamage))
            {
                return false;
            }
            return true;
            /*
            switch (hash)
            {
                case GameHashes.ConduitContentsBoiling:
                case GameHashes.ConduitContentsFrozen:
                case GameHashes.DoBuildingDamage:
                    return false;
                default:
                    break;
            }
            return true;
            */
        }
    }


    [HarmonyPatch(typeof(StructureTemperatureComponents), "DoOverheat")]
    internal class NoOverloadedWiresMod_StructureTemperatureComponents_DoOverheat
    {

        private static bool Prefix(int sim_handle)
        {
            //Debug.Log(" === NoOverloadedWiresMod_StructureTemperatureComponents_DoOverheat Prefix === ");
            if (!NoDamageConfig.Config.Enabled)
                return true;

            if (NoDamageConfig.Config.DisableAllDamage || NoDamageConfig.Config.NoBuildingOverheat)
                return false;

            return true;
        }
    }

    
    [HarmonyPatch(typeof(CircuitManager), "CheckCircuitOverloaded")]
    internal class NoDamageMod_CircuitManager_CheckCircuitOverloaded
    {

        private static bool Prefix(CircuitManager __instance, float dt, int id, float watts_used)
        {
            //Debug.Log(" === NoDamageMod_CircuitManager_CheckCircuitOverloaded Prefix === ");
            if (!NoDamageConfig.Config.Enabled)
                return true;

            if (NoDamageConfig.Config.DisableAllDamage || NoDamageConfig.Config.NoCircuitOverload)
                return false;

            return true;
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
