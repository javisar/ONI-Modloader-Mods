using System;
using System.Linq;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace FluidWarpMod
{
    internal static class FluidWarpMod_Utils
    {
        public const ConduitType LIQUID_CONDUIT_PROVIDER_TYPE = (ConduitType)100;
        public const ConduitType GAS_CONDUIT_PROVIDER_TYPE = (ConduitType)101;
        public const ConduitType LIQUID_CONDUIT_REQUESTER_TYPE = (ConduitType)102;
        public const ConduitType GAS_CONDUIT_REQUESTER_TYPE = (ConduitType)103;
        public const string CHANNEL_LABEL = "Ch.";

        public const string OFF_LABEL = "(off)";

        private static FieldInfo valveSideScreenTargetValveFI = AccessTools.Field(typeof(ValveSideScreen), "targetValve");

        private static FieldInfo valveValveBaseFI = AccessTools.Field(typeof(Valve), "valveBase");

        public static ValveBase GetValveBaseFromScreen(ValveSideScreen __instance)
        {
            return ((ValveBase)valveValveBaseFI.GetValue(valveSideScreenTargetValveFI.GetValue(__instance)));
        }

        public static bool IsWarpGate(ValveBase vb)
        {
            return (vb.conduitType == FluidWarpMod_Utils.GAS_CONDUIT_PROVIDER_TYPE
                || vb.conduitType == FluidWarpMod_Utils.LIQUID_CONDUIT_PROVIDER_TYPE
                || vb.conduitType == FluidWarpMod_Utils.LIQUID_CONDUIT_REQUESTER_TYPE
                || vb.conduitType == FluidWarpMod_Utils.GAS_CONDUIT_REQUESTER_TYPE);
        }
    }

    [HarmonyPatch(typeof(Valve), "OnSpawn")]
    internal static class FluidWarpMod_Valve_OnSpawn
    {
        [HarmonyPostfix]
        private static void Postfix(Valve __instance, ValveBase ___valveBase)
        {
            if (FluidWarpMod_Utils.IsWarpGate(___valveBase))
            {
                WarpSpaceManager.OnWarpGateChannelChange(___valveBase);
                WarpSpaceManager.RegisterConduitUpdater();
            }
        }
    }

    [HarmonyPatch(typeof(Valve), "OnCleanUp")]
    internal static class FluidWarpMod_Valve_OnCleanUp
    {
        [HarmonyPostfix]
        private static void Postfix(Valve __instance, ValveBase ___valveBase)
        {
            if (FluidWarpMod_Utils.IsWarpGate(___valveBase))
            {
                WarpSpaceManager.RemoveWarpGate(___valveBase);
            }
        }
    }

    [HarmonyPatch(typeof(Valve), "UpdateFlow")]
    internal static class FluidWarpMod_Valve_UpdateFlow
    {
        [HarmonyPostfix]
        private static void Postfix(Valve __instance, ValveBase ___valveBase)
        {
            if (FluidWarpMod_Utils.IsWarpGate(___valveBase))
            {
                WarpSpaceManager.OnWarpGateChannelChange(___valveBase);
            }
        }
    }

    [HarmonyPatch(typeof(Valve), "OnCopySettings")]
    internal static class FluidWarpMod_Valve_OnCopySettings
    {
        [HarmonyPrefix]
        private static bool Prefix(Valve __instance, object data)
        {
            if (FluidWarpMod_Utils.IsWarpGate(__instance.GetValveBase()))
            {
                GameObject gameObject = (GameObject)data;
                Valve component = gameObject.GetComponent<Valve>();
                if (component != null && FluidWarpMod_Utils.IsWarpGate(component.GetValveBase()))
                {
                    __instance.ChangeFlow(component.GetValveBase().CurrentFlow);
                }
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(ValveBase), "ConduitUpdate")]
    internal static class FluidWarpMod_ValveBase_ConduitUpdate
    {
        [HarmonyPrefix]
        private static bool Prefix(ValveBase __instance, float dt, int ___inputCell, int ___outputCell)
        {
            if (FluidWarpMod_Utils.IsWarpGate(__instance))
            {
                __instance.UpdateAnim();
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(ValveBase), "UpdateAnim")]
    internal class FluidWarpMod_ValveBase_UpdateAnim
    {
        private static FieldInfo controllerFI = AccessTools.Field(typeof(ValveBase), "controller");

        [HarmonyPrefix]
        public static bool Prefix(ValveBase __instance)
        {
            if (FluidWarpMod_Utils.IsWarpGate(__instance))
            {
                var controller = ((KBatchedAnimController)controllerFI.GetValue(__instance));
                float averageRate = Game.Instance.accumulators.GetAverageRate(__instance.AccumulatorHandle);
                if (averageRate <= 0f)
                {
                    controller.Play("off", KAnim.PlayMode.Once, 1f, 0f);
                }
                else
                {
                    controller.Play("hi", (averageRate > 0f ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once), 1f, 0f);
                }
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(ValveSideScreen), "OnSpawn")]
    internal class FluidWarpMod_ValveSideScreen_OnSpawn
    {
        private static FieldInfo numberInputFI = AccessTools.Field(typeof(ValveSideScreen), "numberInput");

        private static FieldInfo unitsLabelFI = AccessTools.Field(typeof(ValveSideScreen), "unitsLabel");

        [HarmonyPostfix]
        private static void Postfix(ValveSideScreen __instance)
        {
            Logger.LogFormat(" === FluidWarpMod_ValveSideScreen_OnSpawn Postfix === ");
            
            if (FluidWarpMod_Utils.IsWarpGate(FluidWarpMod_Utils.GetValveBaseFromScreen(__instance)))
            {
                if ((float)((KNumberInputField)numberInputFI.GetValue(__instance)).currentValue == 10f)
                {
                    ((LocText)unitsLabelFI.GetValue(__instance)).text = FluidWarpMod_Utils.OFF_LABEL;
                }
                else
                {
                    ((LocText)unitsLabelFI.GetValue(__instance)).text = FluidWarpMod_Utils.CHANNEL_LABEL;
                }
            }
        }
    }

    [HarmonyPatch(typeof(SideScreenContent), "GetTitle")]
    internal class FluidWarpMod_SideScreenContent_GetTitle
    {
        [HarmonyPrefix]
        private static bool Prefix(SideScreenContent __instance, ref string __result)
        {
            Logger.LogFormat(" === FluidWarpMod_SideScreenContent_GetTitle Postfix === ");

            if (!(__instance is ValveSideScreen)) return true;
            
            if (FluidWarpMod_Utils.IsWarpGate(FluidWarpMod_Utils.GetValveBaseFromScreen((ValveSideScreen)__instance)))
            {
                __result = "Channel";
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(ValveSideScreen), "SetTarget")]
    internal static class FluidWarpMod_ValveSideScreen_SetTarget
    {
        private static FieldInfo minFlowLabelFI = AccessTools.Field(typeof(ValveSideScreen), "minFlowLabel");
        private static FieldInfo maxFlowLabelFI = AccessTools.Field(typeof(ValveSideScreen), "maxFlowLabel");

        [HarmonyPostfix]
        private static void Postfix(ValveSideScreen __instance)
        {
            Logger.LogFormat(" === FluidWarpMod_ValveSideScreen_SetTarget Postfix === ");
            var valveBase = FluidWarpMod_Utils.GetValveBaseFromScreen(__instance);
            if (FluidWarpMod_Utils.IsWarpGate(valveBase))
            {

                ((LocText)minFlowLabelFI.GetValue(__instance)).text = "-Channel";
                ((LocText)maxFlowLabelFI.GetValue(__instance)).text = "+Channel";
            }
        }
    }

    [HarmonyPatch(typeof(ValveSideScreen), "UpdateFlowValue")]
    internal class FluidWarpMod_ValveSideScreen_UpdateFlowValue
    {
        private static FieldInfo unitsLabelFI = AccessTools.Field(typeof(ValveSideScreen), "unitsLabel");

        [HarmonyPostfix]
        private static void Postfix(ValveSideScreen __instance, float newValue)
        {
            if (FluidWarpMod_Utils.IsWarpGate(FluidWarpMod_Utils.GetValveBaseFromScreen(__instance)))
            {
                if (newValue == 10f)
                {
                    ((LocText)unitsLabelFI.GetValue(__instance)).text = FluidWarpMod_Utils.OFF_LABEL;
                }
                else
                {
                    ((LocText)unitsLabelFI.GetValue(__instance)).text = FluidWarpMod_Utils.CHANNEL_LABEL;
                }
            }
        }
    }
}