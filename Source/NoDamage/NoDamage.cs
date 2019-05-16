using Harmony;
using UnityEngine;

namespace NoDamageMod
{

    [HarmonyPatch(typeof(EventExtensions), "Trigger")]
    internal class NoOverloadedWiresMod_EventExtensions_Trigger
    {

        private static bool Prefix(GameObject go, GameHashes hash, object data)
        {
            //Debug.Log(" === NoOverloadedWiresMod_EventExtensions_Trigger Prefix === ");
            if (!NoDamageConfig.Instance.Enabled)
                return true;

            if (hash == GameHashes.ConduitContentsBoiling
                && (NoDamageConfig.Instance.DisableAllDamage 
                        || NoDamageConfig.Instance.NoConduitContentsBoiling))
            {
                return false;
            }
            else if (hash == GameHashes.ConduitContentsFrozen
                && (NoDamageConfig.Instance.DisableAllDamage
                        || NoDamageConfig.Instance.NoConduitContentsFrozen))
            {
                return false;
            }
            else if (hash == GameHashes.DoBuildingDamage
                && (NoDamageConfig.Instance.DisableAllDamage
                        || NoDamageConfig.Instance.NoBuildingDamage))
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
            if (!NoDamageConfig.Instance.Enabled)
                return true;

            if (NoDamageConfig.Instance.DisableAllDamage || NoDamageConfig.Instance.NoBuildingOverheat)
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
            if (!NoDamageConfig.Instance.Enabled)
                return true;

            if (NoDamageConfig.Instance.DisableAllDamage || NoDamageConfig.Instance.NoCircuitOverload)
                return false;

            return true;
        }
    }


	/*
    internal class RoomSizeMod_OnLoad
    {
        public static void OnLoad(string modPath)
        {
            NoDamageConfig.LoadConfig(modPath);
        }
    }
	*/


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
