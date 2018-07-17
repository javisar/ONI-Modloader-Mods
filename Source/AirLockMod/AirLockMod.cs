using Harmony;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Door;

namespace AirLockMod
{

	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class AirLockMod_GeneratedBuildings_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Debug.Log(" === GeneratedBuildings Prefix === " + AirLockConfig.ID);
			Strings.Add("STRINGS.BUILDINGS.PREFABS.AIRLOCK.NAME", "AirLock");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.AIRLOCK.DESC", "This door doesn't allow gases or liquids to flow.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.AIRLOCK.EFFECT", "");

			List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[11].data);
			ls.Add(AirLockConfig.ID);
			TUNING.BUILDINGS.PLANORDER[11].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(AirLockConfig.ID);


		}
		private static void Postfix()
		{

			Debug.Log(" === GeneratedBuildings Postfix === " + AirLockConfig.ID);
			object obj = Activator.CreateInstance(typeof(AirLockConfig));
			BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
		}
	}

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class AirLockMod_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			Debug.Log(" === Database.Techs loaded === " + AirLockConfig.ID);
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["PressureManagement"]);
			ls.Add(AirLockConfig.ID);
			Database.Techs.TECH_GROUPING["PressureManagement"] = (string[])ls.ToArray();			
		}
	}

	
	[HarmonyPatch(typeof(Door), "SetSimState")]
	internal class MyTileTechMod
	{
		private static bool Prefix(Door __instance, bool is_door_open, IList<int> cells)
		{
			Debug.Log(" === Door.SetSimState loaded === ");
			if (__instance.building.Def.PrefabID != "AirLock") return true;

			PrimaryElement component = ((Workable)__instance).GetComponent<PrimaryElement>();
			float num = component.Mass / (float)cells.Count;
			Debug.Log(" === Door.SetSimState loaded === " + component.ElementID);

			for (int i = 0; i < cells.Count; i++)
			{
				int num2 = cells[i];
				DoorType doorType = __instance.doorType;
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
			}
			return false;


		}
	}
	
		/*
		[HarmonyPatch(typeof(AccessControlNavMask), "CanTraverse")]
		internal class MyTileTechMod3
		{
			private static bool Prefix(AccessControlNavMask __instance, ref bool __result, int cell, int from_cell)
			{
				if (Grid.HasAccessDoor[cell])
				{
					goto IL_000c;
				}
				goto IL_000c;
				IL_000c:
				if (!Grid.HasAccessDoor[cell])
				{
					__result = true;
					return false;
				}
				GameObject gameObject = Grid.Objects[cell, 1];
				if ((UnityEngine.Object)gameObject == (UnityEngine.Object)null)
				{
					__result = true;
					return false;
				}
				AccessControl component = gameObject.GetComponent<AccessControl>();
				if ((UnityEngine.Object)component == (UnityEngine.Object)null)
				{
					__result = true;
					return false;
				}
				FieldInfo fi = AccessTools.Field(typeof(AccessControlNavMask), "agent");
				AccessControl.Permission permission = component.GetPermission(((Navigator)fi.GetValue(__instance)).gameObject);
				switch (permission)
				{
					case AccessControl.Permission.Neither:
						__result = false;
						return false;
					case AccessControl.Permission.Both:
						__result = true;
						return false;
					default:
						{
							Vector3 vector = Grid.CellToPosCCC(cell, Grid.SceneLayer.NoLayer) - Grid.CellToPosCCC(from_cell, Grid.SceneLayer.NoLayer);
							var component2 = ((Component)component).GetComponent<Door>();
							var component3 = ((Component)component).GetComponent<MyDoor>();

							if ((component2 != null &&((Component)component2).GetComponent<Rotatable>().IsRotated)
								|| (component3 != null && ((Component)component3).GetComponent<Rotatable>().IsRotated))
							{
								if (permission == AccessControl.Permission.GoLeft && vector.y > 0f)
								{
									goto IL_00cc;
								}
								if (permission == AccessControl.Permission.GoRight && vector.y < 0f)
								{
									goto IL_00cc;
								}
							}
							else
							{
								if (permission == AccessControl.Permission.GoLeft && vector.x < 0f)
								{
									goto IL_0103;
								}
								if (permission == AccessControl.Permission.GoRight && vector.x > 0f)
								{
									goto IL_0103;
								}
							}
							__result = false;
							return false;
						}
						IL_00cc:
						__result = true;
						return false;
						IL_0103:
						__result = true;
						return false;
				}
				return false;
			}
		}

		[HarmonyPatch(typeof(Database.BuildingStatusItems), "CreateStatusItems")]
		internal class MyTileTechMod2
		{
			private static void Postfix(Database.BuildingStatusItems __instance)
			{
				Debug.Log(" === BuildingStatusItems.CreateStatusItems postix === " + MyTileConfig.ID);
				__instance.ChangeDoorControlState.resolveStringCallback = delegate (string str, object data)
				{

					if (data is MyDoor)
					{
						MyDoor door2 = (MyDoor)data;
						return str.Replace("{ControlState}", door2.RequestedState.ToString());
					}
					else
					{
						Door door2 = (Door)data;
						return str.Replace("{ControlState}", door2.RequestedState.ToString());
					}
				};

				__instance.CurrentDoorControlState.resolveStringCallback = delegate (string str, object data)
				{
					if (data is MyDoor)
					{
						MyDoor door = (MyDoor)data;
						string newValue6 = Strings.Get("STRINGS.BUILDING.STATUSITEMS.CURRENTDOORCONTROLSTATE." + door.CurrentState.ToString().ToUpper());
						return str.Replace("{ControlState}", newValue6);
					}
					else
					{
						Door door = (Door)data;
						string newValue6 = Strings.Get("STRINGS.BUILDING.STATUSITEMS.CURRENTDOORCONTROLSTATE." + door.CurrentState.ToString().ToUpper());
						return str.Replace("{ControlState}", newValue6);
					}
				};
			}
		}
		*/
		/*
		[HarmonyPatch(typeof(Door), "SetWorldState")]
		internal class MyTileTechMod
		{
			private static bool Prefix(Door __instance)
			{
				Debug.Log(" === Door.SetForceFieldState loaded === ");
				int[] placementCells = __instance.building.PlacementCells;
				bool is_door_open = __instance.IsOpen();
				MethodInfo mi = AccessTools.Method(typeof(Door), "SetForceFieldState");
				mi.Invoke(__instance, new object[] { true, placementCells });
				//__instance.SetForceFieldState(is_door_open, placementCells);
				MethodInfo mi2 = AccessTools.Method(typeof(Door), "SetSimState");
				mi2.Invoke(__instance, new object[] { true, placementCells });
				//__instance.SetSimState(is_door_open, placementCells);
				return false;
			}
		}
		*/
		/*
		[HarmonyPatch(typeof(Door), "SetForceFieldState")]
		internal class MyTileTechMod
		{
			private static void Prefix(Door __instance, bool is_door_open, IList<int> cells)
			{
				Debug.Log(" === Door.SetForceFieldState loaded === ");
				is_door_open = true;
				FieldInfo fi = AccessTools.Field(typeof(Door), "controlState");
				bool solid = !is_door_open;
				bool force_field = is_door_open || (ControlState)fi.GetValue(__instance) == ControlState.Auto;
				for (int i = 0; i < cells.Count; i++)
				{
					int num = cells[i];
					switch (__instance.doorType)
					{
						case DoorType.Pressure:
						case DoorType.ManualPressure:
						case DoorType.Sealed:
							Game.Instance.SetForceField(num, force_field, solid);
							break;
						case DoorType.Internal:
							Grid.Impassable[num] = false;
							Game.Instance.SetForceField(num, (ControlState)fi.GetValue(__instance) != ControlState.Closed, true);
							Pathfinding.Instance.AddDirtyNavGridCell(num);
							break;
					}
				}
			}
		}
		*/
		/*
		[HarmonyPatch(typeof(Door), "SetSimState")]
		internal class MyTileTechMod
		{
			private static bool Prefix(Door __instance, bool is_door_open, IList<int> cells)
			{
				Debug.Log(" === Door.SetSimState loaded === ");
				PrimaryElement component = ((Workable)__instance).GetComponent<PrimaryElement>();
				float num = component.Mass / (float)cells.Count;
				Debug.Log(" === Door.SetSimState loaded === " + component.ElementID);

				for (int i = 0; i < cells.Count; i++)
				{
					int num2 = cells[i];
					DoorType doorType = __instance.doorType;
					if (doorType == DoorType.Pressure || doorType == DoorType.Sealed || doorType == DoorType.ManualPressure)
					{
						World.Instance.groundRenderer.MarkDirty(num2);
						if (is_door_open)
						{
							MethodInfo mi = AccessTools.Method(typeof(Door), "OnSimDoorOpened", new Type[] { });

							if (Grid.Element[num2].IsSolid)
							{
								System.Action action = (System.Action)Delegate.CreateDelegate(typeof(System.Action), mi);
								HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(action, false));
								int gameCell = num2;
								SimHashes new_element = SimHashes.Vacuum;
								CellElementEvent doorOpen = CellEventLogger.Instance.DoorOpen;
								float mass = 0f;
								float temperature = -1f;
								int index = handle.index;
								Debug.Log(" === Door.SetSimState loaded === 1 "+new_element);
								SimMessages.ReplaceElement(gameCell, new_element, doorOpen, mass, temperature, 255, 0, index);
								SimMessages.ClearCellProperties(num2, 4);
							}
							else
							{
								//__instance.OnSimDoorOpened();
								mi.Invoke(__instance, null);
							}
						}
						else if (Grid.Element[num2].IsSolid)
						{
							//this.OnSimDoorClosed();
							MethodInfo mi = AccessTools.Method(typeof(Door), "OnSimDoorClosed", new Type[] { });
							mi.Invoke(__instance, null);
						}
						else
						{
							MethodInfo mi = AccessTools.Method(typeof(Door), "OnSimDoorClosed", new Type[] { });
							System.Action action = (System.Action)Delegate.CreateDelegate(typeof(System.Action), mi);

							HandleVector<Game.CallbackInfo>.Handle handle2 = Game.Instance.callbackManager.Add(new Game.CallbackInfo(action, false));
							int index = num2;
							SimHashes new_element = component.ElementID;
							CellElementEvent doorOpen = CellEventLogger.Instance.DoorClose;
							float temperature = num;
							float mass = component.Temperature;
							int gameCell = handle2.index;
							Debug.Log(" === Door.SetSimState loaded === 2 " + new_element);
							SimMessages.ReplaceAndDisplaceElement(index, new_element, doorOpen, temperature, mass, 255, 0, gameCell);
							SimMessages.SetCellProperties(num2, 4);
						}
					}
				}
				return false;


			}



			[HarmonyPatch(typeof(Door), "OnCleanUp")]
			internal class MyTileTechMod2
			{
				private static bool Prefix(Door __instance)
				{
					Debug.Log(" === Door.OnCleanUp loaded === ");
					//__instance.UpdateDoorState(true);
					MethodInfo mi = AccessTools.Method(typeof(Door), "UpdateDoorState", new Type[] { typeof(bool) });
					mi.Invoke(__instance, new object[] { true});
					List<int> list = new List<int>();
					int[] placementCells = __instance.building.PlacementCells;
					foreach (int num in placementCells)
					{
						SimMessages.ClearCellProperties(num, 12);
						Grid.RenderedByWorld[num] = Grid.Element[num].substance.renderedByWorld;
						Grid.FakeFloor[num] = false;
						if (Grid.Element[num].IsSolid)
						{
							SimMessages.ReplaceAndDisplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.DoorOpen, 0f, -1f, 255, 0, -1);
						}
						Pathfinding.Instance.AddDirtyNavGridCell(num);
						//if (__instance.rotatable.IsRotated)
						FieldInfo fi = AccessTools.Field(typeof(Door), "rotatable");
						if (((Rotatable)fi.GetValue(__instance)).IsRotated)
						{
							list.Add(Grid.CellAbove(num));
							list.Add(Grid.CellBelow(num));
						}
						else
						{
							list.Add(Grid.CellLeft(num));
							list.Add(Grid.CellRight(num));
						}
					}
					int[] placementCells2 = __instance.building.PlacementCells;
					foreach (int num2 in placementCells2)
					{
						Grid.HasDoor[num2] = false;
						Grid.HasAccessDoor[num2] = false;
						Game.Instance.SetForceField(num2, false, Grid.Solid[num2]);
						Grid.Impassable[num2] = false;
						Pathfinding.Instance.AddDirtyNavGridCell(num2);
					}
					Game.Instance.roomProber.RemoveDoor(__instance);
					//((Workable)__instance).OnCleanUp();
					MethodInfo mi2 = AccessTools.Method(typeof(Workable), "UpdateDoorState", new Type[] { typeof(bool) });
					mi2.Invoke(__instance, null);
					return false;
				}
			}
		}
		*/
	}
