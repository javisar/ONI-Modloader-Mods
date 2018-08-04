using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;
using WarpMod;

namespace LiquidWarpMod
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
		private static bool Prefix(ValveBase __instance, float dt)
		{
			//Debug.Log(" === ValveBase.ConduitUpdate(" + dt + ") Prefix " + __instance.conduitType);
			if (__instance.conduitType != (ConduitType)100 && __instance.conduitType != (ConduitType)101) return true;

			FieldInfo fi1 = AccessTools.Field(typeof(ValveBase), "inputCell");
			FieldInfo fi2 = AccessTools.Field(typeof(ValveBase), "outputCell");
			FieldInfo fi3 = AccessTools.Field(typeof(ValveBase), "flowAccumulator");

            ConduitFlow flowManager = null;
            //Debug.Log("ConduitUpdate " + dt);
            if (__instance.conduitType == (ConduitType)100)
            {
                flowManager = Conduit.GetFlowManager(ConduitType.Liquid);
            }
            else if (__instance.conduitType == (ConduitType)101)
            {
                flowManager = Conduit.GetFlowManager(ConduitType.Gas);
            }
            
			ConduitFlow.Conduit conduit = flowManager.GetConduit((int)fi1.GetValue(__instance));

			if (!flowManager.HasConduit((int)fi1.GetValue(__instance)) || !flowManager.HasConduit((int)fi2.GetValue(__instance)))
			{
				__instance.UpdateAnim();
			}

			if ((int)fi1.GetValue(__instance) > 0 && flowManager.HasConduit((int)fi1.GetValue(__instance)) &&
				((int)fi2.GetValue(__instance) > 0 && !flowManager.HasConduit((int)fi2.GetValue(__instance))))
			{
				ConduitFlow.ConduitContents contents = conduit.GetContents(flowManager);
				//float num = Mathf.Min(contents.mass, this.currentFlow * dt);
				FieldInfo fi = AccessTools.Field(typeof(ValveBase), "currentFlow");
				//float num = Mathf.Min(contents.mass, (float)fi.GetValue(this) * dt);
				float num = Mathf.Min(contents.mass, 10f * dt);
				//Debug.Log("ConduitUpdate " + num);
				if (num > 0f)
				{

					float num2 = num / contents.mass;
					int disease_count = (int)(num2 * (float)contents.diseaseCount);
                    //Debug.Log("List " + num);

                    if (__instance.conduitType == (ConduitType)100)
                    {
                        LiquidWarpData.LiquidPackets.Add(new PacketData((int)__instance.conduitType, (float)fi.GetValue(__instance), (int)fi2.GetValue(__instance), contents.element, num, contents.temperature, contents.diseaseIdx, disease_count));
                    }
                    else if (__instance.conduitType == (ConduitType)101)
                    {
                        GasWarpData.GasPackets.Add(new PacketData((int)__instance.conduitType, (float)fi.GetValue(__instance), (int)fi2.GetValue(__instance), contents.element, num, contents.temperature, contents.diseaseIdx, disease_count));
                    }
					//float num3 = flowManager.AddElement(this.outputCell, contents.element, num, contents.temperature, contents.diseaseIdx, disease_count);
					//Game.Instance.accumulators.Accumulate(this.flowAccumulator, num3);				

					//float num3 = Mathf.Min(num, 10f - contents.mass);
					float num3 = num;
					if (num3 > 0f)
					{
						flowManager.RemoveElement((int)fi1.GetValue(__instance), num3);
					}
				}
				__instance.UpdateAnim();
				return false;
			}


			if ((int)fi2.GetValue(__instance) > 0 && flowManager.HasConduit((int)fi2.GetValue(__instance)))
			{
				ConduitFlow.Conduit conduitO = flowManager.GetConduit((int)fi2.GetValue(__instance));
				FieldInfo fi = AccessTools.Field(typeof(ValveBase), "currentFlow");

				PacketData toRemove = null;

				foreach (PacketData packet in LiquidWarpData.LiquidPackets)
				{
					//Debug.Log("currentFlow = " + (float)fi.GetValue(__instance) + ", packet.currentFlow = " + packet.current_flow);
					if ((float)fi.GetValue(__instance) == packet.current_flow
						&& (int)__instance.conduitType == packet.content_type)
					{
						float num3 = flowManager.AddElement((int)fi2.GetValue(__instance), packet.element, packet.mass, packet.temperature, packet.disease_idx, packet.disease_count);
						//Debug.Log("Adding Element to pipe: " + packet.mass + "," + num3);
						Game.Instance.accumulators.Accumulate((HandleVector<int>.Handle)fi3.GetValue(__instance), num3);
						toRemove = packet;
						break;
					}
				}

				if (toRemove != null)
				{
					LiquidWarpData.LiquidPackets.Remove(toRemove);
					toRemove = null;
				}

                foreach (PacketData packet in GasWarpData.GasPackets)
                {
                    //Debug.Log("currentFlow = " + (float)fi.GetValue(__instance) + ", packet.currentFlow = " + packet.current_flow);
                    if ((float)fi.GetValue(__instance) == packet.current_flow
                        && (int)__instance.conduitType == packet.content_type)
                    {
                        float num3 = flowManager.AddElement((int)fi2.GetValue(__instance), packet.element, packet.mass, packet.temperature, packet.disease_idx, packet.disease_count);
                        //Debug.Log("Adding Element to pipe: " + packet.mass + "," + num3);
                        Game.Instance.accumulators.Accumulate((HandleVector<int>.Handle)fi3.GetValue(__instance), num3);
                        toRemove = packet;
                        break;
                    }
                }

                if (toRemove != null)
                {
                    GasWarpData.GasPackets.Remove(toRemove);
                    toRemove = null;
                }

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
            Debug.Log(" === FluidWarpMod_ValveSideScreen_OnSpawn Postfix === ");

			FieldInfo fi0 = AccessTools.Field(typeof(ValveSideScreen), "unitsLabel");
			ConduitType type = FluidWarpMod_Utils.GetConduitType(__instance);			
			if (type == (ConduitType)100 || type == (ConduitType)101)
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
			Debug.Log(" === FluidWarpMod_SideScreenContent_GetTitle Postfix === ");

			if (!(__instance is ValveSideScreen)) return true;

			ConduitType type = FluidWarpMod_Utils.GetConduitType((ValveSideScreen)__instance);
			if (type == (ConduitType)100 || type == (ConduitType)101)
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
			Debug.Log(" === FluidWarpMod_ValveSideScreen_SetTarget Postfix === ");

			FieldInfo fi3 = AccessTools.Field(typeof(ValveSideScreen), "minFlowLabel");
			FieldInfo fi4 = AccessTools.Field(typeof(ValveSideScreen), "maxFlowLabel");

			ConduitType type = FluidWarpMod_Utils.GetConduitType(__instance);
			if (type == (ConduitType)100 || type == (ConduitType)101)
			{
				((LocText)fi3.GetValue(__instance)).text = "Ch.";
				((LocText)fi4.GetValue(__instance)).text = "Ch.";
			}

		}

	}

}
