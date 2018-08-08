using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace RoomSizeMod
{

    [HarmonyPatch(typeof(RoomProber))]
    internal class RoomSizeMod_RoomProber
    {

        private static void Postfix(RoomProber __instance)
        {
            //Debug.Log(" === RoomSizeMod_RoomProber Postfix === ");
            RoomProber.MaxRoomSize = 1024;
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
