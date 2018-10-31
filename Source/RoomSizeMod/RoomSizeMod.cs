using Harmony;
using ONI_Common.Json;
using STRINGS;
using System;
using System.Collections.Generic;
using static RoomConstraints;

namespace RoomSizeMod
{
    
    [HarmonyPatch(typeof(Game),"OnPrefabInit")]
    internal class RoomSizeMod_Game_OnPrefabInit
    {

        private static void Postfix(Game __instance)
        {
            Debug.Log(" === RoomSizeMod_Game_OnPrefabInit Postfix === ");

        }
    }


    [HarmonyPatch(typeof(RoomProber),MethodType.Constructor)]
    internal class RoomSizeMod_RoomProber
    {

        private static void Postfix(RoomProber __instance)
        {
            //Debug.Log(" === RoomSizeMod_RoomProber Postfix === ");
            //RoomProber.MaxRoomSize = 1024;
            //RoomProber.MaxRoomSize = RoomSizeState.StateManager.State.OverallMaximumRoomSize;
            TuningData<RoomProber.Tuning>.Get().maxRoomSize = RoomSizeState.StateManager.State.OverallMaximumRoomSize;
        }
    }

	[HarmonyPatch(typeof(Database.RoomTypes), MethodType.Constructor)]
	[HarmonyPatch(new Type[] { typeof(ResourceSet)})]
	internal class RoomSizeMod_RoomTypes
	{
        /*
		public static Constraint MAXIMUM_SIZE_128 = new Constraint(null, (Room room) => room.cavity.numCells <= 128, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "128"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "128"), null);
		public static Constraint MAXIMUM_SIZE_160 = new Constraint(null, (Room room) => room.cavity.numCells <= 160, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "160"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "160"), null);
		public static Constraint MAXIMUM_SIZE_192 = new Constraint(null, (Room room) => room.cavity.numCells <= 192, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "192"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "192"), null);
		public static Constraint MAXIMUM_SIZE_224 = new Constraint(null, (Room room) => room.cavity.numCells <= 224, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "224"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "224"), null);
		public static Constraint MAXIMUM_SIZE_240 = new Constraint(null, (Room room) => room.cavity.numCells <= 240, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "240"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "240"), null);
        */
		private static void ChangeRoomMaximumSize(RoomType roomType, Constraint constraint)
		{
            if (roomType == null)
            {
                Debug.Log(" === RoomSizeMod_ChangeRoomMaximumSize === Cannot find roomtype ");
                return;
            }
            Debug.Log(" === RoomSizeMod_ChangeRoomMaximumSize("+ roomType.Id + ") === ");
            RoomConstraints.Constraint[] additional_constraints = roomType.additional_constraints;
			for (int i = 0; i < additional_constraints.Length; i++)
			{
				if (additional_constraints[i].name.Contains("Maximum size:"))
				{
					additional_constraints[i] = constraint;
				}
			}
		}

		private static void Postfix(ref Database.RoomTypes __instance)
		{
			Debug.Log(" === RoomSizeMod_RoomTypes Postfix === ");
            /*
			ChangeRoomMaximumSize(__instance.PlumbedBathroom,	MAXIMUM_SIZE_128);
			ChangeRoomMaximumSize(__instance.Latrine,			MAXIMUM_SIZE_128);
			ChangeRoomMaximumSize(__instance.Bedroom,			MAXIMUM_SIZE_128);
			ChangeRoomMaximumSize(__instance.Barracks,			MAXIMUM_SIZE_128);
			ChangeRoomMaximumSize(__instance.GreatHall,			MAXIMUM_SIZE_240);
			ChangeRoomMaximumSize(__instance.MessHall,			MAXIMUM_SIZE_128);
			ChangeRoomMaximumSize(__instance.Hospital,			MAXIMUM_SIZE_192);
			ChangeRoomMaximumSize(__instance.MassageClinic,		MAXIMUM_SIZE_128);
			ChangeRoomMaximumSize(__instance.PowerPlant,		MAXIMUM_SIZE_192);
			ChangeRoomMaximumSize(__instance.Farm,				MAXIMUM_SIZE_192);
			ChangeRoomMaximumSize(__instance.CreaturePen,		MAXIMUM_SIZE_192);
			ChangeRoomMaximumSize(__instance.MachineShop,		MAXIMUM_SIZE_192);
			ChangeRoomMaximumSize(__instance.RecRoom,			MAXIMUM_SIZE_128);
            */
            //foreach (KeyValuePair<string, int> entry in RoomSizeStateManager.ConfiguratorState.MaximumRoomSizes)
            foreach (KeyValuePair<string, int> entry in RoomSizeState.StateManager.State.MaximumRoomSizes)
            {
                Constraint max_size = new Constraint(null, (Room room) => room.cavity.numCells <= entry.Value, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, ""+ entry.Value), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, ""+ entry.Value), null);
                //Debug.Log(entry.Key);
                //Debug.Log(__instance.Get(entry.Key));
                ChangeRoomMaximumSize(__instance.Get(entry.Key), max_size); 
            }
            
           

        }
	}


    

    /*
	public static class RoomSizeModData
	{
		//public static List<PacketData> LiquidPackets = new List<PacketData>();
		public static Dictionary<int, Dictionary<SimHashes, RoomElementData>> RoomData = new Dictionary<int, Dictionary<SimHashes,RoomElementData>>();
	}

	public class RoomElementData
	{
		public SimHashes element = 0;
		public float mass = 0;
		public float temperature = 0;
        public int numCells = 0;
		
	}
	

	[HarmonyPatch(typeof(RoomDetails), "RoomDetailString")]
	internal class RoomSizeMod_RoomDetails_RoomDetailString
	{

		private static void Postfix(Room room, ref string __result)
		{
			CavityInfo ci = room.cavity;
			Dictionary<SimHashes, RoomElementData> data = null;
			bool exists = RoomSizeModData.RoomData.TryGetValue(ci.handle.index, out data);
			if (!exists) return;

			foreach (KeyValuePair<SimHashes, RoomElementData> entry in data)
			{
				__result += "\n    • " + entry.Key + ": " + Math.Round(entry.Value.mass) +" Kg at "+(entry.Value.temperature/ entry.Value.numCells - 273.15f).ToString("N1") + " ºC";
			}
			
		}
	}

	[HarmonyPatch(typeof(RoomProber), "RebuildDirtyCavities")]
	internal class RoomSizeMod_RoomProber_RebuildDirtyCavities
	{

		public unsafe static float GetMass(int cell)
		{
			if (Grid.IsValidCell(cell))
			{
				return Grid.mass[cell];
			}
			return -1;
		}

		public unsafe static float GetTemp(int cell)
		{
			if (Grid.IsValidCell(cell))
			{
				return Grid.temperature[cell];
			}
			return -1;
		}


		private static void Prefix(RoomProber __instance, ref ICollection<int> visited_cells)
		{
			//Debug.Log(" === RoomSizeMod_RoomProber_RebuildDirtyCavities Postfix === ");

			FieldInfo fi1 = AccessTools.Field(typeof(RoomProber), "CellCavityID");
			FieldInfo fi2 = AccessTools.Field(typeof(RoomProber), "cavityInfos");

			foreach (int visited_cell in visited_cells)
			{
				//HandleVector<int>.Handle handle = this.CellCavityID[visited_cell];
				HandleVector<int>.Handle handle = ((HandleVector<int>.Handle[])fi1.GetValue(__instance))[visited_cell];
				//Debug.Log("visited_cell: "+ visited_cell+", handle: " + handle.index);
				if (handle.IsValid())
				{


					Element ele = Grid.Element[visited_cell];
					float mass = GetMass(visited_cell);
					float temperature = GetTemp(visited_cell);
					//Debug.Log("element: "+ele.id.ToString()+", mass: "+mass);

					Dictionary<SimHashes, RoomElementData> roomData = null;
					bool exists = RoomSizeModData.RoomData.TryGetValue(handle.index, out roomData);
					if (!exists)
					{
						roomData = new Dictionary<SimHashes, RoomElementData>();
						RoomSizeModData.RoomData.Add(handle.index, roomData);
					}
					RoomElementData rData = null;
					exists = roomData.TryGetValue(ele.id, out rData);
					if (!exists)
					{
						rData = new RoomElementData();
						rData.element = ele.id;
						roomData.Add(ele.id, rData);
					}
					rData.mass += mass;
					rData.temperature += temperature;
                    rData.numCells += 1;
                    
                }
			}
			//visited_cells.Clear();
		}
	}
    */
}
