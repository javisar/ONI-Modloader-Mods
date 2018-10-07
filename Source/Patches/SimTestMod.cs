using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimTestMod
{
	using Harmony;
	using System;
	using System.Diagnostics;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.InteropServices;
	using UnityEngine;
	using static SimMessages;
	using Debug = Debug;

	internal static class SimMessages_Utils
	{

		public static void PrintProperties(Type myObj)
		{
			foreach (var prop in myObj.GetType().GetProperties())
			{
				Console.WriteLine(prop.Name + ": " + prop.GetValue(myObj, null));
			}

			foreach (var field in myObj.GetType().GetFields())
			{
				Console.WriteLine(field.Name + ": " + field.GetValue(myObj));
			}
		}

		public static void Log(MethodBase method, params object[] values)
		{
			ParameterInfo[] parms = method.GetParameters();
			object[] namevalues = new object[2 * parms.Length];

			string name = new StackTrace().GetFrame(2).GetMethod().Name;

			//string msg = "" + method.Name + "(";
			string msg = "" + name + "(";
			for (int i = 0, j = 0; i < parms.Length; i++, j += 2)
			{
				msg += "{" + j + "}={" + (j + 1) + "}, ";
				namevalues[j] = parms[i].Name;
				if (i < values.Length) namevalues[j + 1] = values[i];
			}
			//if (ex != null) msg += "exception=" + ex.Message;
			msg += ")";
			Debug.Log(string.Format(msg, namevalues));
		}
	}

	/*
	[HarmonyPatch(typeof(Sim), "SIM_HandleMessage")]
	internal static class Sim_SIM_HandleMessage
	{
		private static  bool Prefix(int sim_msg_id, int msg_length)
		{
			Debug.Log(" === Sim_SIM_HandleMessage Prefix === (" + sim_msg_id + "," + msg_length + ") ");			
			return true;
		}
	}
	*/

	[HarmonyPatch(typeof(Sim), "HandleMessage")]
	internal static class Sim_HandleMessage
	{
		private static bool Prefix(SimMessageHashes sim_msg_id, int msg_length, byte[] msg)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				sim_msg_id, msg_length,  msg);
			return true;
		}
	}

	[HarmonyPatch(typeof(Sim), "DLL_MessageHandler")]
	internal class Sim_DLL_MessageHandler
	{
		private unsafe static bool Prefix(int message_id, IntPtr data)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				message_id, data);

			switch (message_id)
			{
				case 1:
					{
						Sim.DLLReportMessageMessage* ptr2 = (Sim.DLLReportMessageMessage*)(void*)data;
						string msg = "SimMessage: " + Marshal.PtrToStringAnsi(ptr2->message);
						string arg = Marshal.PtrToStringAnsi(ptr2->file);
						int line = ptr2->line;
						string stack_trace = arg + ":" + line;
						Debug.Log(msg);
						Debug.Log(stack_trace);
						break;
					}
				case 0:
					{
						Sim.DLLExceptionHandlerMessage* ptr = (Sim.DLLExceptionHandlerMessage*)(void*)data;
						string text = Marshal.PtrToStringAnsi(ptr->callstack);
						string dmp_filename = Marshal.PtrToStringAnsi(ptr->dmpFilename);
						Debug.Log(text);
						Debug.Log(dmp_filename);
						break;
					}
				default:
					break;
			}
			return true;


		}
	}


	[HarmonyPatch(typeof(KCrashReporter), "ReportDLLCrash")]
	internal class KCrashReporter_ReportDLLCrash
	{
		private static bool Prefix(string msg, string stack_trace, string dmp_filename)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				msg, stack_trace, dmp_filename);
			return true;


		}
	}
	/*
    [HarmonyPatch(typeof(World), "UpdateCellInfo")]
    internal class World_UpdateCellInfo
    {
        private static bool Prefix(
                        List<SolidInfo> solidInfo, 
                        List<CallbackInfo> callbackInfo, 
                        int num_solid_substance_change_info, 
                        //Sim.SolidSubstanceChangeInfo* solid_substance_change_info, 
                        int num_liquid_change_info
                        //Sim.LiquidChangeInfo* liquid_change_info
                        )
        {
            Debug.Log(" === World_UpdateCellInfo loaded === "+ solidInfo.Count);
            return true;


        }
    }
    */

	[HarmonyPatch(typeof(Game), "UnsafeSim200ms")]
	internal class Game_UnsafeSim200ms
	{
		private static bool Prefix(float dt)
		{
			//Debug.Log(" === Game_UnsafeSim200ms Prefix === " + dt);
			return true;


		}

		private unsafe static void Postfix()
		{
			//Debug.Log(" === Game_UnsafeSim200ms Postfix === " + Grid.temperature[20000]);

		}
	}

	[HarmonyPatch(typeof(CellSelectionObject), "UpdateValues")]
	internal class CellSelectionObject_UpdateValues
	{


		private static void Postfix(CellSelectionObject __instance)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				__instance);

			FieldInfo fi = AccessTools.Field(typeof(CellSelectionObject), "selectedCell");
			if (Grid.IsValidCell((int)fi.GetValue(__instance)))
			{
				//__instance.Mass = Grid.Mass[(int)fi.GetValue(__instance)];
				__instance.ElementName = __instance.ElementName + " " + (int)fi.GetValue(__instance);
			}
		}
	}


	[HarmonyPatch(typeof(SimCellOccupier), "GetSimCellProperties")]
	internal static class SimCellOccupier_GetSimCellProperties
	{
		private static void Postfix(ref Sim.Cell.Properties __result)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				__result);
			//return true;
		}
	}
	/********************/

	[HarmonyPatch(typeof(Game), "OnPrefabInit")]
	internal class Game_OnPrefabInit
	{
		private static bool Prefix()
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod());
			return true;
		}
	}

	[HarmonyPatch(typeof(Game), "OnSpawn")]
	internal class Game_OnSpawn
	{
		private static bool Prefix()
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod());
			return true;
		}
	}

	[HarmonyPatch(typeof(Game), "Update")]
	internal class Game_Update
	{
		private static bool Prefix()
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod());
			return true;
		}
	}

	[HarmonyPatch(typeof(Game), "SimEveryTick")]
	internal class Game_SimEveryTick
	{
		private static bool Prefix(float dt)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				dt);
			return true;
		}
	}

	[HarmonyPatch(typeof(Game), "StepTheSim")]
	internal class Game_StepTheSim
	{
		private static bool Prefix(float dt)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				dt);
			return true;
		}
	}
	/********************/

	[HarmonyPatch(typeof(InterfaceTool), nameof(InterfaceTool.OnMouseMove))]
	public static class StorageLockerConfigPatch
	{
		public static void Postfix(ref InterfaceTool __instance, ref Vector3 cursor_pos)
		{
			//if (__instance.Dragging)
			//{
				//DragTool.Mode mode = (DragTool.Mode)AccessTools.Field(typeof(DragTool), "mode").GetValue(__instance);
				//Vector3 downPos = (Vector3)AccessTools.Field(typeof(DragTool), "downPos").GetValue(__instance);

				//if (mode == DragTool.Mode.Box)
				//{
					HoverTextConfiguration hoverText = __instance.GetComponent<HoverTextConfiguration>();

					int index = hoverText.ToolName.IndexOf('[');

					if (index != -1)
					{
						hoverText.ToolName = hoverText.ToolName.Remove(index - 1);
					}

					//var downPosXY = Grid.PosToXY(downPos);
					var cursorPosXY = Grid.PosToXY(cursor_pos);
					

					//int x = Mathf.Abs(downPosXY.X - cursorPosXY.X) + 1;
					//int y = Mathf.Abs(downPosXY.Y - cursorPosXY.Y) + 1;

					//string boxSizeInfo = $" [{x}x{y}, {x * y} tiles total]";
					string boxSizeInfo = $" [{Grid.PosToCell(cursor_pos)} ({cursorPosXY.x},{cursorPosXY.y})]";

					hoverText.ToolName += boxSizeInfo;
				//}
			//}
		}
	}

	[HarmonyPatch(typeof(GameUtil), "GetUnitFormattedName", new Type[] { typeof(GameObject), typeof(bool)})]
	public static class GameUtil_GetUnitFormattedName
	{
		public static void Postfix(ref string __result, GameObject go, bool upperName)
		{
			var pos = KInputManager.GetMousePos();
			int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(pos));
			string boxSizeInfo = $" [{num} ({pos.x},{pos.y})]";
			__result += " "+ boxSizeInfo;
		}
	}

	/*
	[HarmonyPatch(typeof(GameUtil), "GetUnitFormattedName", new Type[] { typeof(string), typeof(float), typeof(bool) })]
	public static class GameUtil_GetUnitFormattedName_2
	{
		public static void Postfix(ref string __result, string name, float count, bool upperName)
		{
			var pos = KInputManager.GetMousePos();
			int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(pos));
			string boxSizeInfo = $" [{num} ({pos.x},{pos.y})]";
			__result += " " + boxSizeInfo;
		}
	}
	*/

	[HarmonyPatch(typeof(SelectToolHoverTextCard), "UpdateHoverElements")]
	public static class GameUtil_GetUnitFormattedName_2
	{
		public static string PrevName = null;

		public static void Prefix(List<KSelectable> hoverObjects)
		{
			var pos = KInputManager.GetMousePos();
			int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(pos));
			string boxSizeInfo = $" [{num} ({pos.x},{pos.y})]";
			//__result += " " + boxSizeInfo;
			Element element = Grid.Element[num];
			PrevName = element.nameUpperCase;
			element.nameUpperCase += boxSizeInfo;
		}

		public static void Postfix()
		{
			var pos = KInputManager.GetMousePos();
			int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(pos));
			Element element = Grid.Element[num];
			if (PrevName != null)
			{
				element.nameUpperCase = PrevName;
			}
		}
	}


	/********************/

	[HarmonyPatch(typeof(SimMessages), "AddBuildingHeatExchange")]
	internal static class SimMessages_AddBuildingHeatExchange
	{
		private static bool Prefix(Extents extents, float mass, float temperature, float thermal_conductivity, float operating_kw, byte elem_idx, int callbackIdx)
		{
			//Debug.Log(" === SimMessages_AddBuildingHeatExchange === " + temperature);
			//Debug.Log(" === SimMessages_AddBuildingHeatExchange === " + operating_kw);
			//Debug.Log(" === SimMessages_AddBuildingHeatExchange === " + callbackIdx);
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				extents, mass, temperature, thermal_conductivity, operating_kw, elem_idx, callbackIdx);
			return true;
		}
	}
	
	[HarmonyPatch(typeof(SimMessages), "AddDiseaseEmitter")]
	internal static class SimMessages_AddDiseaseEmitter
	{
		private static bool Prefix(int callbackIdx)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				callbackIdx);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "AddElementChunk")]
	internal static class SimMessages_AddElementChunk
	{
		private static bool Prefix(int gameCell, SimHashes element, float mass, float temperature, float surface_area, float thickness, int cb_handle)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				gameCell, element, mass, temperature, surface_area, thickness, cb_handle);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "AddElementConsumer")]
	internal static class SimMessages_AddElementConsumer
	{
		private static bool Prefix(int gameCell, ElementConsumer.Configuration configuration, SimHashes element, byte radius, int cb_handle)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				gameCell, configuration, element, radius, cb_handle);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "AddElementEmitter")]
	internal static class SimMessages_AddElementEmitter
	{
		private static bool Prefix(float max_pressure, int on_registered, int on_blocked, int on_unblocked)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 max_pressure, on_registered, on_blocked, on_unblocked );
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "AddRemoveSubstance", new Type[] { typeof(int),typeof(SimHashes),typeof(CellAddRemoveSubstanceEvent), typeof(float), typeof(float), typeof(byte),typeof(int),typeof(bool),typeof(int)})]
	internal static class SimMessages_AddRemoveSubstance_1
	{
		private static bool Prefix(int gameCell, SimHashes new_element, CellAddRemoveSubstanceEvent ev, float mass, float temperature, byte disease_idx, int disease_count, bool do_vertical_solid_displacement , int callbackIdx)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 gameCell, new_element, ev, mass, temperature, disease_idx, disease_count, do_vertical_solid_displacement, callbackIdx);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "AddRemoveSubstance", new Type[] { typeof(int), typeof(int), typeof(CellAddRemoveSubstanceEvent), typeof(float), typeof(float), typeof(byte), typeof(int), typeof(bool), typeof(int) })]
	internal static class SimMessages_AddRemoveSubstance_2
	{
		private static bool Prefix(int gameCell, int elementIdx, CellAddRemoveSubstanceEvent ev, float mass, float temperature, byte disease_idx, int disease_count, bool do_vertical_solid_displacement , int callbackIdx )
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 gameCell, elementIdx, ev, mass, temperature, disease_idx, disease_count, do_vertical_solid_displacement, callbackIdx);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ClearCellProperties")]
	internal static class SimMessages_ClearCellProperties
	{
		private static bool Prefix(int gameCell, byte properties)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 gameCell, properties);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ConsumeDisease")]
	internal static class SimMessages_ConsumeDisease
	{
		private static bool Prefix(int game_cell, float percent_to_consume, int max_to_consume, int callback_idx)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 game_cell, percent_to_consume, max_to_consume, callback_idx);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ConsumeMass")]
	internal static class SimMessages_ConsumeMass
	{
		private static bool Prefix(int gameCell, SimHashes element, float mass, byte radius, int callbackIdx )
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 gameCell, element, mass, radius, callbackIdx );
			if (Grid.IsValidCell(gameCell))
			{
				Vector2I v = Grid.CellToXY(gameCell);
				Debug.Log("(" + v.x + "," + v.y + ")");
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "CreateDiseaseTable")]
	internal static class SimMessages_CreateDiseaseTable
	{
		private static bool Prefix()
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod());
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "CreateElementInteractions")]
	internal static class SimMessages_CreateElementInteractions
	{
		private static bool Prefix(ElementInteraction[] interactions)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				interactions);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "CreateSimElementsTable")]
	internal static class SimMessages_CreateSimElementsTable
	{
		private static bool Prefix(List<Element> elements)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				elements);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "CreateWorldGenHACKDiseaseTable")]
	internal static class SimMessages_CreateWorldGenHACKDiseaseTable
	{
		private static bool Prefix(List<string> diseaseIds)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				diseaseIds);
			return true;
		}
	}


	[HarmonyPatch(typeof(SimMessages), "Dig")]
	internal static class SimMessages_Dig
	{
		private static bool Prefix(int gameCell, int callbackIdx)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 gameCell, callbackIdx);
			return true;
		}
	}


	[HarmonyPatch(typeof(SimMessages), "EmitMass")]
	internal static class SimMessages_EmitMass
	{
		private static bool Prefix(int gameCell, byte element_idx, float mass, float temperature, byte disease_idx, int disease_count, int callbackIdx)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 gameCell, element_idx, mass, temperature, disease_idx, disease_count, callbackIdx);
			return true;
		}
	}

	
	//[HarmonyPatch(typeof(SimMessages), "GetElementIndex")]
	//internal static class SimMessages_GetElementIndex
	//{
	//	private static bool Prefix(SimHashes element)
	//	{
	//		SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
	//			 element);
	//		return true;
	//	}
	//}
	

	[HarmonyPatch(typeof(SimMessages), "ModifyBuildingEnergy")]
	internal static class SimMessages_ModifyBuildingEnergy
	{
		private static bool Prefix(int sim_handle, float delta_kj, float min_temperature, float max_temperature)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 sim_handle, delta_kj, min_temperature, max_temperature);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ModifyBuildingHeatExchange")]
	internal static class SimMessages_ModifyBuildingHeatExchange
	{
		private static bool Prefix(int sim_handle, Extents extents, float mass, float temperature, float thermal_conductivity, float overheat_temperature, float operating_kw, byte element_idx)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 sim_handle, extents, mass, temperature, thermal_conductivity, overheat_temperature, operating_kw, element_idx);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ModifyCell")]
	internal static class SimMessages_ModifyCell
	{
		private static bool Prefix(int gameCell, int elementIdx, float temperature, float mass, byte disease_idx, int disease_count, ReplaceType replace_type , bool do_vertical_solid_displacement, int callbackIdx)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				  gameCell,  elementIdx,  temperature,  mass,  disease_idx, disease_count,  replace_type, do_vertical_solid_displacement , callbackIdx);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ModifyCellWorldZone")]
	internal static class SimMessages_ModifyCellWorldZone
	{
		private static bool Prefix(int cell, byte zone_id)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				  cell, zone_id);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ModifyDiseaseEmitter")]
	internal static class SimMessages_ModifyDiseaseEmitter
	{
		private static bool Prefix(int sim_handle, int cell, byte range, byte disease_idx, float emit_interval, int emit_count)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				  sim_handle, cell, range, disease_idx, emit_interval, emit_count);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ModifyDiseaseOnCell")]
	internal static class SimMessages_ModifyDiseaseOnCell
	{
		private static bool Prefix(int gameCell, byte disease_idx, int disease_count)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				  gameCell, disease_idx, disease_count);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ModifyElementChunkEnergy")]
	internal static class SimMessages_ModifyElementChunkEnergy
	{
		private static bool Prefix(int sim_handle, float delta_kj)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				  sim_handle, delta_kj);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ModifyElementChunkTemperatureAdjuster")]
	internal static class SimMessages_ModifyElementChunkTemperatureAdjuster
	{
		private static bool Prefix(int sim_handle, float temperature, float heat_capacity, float thermal_conductivity)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				  sim_handle, temperature, heat_capacity, thermal_conductivity);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ModifyElementEmitter")]
	internal static class SimMessages_ModifyElementEmitter
	{
		private static bool Prefix(int sim_handle, int game_cell, int max_depth, SimHashes element, float emit_interval, float emit_mass, float emit_temperature, float max_pressure, byte disease_idx, int disease_count)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				  sim_handle, game_cell, max_depth, element, emit_interval, emit_mass, emit_temperature, max_pressure, disease_idx, disease_count);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ModifyEnergy")]
	internal static class SimMessages_ModifyEnergy
	{
		private static bool Prefix(int gameCell, float kilojoules, float max_temperature, EnergySourceID id)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 gameCell, kilojoules, max_temperature, id);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ModifyMass")]
	internal static class SimMessages_ModifyMass
	{
		private static bool Prefix(int gameCell, float mass, byte disease_idx, int disease_count, CellModifyMassEvent ev, float temperature, SimHashes element)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 gameCell, mass, disease_idx, disease_count, ev, temperature , element);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "MoveElementChunk")]
	internal static class SimMessages_MoveElementChunk
	{
		private static bool Prefix(int sim_handle, int cell)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 sim_handle, cell);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "NewGameFrame")]
	internal static class SimMessages_NewGameFrame
	{
		private static bool Prefix(float elapsed_seconds, Vector2I min, Vector2I max)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 elapsed_seconds, min, max);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "RemoveBuildingHeatExchange")]
	internal static class SimMessages_RemoveBuildingHeatExchange
	{
		private static bool Prefix(int sim_handle, int callbackIdx)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 sim_handle, callbackIdx);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "RemoveDiseaseEmitter")]
	internal static class SimMessages_RemoveDiseaseEmitter
	{
		private static bool Prefix(int cb_handle, int sim_handle)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 cb_handle, sim_handle);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "RemoveElementChunk")]
	internal static class SimMessages_RemoveElementChunk
	{
		private static bool Prefix (int sim_handle, int cb_handle)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 sim_handle, cb_handle);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "RemoveElementConsumer")]
	internal static class SimMessages_RemoveElementConsumer
	{
		private static bool Prefix(int cb_handle, int sim_handle)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 cb_handle, sim_handle);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "RemoveElementEmitter")]
	internal static class SimMessages_RemoveElementEmitter
	{
		private static bool Prefix(int cb_handle, int sim_handle)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 cb_handle, sim_handle);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ReplaceAndDisplaceElement")]
	internal static class SimMessages_ReplaceAndDisplaceElement
	{
		private static bool Prefix(int gameCell, SimHashes new_element, CellElementEvent ev, float mass, float temperature, byte disease_idx, int disease_count, int callbackIdx)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 gameCell, new_element, ev, mass, temperature, disease_idx, disease_count, callbackIdx);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "ReplaceElement")]
	internal static class SimMessages_ReplaceElement
	{
		private static bool Prefix(int gameCell, SimHashes new_element, CellElementEvent ev, float mass, float temperature, byte diseaseIdx , int diseaseCount, int callbackIdx)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 gameCell, new_element, ev, mass, temperature, diseaseIdx, diseaseCount, callbackIdx);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "SetCellProperties")]
	internal static class SimMessages_SetCellProperties
	{
		private static bool Prefix(int gameCell, byte properties)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				 gameCell, properties);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "SetDebugProperties")]
	internal static class SimMessages_SetDebugProperties
	{
		private static bool Prefix(Sim.DebugProperties properties)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				  properties);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "SetElementChunkData")]
	internal static class SimMessages_SetElementChunkData
	{
		private static bool Prefix(int sim_handle, float temperature, float heat_capacity)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				  sim_handle, temperature, heat_capacity);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "SetElementConsumerData")]
	internal static class SimMessages_SetElementConsumerData
	{
		private static bool Prefix(int sim_handle, int cell, float consumptionRate)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				  sim_handle, cell, consumptionRate);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "SetInsulation")]
	internal static class SimMessages_SetInsulation
	{
		private static bool Prefix(int gameCell, float value)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				  gameCell, value);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "SetStrength")]
	internal static class SimMessages_SetStrength
	{
		private static bool Prefix(int gameCell, int weight, float strengthMultiplier)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				  gameCell, weight, strengthMultiplier);
			return true;
		}
	}

	[HarmonyPatch(typeof(SimMessages), "SimDataInitializeFromCells")]
	internal static class SimMessages_SimDataInitializeFromCells
	{
		private static bool Prefix(int width, int height, Sim.Cell[] cells, float[] bgTemp, Sim.DiseaseCell[] dc)
		{
			SimMessages_Utils.Log(MethodBase.GetCurrentMethod(),
				   width,  height, cells,bgTemp, dc);
			return true;
		}
	}

}
