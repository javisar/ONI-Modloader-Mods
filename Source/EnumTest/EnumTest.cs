using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace EnumTest
{
    [HarmonyPatch(typeof(App), "Awake")]
    internal class EnumTest_App_Awake
    {

        private static void Postfix(Game __instance)
        {            
            Debug.Log(" === EnumTest_App_Awake Postfix === ");
            foreach (object o in Enum.GetValues(typeof(SimHashes)))
            {
                Console.WriteLine("{0}.{1} = {2}", typeof(SimHashes), o, ((int)o));
            }
        }
    }
}
