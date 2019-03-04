using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using static Door;

namespace AirLock
{

	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class AirLock_GeneratedBuildings_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			//Debug.Log(" === AirLock_GeneratedBuildings_LoadGeneratedBuildings Prefix === ");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.AIRLOCK.NAME", "AirLock");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.AIRLOCK.DESC", "This door doesn't allow gases or liquids to flow.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.AIRLOCK.EFFECT", "");

			ModUtil.AddBuildingToPlanScreen("Base", AirLockConfig.ID);

		}
	
	}

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class AirLock_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			//Debug.Log(" === AirLock_Db_Initialize loaded === ");
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["DupeTrafficControl"]);
			ls.Add(AirLockConfig.ID);
			Database.Techs.TECH_GROUPING["DupeTrafficControl"] = (string[])ls.ToArray();

			//Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
		}
	}

	[HarmonyPatch(typeof(Door), "OnPrefabInit")]
	internal class AirLock_Door_OnPrefabInit
	{

		private static void Postfix(ref Door __instance)
		{
			//Debug.Log(" === AirLock_Door_OnPrefabInit prefix === ");
			__instance.overrideAnims = new KAnimFile[1]
			{
				Assets.GetAnim("anim_use_remote_kanim")
			};
		}
	}

	[HarmonyPatch(typeof(Door), "SetForceFieldState")]
	internal class AirLock_Door_SetForceFieldState
	{

		private static FieldInfo _controlStateF = AccessTools.Field(typeof(Door), "controlState");

		private static bool Prefix(ref Door __instance, bool is_door_open, IList<int> cells)
		{
			//Debug.Log(" === AirLock_Door_SetForceFieldState prefix === ");
			if (__instance.doorType != (Door.DoorType)100)
				return true;

			bool solid = !is_door_open;
			ControlState controlState = (ControlState)_controlStateF.GetValue(__instance);
			bool force_field = is_door_open || controlState == ControlState.Auto;
			for (int i = 0; i < cells.Count; i++)
			{
				int num = cells[i];
				/*
				switch (__instance.doorType)
				{
					case DoorType.Pressure:
					case DoorType.ManualPressure:
					case DoorType.Sealed:
						Game.Instance.SetForceField(num, force_field, solid);
						break;
					case DoorType.Internal:
						Grid.Impassable[num] = (controlState != ControlState.Opened);
						Game.Instance.SetForceField(num, controlState != ControlState.Closed, solid: false);
						Pathfinding.Instance.AddDirtyNavGridCell(num);
						break;
				}
				*/
				Grid.Impassable[num] = false;
				Game.Instance.SetForceField(num, true, solid: false);
				Pathfinding.Instance.AddDirtyNavGridCell(num);
			}
			return false;
		}
	}

	[HarmonyPatch(typeof(Door), "SetSimState")]
	internal class AirLock_Door_SetSimState
	{
		private delegate void OnSimDoorClosed_Delegate();


		private static bool Prefix(ref Door __instance, bool is_door_open, IList<int> cells)
		{
			//Debug.Log(" === AirLock_Door_SetSimState prefix === ");

			if (__instance.doorType != (Door.DoorType)100)
				return true;

			MethodInfo mi = AccessTools.Method(typeof(Door), "OnSimDoorClosed");

			System.Action action = (System.Action)Delegate.CreateDelegate(typeof(System.Action), __instance, mi);


			PrimaryElement component = ((Workable)__instance).GetComponent<PrimaryElement>();
			float num = component.Mass / (float)cells.Count;
			Debug.Log(" === Door.SetSimState loaded === " + component.ElementID);

			for (int i = 0; i < cells.Count; i++)
			{
				int num2 = cells[i];
				DoorType doorType = __instance.doorType;

				//

				HandleVector<Game.CallbackInfo>.Handle handle2 = Game.Instance.callbackManager.Add(new Game.CallbackInfo(action, false));
				float temperature = component.Temperature;
				if (temperature <= 0f)
				{
					temperature = component.Temperature;
				}
				int gameCell = num2;
				SimHashes elementID = component.ElementID;
				CellElementEvent doorClose = CellEventLogger.Instance.DoorClose;
				float mass = num;
				float temperature2 = temperature;
				int index = handle2.index;
				SimMessages.ReplaceAndDisplaceElement(gameCell, elementID, doorClose, mass, temperature2, byte.MaxValue, 0, index);
				SimMessages.SetCellProperties(num2, 3);

				/*
				if (doorType == DoorType.Pressure || doorType == DoorType.Sealed || doorType == DoorType.ManualPressure)
				{
					World.Instance.groundRenderer.MarkDirty(num2);
					if (is_door_open)
					{
						MethodInfo mi = AccessTools.Method(typeof(Door), "OnSimDoorOpened", new Type[] { });
						System.Action action = (System.Action)Delegate.CreateDelegate(typeof(System.Action), mi);

						SimMessages.Dig(num2, Game.Instance.callbackManager.Add(new Game.CallbackInfo(action, false)).index);
						SimMessages.ClearCellProperties(num2, 3);
					}
					else
					{
						MethodInfo mi = AccessTools.Method(typeof(Door), "OnSimDoorClosed", new Type[] { });
						System.Action action = (System.Action)Delegate.CreateDelegate(typeof(System.Action), mi);

						HandleVector<Game.CallbackInfo>.Handle handle2 = Game.Instance.callbackManager.Add(new Game.CallbackInfo(action, false));
						float temperature = component.Temperature;
						if (temperature <= 0f)
						{
							temperature = component.Temperature;
						}
						int gameCell = num2;
						SimHashes elementID = component.ElementID;
						CellElementEvent doorClose = CellEventLogger.Instance.DoorClose;
						float mass = num;
						float temperature2 = temperature;
						int index = handle2.index;
						SimMessages.ReplaceAndDisplaceElement(gameCell, elementID, doorClose, mass, temperature2, 255, 0, index);
						SimMessages.SetCellProperties(num2, 3);
					}
				}
				*/
			}
			return false;


		}
	}
	/*
	
	[HarmonyPatch(typeof(Door), "Close")]
	internal class AirLock_Door_Close
	{
		private static bool Prefix(Door __instance)
		{
			Debug.Log(" === AirLock_Door_Close prefix === ");
			return true;
		}
	}


	[HarmonyPatch(typeof(Door), "DisplacesGas")]
	internal class AirLock_Door_DisplacesGas
	{
		private static bool Prefix(Door __instance, DoorType type)
		{
			Debug.Log(" === AirLock_Door_DisplacesGas prefix === ");
			return true;
		}
	}


	[HarmonyPatch(typeof(Door), "OnCleanUp")]
	internal class AirLock_Door_OnCleanUp
	{
		private static bool Prefix(Door __instance)
		{
			Debug.Log(" === AirLock_Door_OnCleanUp prefix === ");
			return true;
		}
	}


	[HarmonyPatch(typeof(Door), "OnSimDoorClosed")]
	internal class AirLock_Door_OnSimDoorClosed
	{
		private static bool Prefix(Door __instance)
		{
			Debug.Log(" === AirLock_Door_OnSimDoorClosed prefix === ");
			return true;
		}
	}

	[HarmonyPatch(typeof(Door), "OnSimDoorOpened")]
	internal class AirLock_Door_OnSimDoorOpened
	{
		private static bool Prefix(Door __instance)
		{
			Debug.Log(" === AirLock_Door_OnSimDoorOpened prefix === ");
			return true;
		}
	}

	// OnSpawn???


	[HarmonyPatch(typeof(Door), "Open")]
	internal class AirLock_Door_Open
	{
		private static bool Prefix(Door __instance, ref float __result)
		{
			Debug.Log(" === AirLock_Door_Open prefix === ");
			return true;
		}
	}



	[HarmonyPatch(typeof(Door), "SetWorldState")]
	internal class AirLock_Door_SetWorldState
	{
		private static bool Prefix(Door __instance)
		{
			Debug.Log(" === AirLock_Door_SetWorldState prefix === ");
			return true;
		}
	}


	[HarmonyPatch(typeof(Door), "Sim200ms")]
	internal class AirLock_Door_Sim200ms
	{
		private static bool Prefix(Door __instance, float dt)
		{
			Debug.Log(" === AirLock_Door_Sim200ms prefix === ");
			return true;
		}
	}
	*/
}
