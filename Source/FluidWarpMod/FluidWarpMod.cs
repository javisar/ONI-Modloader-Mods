using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Harmony;
using UnityEngine;
using WarpMod;

namespace LiquidWarpMod
{
    static class Logger
    {
        sealed class FileLogHandler : ILogHandler
        {
            private FileStream fileStream;
            private StreamWriter streamWriter;

            public FileLogHandler(string LogFileName)
            {
                fileStream = new FileStream(LogFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                streamWriter = new StreamWriter(fileStream);
            }

            public void LogException(Exception exception, UnityEngine.Object context)
            {
                streamWriter.WriteLine("Exception: {0}", exception.Message);
                streamWriter.WriteLine("Stacktrace: {0}", exception.StackTrace);
                streamWriter.Flush();
            }

            public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
            {
                streamWriter.WriteLine(format, args);
            }
        }

        static Logger()
        {
#if DEBUG
            UnityEngine.Debug.unityLogger.logHandler = new FileLogHandler("Mods" + System.IO.Path.DirectorySeparatorChar + "Logs" + System.IO.Path.DirectorySeparatorChar + "FluidWarpMod.txt");
            UnityEngine.Debug.unityLogger.logEnabled = true;
#endif
        }
    }
    
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

        private static bool Prefix(ValveBase __instance, float dt, int ___inputCell, int ___outputCell)
		{
            if (__instance.conduitType != (ConduitType)100 && __instance.conduitType != (ConduitType)101)
            {
                return true;
            }
            int channelNo = (int)(__instance.CurrentFlow * 1000.0f);
            Debug.LogFormat(" === ValveBase.ConduitUpdate({0}) Prefix conduitType={1}, inputCell={2}, outputCell={3}, channelNo={4}", dt, __instance.conduitType, ___inputCell, ___outputCell, channelNo);

            if (channelNo == 0)
            {
                // Cannel number is set to 0, WarpGate disabled
                return false;
            }

            ConduitFlow flowManager = null;
            if (__instance.conduitType == (ConduitType)100)
            {
                flowManager = Conduit.GetFlowManager(ConduitType.Liquid);
            }
            else if (__instance.conduitType == (ConduitType)101)
            {
                flowManager = Conduit.GetFlowManager(ConduitType.Gas);
            }

			if (!flowManager.HasConduit(___inputCell) || !flowManager.HasConduit(___outputCell))
			{
                __instance.UpdateAnim();
			}

            List<PacketData> availablePackets = WarpPackageManager.getAvailablePackages(__instance.conduitType);
            if (flowManager.HasConduit(___inputCell))
			{
                ConduitFlow.Conduit inputConduit = flowManager.GetConduit(___inputCell);
                ConduitFlow.ConduitContents contents = inputConduit.GetContents(flowManager);
				float num = Mathf.Min(contents.mass, 10f * dt);
				if (num > 0f)
				{

					float num2 = num / contents.mass;
					int disease_count = (int)(num2 * (float)contents.diseaseCount);

                    if (WarpPackageManager.getTotalStoredMass(__instance.conduitType, channelNo) < WarpPackageManager.getMaxContent(__instance.conduitType))
                    {
                        availablePackets.Add(new PacketData((int)__instance.conduitType, channelNo, ___outputCell, contents.element, num, contents.temperature, contents.diseaseIdx, disease_count));
                        flowManager.RemoveElement(___inputCell, num);
                        Debug.LogFormat("Adding Element to WarpSpace, mass={0}", num);
                    }
				}
				__instance.UpdateAnim();
				return false;
			}


			if (flowManager.HasConduit(___outputCell))
			{
				ConduitFlow.Conduit conduitO = flowManager.GetConduit(___outputCell);
				
                if (!flowManager.IsConduitFull(___outputCell))
                {
                    foreach (PacketData packet in availablePackets)
                    {
                        if (channelNo == packet.channel_no
                            && (int)__instance.conduitType == packet.content_type)
                        {
                            float num3 = flowManager.AddElement(___outputCell, packet.element, packet.mass, packet.temperature, packet.disease_idx, packet.disease_count);
                            Debug.LogFormat("Adding Element to pipe: packet mass={0}, actually added mass={1}", packet.mass, num3);
                            Game.Instance.accumulators.Accumulate(__instance.AccumulatorHandle, num3);
                            packet.mass -= num3;
                            break;
                        }
                    }
                }

                int removed = availablePackets.RemoveAll(_ => _.mass <= 0);
                Debug.LogFormat("Packets removed: {0}", removed);
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
            Debug.LogFormat(" === FluidWarpMod_ValveSideScreen_OnSpawn Postfix === ");

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
            Debug.LogFormat(" === FluidWarpMod_SideScreenContent_GetTitle Postfix === ");

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
            Debug.LogFormat(" === FluidWarpMod_ValveSideScreen_SetTarget Postfix === ");

			FieldInfo fi3 = AccessTools.Field(typeof(ValveSideScreen), "minFlowLabel");
			FieldInfo fi4 = AccessTools.Field(typeof(ValveSideScreen), "maxFlowLabel");

			ConduitType type = FluidWarpMod_Utils.GetConduitType(__instance);
			if (type == (ConduitType)100 || type == (ConduitType)101)
			{
				((LocText)fi3.GetValue(__instance)).text = "Channel";
				((LocText)fi4.GetValue(__instance)).text = "Channel";
			}

		}

	}

}
