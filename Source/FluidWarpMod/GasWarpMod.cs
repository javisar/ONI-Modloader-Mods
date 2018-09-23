using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;
using WarpMod;

namespace GasWarpMod
{
       
    
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class GasWarpMod_GeneratedBuildings_LoadGeneratedBuildings
	{
        private static void Prefix()
        {
            Debug.Log(" === GeneratedBuildings Prefix === " + GasWarpConfig.ID);
            Strings.Add("STRINGS.BUILDINGS.PREFABS.GASWARP.NAME", "Gas Stargate");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.GASWARP.DESC", "Gas Stargates provides an easy way to transport gases from one place to another.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.GASWARP.EFFECT", "Place one providing input fluid, and another one with an output pipe. Sintonize your stargates using the same channel.");

            List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[6].data);
            ls.Add(GasWarpConfig.ID);
            TUNING.BUILDINGS.PLANORDER[6].data = (string[])ls.ToArray();

            TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(GasWarpConfig.ID);


        }
        private static void Postfix()
        {

            Debug.Log(" === GeneratedBuildings Postfix === " + GasWarpConfig.ID);
            object obj = Activator.CreateInstance(typeof(GasWarpConfig));
            BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class GasWarpMod_Db_Initialize
	{
        private static void Prefix(Db __instance)
        {
            Debug.Log(" === Database.Techs loaded === " + GasWarpConfig.ID);
            List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["ImprovedGasPiping"]);
            ls.Add(GasWarpConfig.ID);
            Database.Techs.TECH_GROUPING["ImprovedGasPiping"] = (string[])ls.ToArray();

            //Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
        }
    }
	/*
	[HarmonyPatch(typeof(ValveBase), "ConduitUpdate")]
	internal class GasWarpMod_ValveBase_ConduitUpdate
	{
		private static bool Prefix(ValveBase __instance, float dt)
		{
			Debug.Log(" === ValveBase.ConduitUpdate("+dt+") Prefix "+ __instance.conduitType);
			if (__instance.conduitType != (ConduitType)101) return true;

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

					GasWarpData.GasPackets.Add(new PacketData((int)__instance.conduitType, (float)fi.GetValue(__instance), (int)fi2.GetValue(__instance), contents.element, num, contents.temperature, contents.diseaseIdx, disease_count));
					
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

				foreach (PacketData packet in GasWarpData.GasPackets)
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
					GasWarpData.GasPackets.Remove(toRemove);
					toRemove = null;
				}


				__instance.UpdateAnim();
				return false;
			}

			return false;
		}
	}
	
	*/
}
