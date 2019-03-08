using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;

namespace InstantDeconstruct
{
	//DeconstructTool.Instance.DeconstructCell(cell);
	[HarmonyPatch(typeof(DeconstructTool), "DeconstructCell")]
	internal class InstantDeconstructMod_DeconstructTool_DeconstructCell
	{
		private static bool InstantBuildModeCopy = false;
		private static MethodInfo GetFilterLayerFromGameObjectM = AccessTools.Method(typeof(DeconstructTool), "GetFilterLayerFromGameObject");

		private static bool Prefix(DeconstructTool __instance, int cell)
		{
			Debug.Log(" === InstantDeconstructMod_Game_OnPrefabInit prefix === ");

			//InstantBuildModeCopy = false;
			//bool instantDeconstruct = false;

			for (int i = 0; i < 39; i++)
			{
				GameObject gameObject = Grid.Objects[cell, i];
				if (gameObject != null)
				{
					BuildingComplete bComplete = gameObject.GetComponent<BuildingComplete>();
					bool instantDeconstruct = false;
					if (bComplete != null)
					{
						Debug.Log(bComplete.Def.PrefabID);
						instantDeconstruct = bComplete.Def.PrefabID.Equals("Ladder");
					}

					if (instantDeconstruct)
					{
						InstantBuildModeCopy = DebugHandler.InstantBuildMode;
						DebugHandler.InstantBuildMode = true;
					}

					string filterLayerFromGameObject = (string) GetFilterLayerFromGameObjectM.Invoke(__instance, new object[] { gameObject });
					//if (filterLayerFromGameObject.Equals(ToolParameterMenu.FILTERLAYERS.TILES))
					if (__instance.IsActiveLayer(filterLayerFromGameObject))
					{
						gameObject.Trigger(-790448070);
						Prioritizable component = gameObject.GetComponent<Prioritizable>();
						if (component != null)
						{
							component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
						}
					}

					if (instantDeconstruct)
					{
						DebugHandler.InstantBuildMode = InstantBuildModeCopy;
					}
				}
			}

			//InstantBuildModeCopy = DebugHandler.InstantBuildMode;
			//if (instantDeconstruct)
			//	DebugHandler.InstantBuildMode = true;
			return false;
		}
		/*
		private static void Postfix(DeconstructTool __instance, int cell)
		{
			Debug.Log(" === InstantDeconstructMod_Game_OnPrefabInit postfix === ");
			DebugHandler.InstantBuildMode = InstantBuildModeCopy;
		}
		*/
	}
}
