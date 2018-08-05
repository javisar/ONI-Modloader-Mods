using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomSizeMod
{
    [HarmonyPatch(typeof(RoomProber))]
    internal class RoomSizeMod_RoomProber
    {
      
        private static void Postfix(RoomProber __instance)
        {
            Debug.Log(" === RoomSizeMod_RoomProber Postfix === ");
            RoomProber.MaxRoomSize = 1024;
        }
    }
}
