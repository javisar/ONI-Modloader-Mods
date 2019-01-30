using System;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace FluidWarpMod
{   
    internal static class FluidWarpMod_Utils
	{
        public const string CHANNEL_LABEL = "Ch.";

        public const string OFF_LABEL = "(off)";

        private static FieldInfo valveSideScreenTargetValveFI = AccessTools.Field(typeof(ValveSideScreen), "targetValve");

        private static FieldInfo valveValveBaseFI = AccessTools.Field(typeof(Valve), "valveBase");

        public static ConduitType GetConduitType(ValveSideScreen __instance) {			
			ConduitType type = ((ValveBase)valveValveBaseFI.GetValue( valveSideScreenTargetValveFI.GetValue(__instance) )).conduitType;
			return type;
		}
	}

    [HarmonyPatch(typeof(Valve), "OnSpawn")]
    internal static class FluidWarpMod_Valve_OnSpawn
    {
        private static void Postfix(Valve __instance, ValveBase ___valveBase)
        {
            if (___valveBase.conduitType == GasWarpConfig.CONDUIT_TYPE || ___valveBase.conduitType == LiquidWarpConfig.CONDUIT_TYPE)
            {
                WarpSpaceManager.OnValveChannelChange(___valveBase);
                WarpSpaceManager.RegisterConduitUpdater();
            }
        }
    }

    [HarmonyPatch(typeof(Valve), "OnCleanUp")]
    internal static class FluidWarpMod_Valve_OnCleanUp
    {
        private static void Postfix(Valve __instance, ValveBase ___valveBase)
        {
            if (___valveBase.conduitType == GasWarpConfig.CONDUIT_TYPE || ___valveBase.conduitType == LiquidWarpConfig.CONDUIT_TYPE)
            {
                WarpSpaceManager.RemoveProviderValve(___valveBase);
            }
        }
    }


    [HarmonyPatch(typeof(Valve), "UpdateFlow")]
    internal static class FluidWarpMod_Valve_UpdateFlow
    {
        [HarmonyPrefix]
        private static void Postfix(Valve __instance, ValveBase ___valveBase)
        {
            if (___valveBase.conduitType == GasWarpConfig.CONDUIT_TYPE || ___valveBase.conduitType == LiquidWarpConfig.CONDUIT_TYPE)
            {
                WarpSpaceManager.OnValveChannelChange(___valveBase);
            }
        }
    }

    [HarmonyPatch(typeof(Valve), "OnCopySettings")]
    internal static class FluidWarpMod_Valve_OnCopySettings
    {
        [HarmonyPrefix]
        private static bool Prefix(Valve __instance, object data)
        {
            if (__instance.GetValveBase().conduitType == GasWarpConfig.CONDUIT_TYPE || __instance.GetValveBase().conduitType == LiquidWarpConfig.CONDUIT_TYPE)
            {
                GameObject gameObject = (GameObject)data;
                Valve component = gameObject.GetComponent<Valve>();
                if (component != null && (component.GetValveBase().conduitType == GasWarpConfig.CONDUIT_TYPE || component.GetValveBase().conduitType == LiquidWarpConfig.CONDUIT_TYPE))
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
            if (__instance.conduitType == LiquidWarpConfig.CONDUIT_TYPE || __instance.conduitType == GasWarpConfig.CONDUIT_TYPE)
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
        public static bool Prefix(ValveBase __instance)
        {
            if (__instance.conduitType == LiquidWarpConfig.CONDUIT_TYPE || __instance.conduitType == GasWarpConfig.CONDUIT_TYPE)
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

        private static void Postfix(ValveSideScreen __instance)
        {
            Logger.LogFormat(" === FluidWarpMod_ValveSideScreen_OnSpawn Postfix === ");

			ConduitType type = FluidWarpMod_Utils.GetConduitType(__instance);			
			if (type == LiquidWarpConfig.CONDUIT_TYPE || type == GasWarpConfig.CONDUIT_TYPE)
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
		private static bool Prefix(SideScreenContent __instance, ref string __result)
		{
            Logger.LogFormat(" === FluidWarpMod_SideScreenContent_GetTitle Postfix === ");

			if (!(__instance is ValveSideScreen)) return true;

			ConduitType type = FluidWarpMod_Utils.GetConduitType((ValveSideScreen)__instance);
			if (type == LiquidWarpConfig.CONDUIT_TYPE || type == GasWarpConfig.CONDUIT_TYPE)
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
        private static void Postfix(ValveSideScreen __instance)
		{
            Logger.LogFormat(" === FluidWarpMod_ValveSideScreen_SetTarget Postfix === ");
            
			ConduitType type = FluidWarpMod_Utils.GetConduitType(__instance);
			if (type == LiquidWarpConfig.CONDUIT_TYPE || type == GasWarpConfig.CONDUIT_TYPE)
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

        private static void Postfix(ValveSideScreen __instance, float newValue)
        {
            ConduitType type = FluidWarpMod_Utils.GetConduitType(__instance);
            if ((type == LiquidWarpConfig.CONDUIT_TYPE || type == GasWarpConfig.CONDUIT_TYPE))
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
