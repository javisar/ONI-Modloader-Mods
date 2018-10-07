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
          
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class LiquidWarpMod_GeneratedBuildings_LoadGeneratedBuildings
	{
        private static void Prefix()
        {
            Debug.Log(" === GeneratedBuildings Prefix === " + LiquidWarpConfig.ID);
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDWARP.NAME", "Liquid Stargate");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDWARP.DESC", "Liquid Stargates provides an easy way to transport liquids from one place to another.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDWARP.EFFECT", "Place one providing input fluid, and another one with an output pipe. Sintonize your stargates using the same channel.");

            List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[4].data);
            ls.Add(LiquidWarpConfig.ID);
            TUNING.BUILDINGS.PLANORDER[4].data = (string[])ls.ToArray();          

            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(LiquidWarpConfig.ID);


        }
        private static void Postfix()
        {

            Debug.Log(" === GeneratedBuildings Postfix === " + LiquidWarpConfig.ID);
            object obj = Activator.CreateInstance(typeof(LiquidWarpConfig));
            BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class LiquidWarpMod_Db_Initialize
	{
        private static void Prefix(Db __instance)
        {
            Debug.Log(" === Database.Techs loaded === " + LiquidWarpConfig.ID);
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["ImprovedLiquidPiping"]);
            ls.Add(LiquidWarpConfig.ID);
            Database.Techs.TECH_GROUPING["ImprovedLiquidPiping"] = (string[])ls.ToArray();

            //Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
        }
    }
	/*
	[HarmonyPatch(typeof(ValveBase), "ConduitUpdate")]
	internal class LiquidWarpMod_ValveBase_ConduitUpdate
	{
		private static bool Prefix(ValveBase __instance, float dt)
		{
			Debug.Log(" === ValveBase.ConduitUpdate("+dt+") Prefix "+ __instance.conduitType);
			if (__instance.conduitType != (ConduitType)100) return true;

			FieldInfo fi1 = AccessTools.Field(typeof(ValveBase), "inputCell");
			FieldInfo fi2 = AccessTools.Field(typeof(ValveBase), "outputCell");
			FieldInfo fi3 = AccessTools.Field(typeof(ValveBase), "flowAccumulator");

			//Debug.Log("ConduitUpdate " + dt);
			ConduitFlow flowManager = Conduit.GetFlowManager(__instance.conduitType);
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
				Debug.Log("ConduitUpdate " + num);
				if (num > 0f)
				{

					float num2 = num / contents.mass;
					int disease_count = (int)(num2 * (float)contents.diseaseCount);
					Debug.Log("List " + num);

					LiquidWarpData.LiquidPackets.Add(new PacketData((int)__instance.conduitType, (float)fi.GetValue(__instance), (int)fi2.GetValue(__instance), contents.element, num, contents.temperature, contents.diseaseIdx, disease_count));
					
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
					Debug.Log("currentFlow = " + (float)fi.GetValue(__instance) + ", packet.currentFlow = " + packet.current_flow);
					if ((float)fi.GetValue(__instance) == packet.current_flow
						&& (int)__instance.conduitType == packet.content_type)
					{
						float num3 = flowManager.AddElement((int)fi2.GetValue(__instance), packet.element, packet.mass, packet.temperature, packet.disease_idx, packet.disease_count);
						Debug.Log("Adding Element to pipe: " + packet.mass + "," + num3);
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


				__instance.UpdateAnim();
				return false;
			}

			return false;
		}
	}
	*/
	/*
	[HarmonyPatch(typeof(ValveSideScreen), "OnReleaseHandle")]
	internal class ValveSideScreen1
	{
		private static bool Prefix(ValveSideScreen __instance)
		{
			Debug.Log(" === ValveSideScreen.OnReleaseHandle ( === ");
			FieldInfo fi1 = AccessTools.Field(typeof(ValveSideScreen), "targetValve");
			FieldInfo fi2 = AccessTools.Field(typeof(ValveSideScreen), "targetFlow");

			if (fi1.GetValue(__instance) is ValveStarGate)
			{
				((ValveStarGate)fi1.GetValue(__instance)).ChangeFlow((float)fi2.GetValue(__instance));
				return false;
			}			
			return true;			
		}
	}

	[HarmonyPatch(typeof(ValveSideScreen), "ReceiveValueFromInput")]
	internal class ValveSideScreen2
	{
		private static bool Prefix(ValveSideScreen __instance, float input)
		{
			Debug.Log(" === ValveSideScreen.ReceiveValueFromInput ( === ");
			FieldInfo fi1 = AccessTools.Field(typeof(ValveSideScreen), "targetValve");
			FieldInfo fi2 = AccessTools.Field(typeof(ValveSideScreen), "targetFlow");
			MethodInfo mi1 = AccessTools.Method(typeof(ValveSideScreen), "UpdateFlowValue", new Type[] { typeof(float) });

			if (fi1.GetValue(__instance) is ValveStarGate)
			{
				float newValue = input / 1000f;
				mi1.Invoke(__instance, new object[] { newValue });
				((ValveStarGate)fi1.GetValue(__instance)).ChangeFlow((float)fi2.GetValue(__instance));
				
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(ValveSideScreen), "SetTarget")]
	internal class ValveSideScreen3
	{
		private static bool Prefix(ValveSideScreen __instance, GameObject target)
		{
			Debug.Log(" === ValveSideScreen.SetTarget ( === ");
			FieldInfo fi1 = AccessTools.Field(typeof(ValveSideScreen), "targetValve");
			FieldInfo fi2 = AccessTools.Field(typeof(ValveSideScreen), "targetFlow");
			FieldInfo fi3 = AccessTools.Field(typeof(ValveSideScreen), "flowSlider");
			FieldInfo fi4 = AccessTools.Field(typeof(ValveSideScreen), "minFlowLabel");
			FieldInfo fi5 = AccessTools.Field(typeof(ValveSideScreen), "maxFlowLabel");
			FieldInfo fi6 = AccessTools.Field(typeof(ValveSideScreen), "numberInput");
			MethodInfo mi1 = AccessTools.Method(typeof(ValveSideScreen), "UpdateFlowValue", new Type[] { typeof(float) });
			Debug.Log("1");
			if (target.GetComponent<ValveStarGate>() != null)
			{
				Debug.Log("2");
				fi1.SetValue(__instance, target.GetComponent<ValveStarGate>());
				Debug.Log("3");
				if ((UnityEngine.Object)fi1.GetValue(__instance) == (UnityEngine.Object)null)
				{
					Debug.LogError("The target object does not have a Valve component.", null);
				}
				else
				{
					Debug.Log("4");
					((KSlider)fi3.GetValue(__instance)).minValue = 0f;
					((KSlider)fi3.GetValue(__instance)).maxValue = ((ValveStarGate)fi1.GetValue(__instance)).MaxFlow;
					((KSlider)fi3.GetValue(__instance)).value = ((ValveStarGate)fi1.GetValue(__instance)).DesiredFlow;
					((LocText)fi4.GetValue(__instance)).text = GameUtil.GetFormattedMass(0f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
					((LocText)fi5.GetValue(__instance)).text = GameUtil.GetFormattedMass(((ValveStarGate)fi1.GetValue(__instance)).MaxFlow, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
					Debug.Log("5");
					((KNumberInputField)fi6.GetValue(__instance)).minValue = 0f;
					((KNumberInputField)fi6.GetValue(__instance)).maxValue = ((ValveStarGate)fi1.GetValue(__instance)).MaxFlow * 1000f;
					((KNumberInputField)fi6.GetValue(__instance)).SetDisplayValue(GameUtil.GetFormattedMass(Mathf.Max(0f, ((ValveStarGate)fi1.GetValue(__instance)).DesiredFlow), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
					((KNumberInputField)fi6.GetValue(__instance)).Activate();
					Debug.Log("6");
				}
				Debug.Log("7");
				return false;
			}
			return true;
		}
	}
	
	[HarmonyPatch(typeof(ValveSideScreen), "IsValidForTarget")]
	internal class ValveSideScreen4
	{
		private static bool Prefix(ValveSideScreen __instance, ref bool __result, GameObject target)
		{
			Debug.Log(" === ValveSideScreen.IsValidForTarget ( === ");
			FieldInfo fi1 = AccessTools.Field(typeof(ValveSideScreen), "targetValve");
			Debug.Log(" === ValveSideScreen.IsValidForTarget ( === " + fi1.GetValue(__instance));
			if (fi1.GetValue(__instance) is ValveStarGate)
			{
				__result = (UnityEngine.Object)target.GetComponent<ValveStarGate>() != (UnityEngine.Object)null;
				Debug.Log(" === ValveSideScreen.IsValidForTarget ( === "+ __result);
				return false;
			}
			return true;
		}
	}
	*/


}
