using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;

namespace FluidWarpMod
{
       
    public static class LiquidWarpData
    {
        public static List<PacketData> LiquidPackets = new List<PacketData>();

    }

    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class InverseElectrolyzerMod
    {
        private static void Prefix()
        {
            Debug.Log(" === GeneratedBuildings Prefix === " + LiquidWarpConfig.ID);
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDWARP.NAME", "Liquid Stargate");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDWARP.DESC", "");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDWARP.EFFECT", "");

            List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
            ls.Add(LiquidWarpConfig.ID);
            TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

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
    internal class InverseElectrolyzerTechMod
    {
        private static void Prefix(Db __instance)
        {
            Debug.Log(" === Database.Techs loaded === " + LiquidWarpConfig.ID);
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["ImprovedGasPiping"]);
            ls.Add(LiquidWarpConfig.ID);
            Database.Techs.TECH_GROUPING["ImprovedGasPiping"] = (string[])ls.ToArray();

            //Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
        }
    }

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
