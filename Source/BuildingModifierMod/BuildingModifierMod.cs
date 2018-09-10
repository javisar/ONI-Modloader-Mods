using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BuildingModifierMod
{
    internal class PostProcessClass
    {
        private static void PostProcess(ref GameObject go)
        {
            Debug.Log(" === PostProcess === " + go.PrefabID().Name);
            //Storage storage = go.AddOrGet<Storage>();
            //storage.capacityKg = 10000f;
           
            //var test = BuildingModifierState.StateManager.State.Modifiers[go.PrefabID];

            // MethodInfo method = typeof(EntityTemplateExtensions).GetMethod("AddOrGet");
            MethodInfo method = typeof(GameObject).GetMethod("GetComponent",new Type[] {typeof(Type) });

            Debug.Log("a"+method);
            //MethodInfo generic = method.MakeGenericMethod(Type.GetType("Storage, Assembly-CSharp"));
            //Debug.Log("b"+ generic);
            var component = method.Invoke(go, new object[] { Type.GetType("Storage, Assembly-CSharp") });
            Debug.Log("c"+ component);
            FieldInfo fi = AccessTools.Field(Type.GetType("Storage, Assembly-CSharp"), "capacityKg");
            Debug.Log("d"+fi);
            fi.SetValue(component, 10000f);
            Debug.Log("e");
        }
    }

    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class BuildingModifierMod_GeneratedBuildings_LoadGeneratedBuildings
    {
        private static void Prefix()
        {
            //var harmony = HarmonyInstance.Create("Assembly-CSharp");
            //harmony.PatchAll(Assembly.GetExecutingAssembly());
            //var harmony = HarmonyInstance.Create("ONI-Modloader");
            var harmony = HarmonyInstance.Create("Assembly-CSharp");
            //harmony.PatchAll(Assembly.GetExecutingAssembly());
            var methods = harmony.GetPatchedMethods();
            foreach (var method in methods)
            {
                Debug.Log(method.ToString());
            }
            //harmony.PatchAll(Assembly.LoadFrom("Assembly-CSharp"));

            MethodInfo postfix = AccessTools.Method(typeof(PostProcessClass), "PostProcess");

            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, int>>> entry in BuildingModifierState.StateManager.State.Modifiers)
            {
                var original = Type.GetType(entry.Key+", Assembly-CSharp").GetMethod("DoPostConfigureComplete");
                //var prefix = typeof(MyPatchClass1).GetMethod("SomeMethod");
                //var postfix = typeof(TestClass).GetMethod("Modify");
               
                //harmony.Patch(original, new HarmonyMethod(prefix), new HarmonyMethod(postfix));
                harmony.Patch(original, new HarmonyMethod(null), new HarmonyMethod(postfix));
                
            }
            
        }

       
    }
}
