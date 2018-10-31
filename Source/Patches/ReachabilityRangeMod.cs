using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace ReachabilityRangeMod
{
	[HarmonyPatch(typeof(OffsetGroups), "BuildReachabilityTable")]
	internal class ReachabilityRangeMod_OffsetGroups_BuildReachabilityTable
	{

		private static void Prefix(CellOffset[] area_offsets, CellOffset[][] table, CellOffset[] filter)
		{
			Debug.Log(" === ReachabilityRangeMod_OffsetGroups_BuildReachabilityTable Prefix === ");
			Debug.Log("area_offsets: ");
			foreach (CellOffset a in area_offsets)
			{
				Debug.Log(a.ToString());
			}

			Debug.Log("table: ");
			foreach (CellOffset[] a in table)
			{
				foreach (CellOffset b in a)
				{
					Debug.Log(b.ToString());
				}
			}

			Debug.Log("filter: ");
			if (filter !=  null) 
				foreach (CellOffset a in filter)
				{
					Debug.Log(a.ToString());
				}
		}

		private static void Postfix(CellOffset[][] __result, CellOffset[] area_offsets, CellOffset[][] table, CellOffset[] filter)
		{
			Debug.Log(" === ReachabilityRangeMod_OffsetGroups_BuildReachabilityTable Postfix === ");
			foreach (CellOffset[] a in __result)
			{
				foreach (CellOffset b in a)
				{
					Debug.Log(b.ToString());
				}
			}
		}
	}

	[HarmonyPatch(typeof(Global),"Awake")]
	internal class ReachabilityRangeMod_Global_ctor
	{

		private static void Postfix()
		{
			Debug.Log(" === ReachabilityRangeMod_Global_ctor Postfix === ");
			//OffsetGroups.InvertedStandardTable = InvertedStandardTable_Alt;
			//OffsetGroups.InvertedStandardTableWithCorners = InvertedStandardTableWithCorners_Alt;
		}
		/*
		public static CellOffset[][] InvertedStandardTable_Alt = OffsetTable.Mirror(new CellOffset[28][]
		{
			new CellOffset[1]
			{
				new CellOffset(0, 0)
			},
			new CellOffset[1]
			{
				new CellOffset(0, 1)
			},
			new CellOffset[2]
			{
				new CellOffset(0, 2),
				new CellOffset(0, 1)
			},
			new CellOffset[3]
			{
				new CellOffset(0, 3),
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			},
			new CellOffset[1]
			{
				new CellOffset(0, -1)
			},
			new CellOffset[1]
			{
				new CellOffset(0, -2)
			},
			new CellOffset[3]
			{
				new CellOffset(0, -3),
				new CellOffset(0, -2),
				new CellOffset(0, -1)
			},
			new CellOffset[1]
			{
				new CellOffset(1, 0)
			},
			new CellOffset[2]
			{
				new CellOffset(1, 1),
				new CellOffset(0, 1)
			},
			new CellOffset[2]
			{
				new CellOffset(1, 1),
				new CellOffset(1, 0)
			},
			new CellOffset[3]
			{
				new CellOffset(1, 2),
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			},
			new CellOffset[3]
			{
				new CellOffset(1, 2),
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			},
			new CellOffset[3]
			{
				new CellOffset(1, 3),
				new CellOffset(1, 2),
				new CellOffset(1, 1)
			},
			new CellOffset[4]
			{
				new CellOffset(1, 3),
				new CellOffset(0, 1),
				new CellOffset(0, 2),
				new CellOffset(0, 3)
			},
			new CellOffset[1]
			{
				new CellOffset(1, -1)
			},
			new CellOffset[3]
			{
				new CellOffset(1, -2),
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			},
			new CellOffset[3]
			{
				new CellOffset(1, -2),
				new CellOffset(1, -1),
				new CellOffset(0, -1)
			},
			new CellOffset[3]
			{
				new CellOffset(1, -3),
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			},
			new CellOffset[3]
			{
				new CellOffset(1, -3),
				new CellOffset(0, -1),
				new CellOffset(0, -2)
			},
			new CellOffset[3]
			{
				new CellOffset(1, -3),
				new CellOffset(0, -1),
				new CellOffset(-1, -1)
			},
			new CellOffset[2]
			{
				new CellOffset(2, 0),
				new CellOffset(1, 0)
			},
			new CellOffset[3]
			{
				new CellOffset(2, 1),
				new CellOffset(1, 1),
				new CellOffset(0, 1)
			},
			new CellOffset[3]
			{
				new CellOffset(2, 1),
				new CellOffset(1, 1),
				new CellOffset(1, 0)
			},
			new CellOffset[3]
			{
				new CellOffset(2, 2),
				new CellOffset(1, 2),
				new CellOffset(1, 1)
			},
			new CellOffset[4]
			{
				new CellOffset(2, 3),
				new CellOffset(1, 1),
				new CellOffset(1, 2),
				new CellOffset(1, 3)
			},
			new CellOffset[3]
			{
				new CellOffset(2, -1),
				new CellOffset(2, 0),
				new CellOffset(1, 0)
			},
			new CellOffset[4]
			{
				new CellOffset(2, -2),
				new CellOffset(1, 0),
				new CellOffset(1, -1),
				new CellOffset(2, -1)
			},
			new CellOffset[4]
			{
				new CellOffset(2, -3),
				new CellOffset(1, 0),
				new CellOffset(1, -1),
				new CellOffset(1, -2)
			}
		});
		*/

		public static CellOffset[][] InvertedStandardTableWithCorners_Alt = OffsetTable.Mirror(new CellOffset[27][]
		{
			new CellOffset[1]
			{
				new CellOffset(0, 0)
			},
			new CellOffset[1]
			{
				new CellOffset(0, 1)
			},
			new CellOffset[2]
			{
				new CellOffset(0, 2),
				new CellOffset(0, 1)
			},
			new CellOffset[3]
			{
				new CellOffset(0, 3),
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			},
			new CellOffset[1]
			{
				new CellOffset(0, -1)
			},
			new CellOffset[1]
			{
				new CellOffset(0, -2)
			},
			new CellOffset[3]
			{
				new CellOffset(0, -3),
				new CellOffset(0, -2),
				new CellOffset(0, -1)
			},
			new CellOffset[1]
			{
				new CellOffset(1, 0)
			},
			new CellOffset[1]
			{
				new CellOffset(1, 1)
			},
			new CellOffset[2]
			{
				new CellOffset(1, 1),
				new CellOffset(1, 0)
			},
			new CellOffset[3]
			{
				new CellOffset(1, 2),
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			},
			new CellOffset[3]
			{
				new CellOffset(1, 2),
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			},
			new CellOffset[3]
			{
				new CellOffset(1, 3),
				new CellOffset(1, 2),
				new CellOffset(1, 1)
			},
			new CellOffset[4]
			{
				new CellOffset(1, 3),
				new CellOffset(0, 1),
				new CellOffset(0, 2),
				new CellOffset(0, 3)
			},
			new CellOffset[1]
			{
				new CellOffset(1, -1)
			},
			new CellOffset[3]
			{
				new CellOffset(1, -2),
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			},
			new CellOffset[2]
			{
				new CellOffset(1, -2),
				new CellOffset(1, -1)
			},
			new CellOffset[4]
			{
				new CellOffset(1, -3),
				new CellOffset(1, 0),
				new CellOffset(1, -1),
				new CellOffset(1, -2)
			},
			new CellOffset[5]		// Nuevo
			{
				new CellOffset(1, -4),
				new CellOffset(1, -3),
				new CellOffset(1, 0),
				new CellOffset(1, -1),
				new CellOffset(1, -2)
			},
			new CellOffset[3]
			{
				new CellOffset(1, -3),
				new CellOffset(1, -2),
				new CellOffset(1, -1)
			},
			new CellOffset[2]
			{
				new CellOffset(2, 0),
				new CellOffset(1, 0)
			},
			new CellOffset[2]
			{
				new CellOffset(2, 1),
				new CellOffset(1, 1)
			},
			new CellOffset[3]
			{
				new CellOffset(2, 2),
				new CellOffset(1, 2),
				new CellOffset(1, 1)
			},
			new CellOffset[4]
			{
				new CellOffset(2, 3),
				new CellOffset(1, 1),
				new CellOffset(1, 2),
				new CellOffset(1, 3)
			},
			new CellOffset[3]
			{
				new CellOffset(2, -1),
				new CellOffset(2, 0),
				new CellOffset(1, 0)
			},
			new CellOffset[4]
			{
				new CellOffset(2, -2),
				new CellOffset(1, 0),
				new CellOffset(1, -1),
				new CellOffset(2, -1)
			},
			new CellOffset[4]
			{
				new CellOffset(2, -3),
				new CellOffset(1, 0),
				new CellOffset(1, -1),
				new CellOffset(1, -2)
			}
		});

	}
}
