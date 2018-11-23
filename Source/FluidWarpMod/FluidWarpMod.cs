using System.Reflection;
using Harmony;
using UnityEngine;

namespace FluidWarpMod
{   
    internal class FluidWarpMod_Utils
	{
		public static ConduitType GetConduitType(ValveSideScreen __instance) {			
			FieldInfo fi1 = AccessTools.Field(typeof(ValveSideScreen), "targetValve");
			FieldInfo fi2 = AccessTools.Field(typeof(Valve), "valveBase");

			ConduitType type = ((ValveBase)fi2.GetValue(fi1.GetValue(__instance))).conduitType;
			return type;
		}
	}

	[HarmonyPatch(typeof(ValveBase), "ConduitUpdate")]
	internal class FluidWarpMod_ValveBase_ConduitUpdate
	{
        public FluidWarpMod_ValveBase_ConduitUpdate()
        {

        }
        private static bool Prefix(ValveBase __instance, float dt, int ___inputCell, int ___outputCell)
		{
            if (__instance.conduitType != LiquidWarpConfig.CONDUIT_TYPE && __instance.conduitType != GasWarpConfig.CONDUIT_TYPE)
            {
                return true;
            }

            int channelNo = Mathf.RoundToInt(__instance.CurrentFlow * 1000.0f); // simple cast to int sometimes gives invalid result
            Logger.LogFormat(" === ValveBase.ConduitUpdate({0}) Prefix conduitType={1}, inputCell={2}, outputCell={3}, channelNo={4}", dt, __instance.conduitType, ___inputCell, ___outputCell, channelNo);

            if (channelNo == 10000)
            {
                // Channel number is set to MaxFlow (10k), which means WarpGate is disabled
                return false;
            }

            ConduitFlow flowManager = null;
            if (__instance.conduitType == LiquidWarpConfig.CONDUIT_TYPE)
            {
                flowManager = Conduit.GetFlowManager(ConduitType.Liquid);
            }
            else if (__instance.conduitType == GasWarpConfig.CONDUIT_TYPE)
            {
                flowManager = Conduit.GetFlowManager(ConduitType.Gas);
            }

			if (!flowManager.HasConduit(___inputCell) || !flowManager.HasConduit(___outputCell))
			{
                __instance.UpdateAnim();
			}

            if (flowManager.HasConduit(___inputCell))
			{
                ConduitFlow.Conduit inputConduit = flowManager.GetConduit(___inputCell);
                ConduitFlow.ConduitContents contents = inputConduit.GetContents(flowManager);
				float num = Mathf.Min(contents.mass, (__instance.MaxFlow * dt));
				if (num > 0f)
				{
                    WarpPackageManager.ProvideConduitContents(flowManager, __instance.conduitType, ___inputCell, channelNo, num);
				}
				__instance.UpdateAnim();
				return false;
			}


			if (flowManager.HasConduit(___outputCell))
            {
                ConduitFlow.Conduit conduitO = flowManager.GetConduit(___outputCell);
                float actuallyAdded = WarpPackageManager.RequestElementFromChannel(flowManager, __instance.conduitType, ___outputCell, channelNo);
                Game.Instance.accumulators.Accumulate(__instance.AccumulatorHandle, actuallyAdded);

                __instance.UpdateAnim();
                return false;
            }

            return false;
		}
    }

	[HarmonyPatch(typeof(ValveSideScreen), "OnSpawn")]
    internal class FluidWarpMod_ValveSideScreen_OnSpawn
	{
        private static void Postfix(ValveSideScreen __instance)
        {
            Logger.LogFormat(" === FluidWarpMod_ValveSideScreen_OnSpawn Postfix === ");

			FieldInfo fi0 = AccessTools.Field(typeof(ValveSideScreen), "unitsLabel");
			ConduitType type = FluidWarpMod_Utils.GetConduitType(__instance);			
			if (type == LiquidWarpConfig.CONDUIT_TYPE || type == GasWarpConfig.CONDUIT_TYPE)
			{
				((LocText)fi0.GetValue(__instance)).text = "Ch.";
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
	internal class FluidWarpMod_ValveSideScreen_SetTarget
	{
		private static void Postfix(ValveSideScreen __instance)
		{
            Logger.LogFormat(" === FluidWarpMod_ValveSideScreen_SetTarget Postfix === ");

			FieldInfo fi3 = AccessTools.Field(typeof(ValveSideScreen), "minFlowLabel");
			FieldInfo fi4 = AccessTools.Field(typeof(ValveSideScreen), "maxFlowLabel");

			ConduitType type = FluidWarpMod_Utils.GetConduitType(__instance);
			if (type == LiquidWarpConfig.CONDUIT_TYPE || type == GasWarpConfig.CONDUIT_TYPE)
			{
				((LocText)fi3.GetValue(__instance)).text = "Channel";
				((LocText)fi4.GetValue(__instance)).text = "Channel";
			}

		}

	}

}
