using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace NoOverloadedWiresMod
{
	
	[HarmonyPatch(typeof(CircuitManager), "CheckCircuitOverloaded")]
	internal class NoOverloadedWiresMod_CircuitManager_CheckCircuitOverloaded
	{

		private static bool Prefix(CircuitManager __instance, float dt, int id, float watts_used)
		{
			//Debug.Log(" === NoOverloadedWiresMod_CircuitManager_CheckCircuitOverloaded Prefix === ");

			return false;
		}
	}

	/*
	[HarmonyPatch(typeof(ElectricalUtilityNetwork), "UpdateOverloadTime")]
	internal class NoOverloadedWiresMod_ElectricalUtilityNetwork_UpdateOverloadTime
	{

		private static bool Prefix(ElectricalUtilityNetwork __instance, float dt, ref float watts_used, List<WireUtilityNetworkLink>[] bridgeGroups)
		{
			Debug.Log(" === NoOverloadedWiresMod_ElectricalUtilityNetwork_UpdateOverloadTime Prefix === ");
			watts_used = 0;
			return true;

		}
	}
	*/

}
