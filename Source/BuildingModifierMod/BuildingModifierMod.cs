using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace BuildingModifierMod
{
    internal class PostProcessClass
    {
        public static void PostProcessOld(ref GameObject go)
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

    [HarmonyPatch(typeof(BuildingDef), "PostProcess")]
    internal class BuildingModifierMod_BuildingDef_PostProcess
    {
        private static void Process(ref GameObject go, String componentName, JObject jobj)
        {
            Debug.Log(" === PostProcess === " + go.PrefabID().Name+ " "+jobj.ToString());
            //Storage storage = go.AddOrGet<Storage>();
            //storage.capacityKg = 10000f;
            foreach (JProperty x in (JToken)jobj)
            { // if 'obj' is a JObject
                string name = x.Name;
                JToken value = x.Value;
                Debug.Log(componentName + ", "+name + ": " + value.ToString());
                MethodInfo method = typeof(GameObject).GetMethod("GetComponent", new Type[] { typeof(Type) });
                var component = method.Invoke(go, new object[] { Type.GetType(componentName + ", Assembly-CSharp") });
                FieldInfo fi = AccessTools.Field(Type.GetType(componentName + ", Assembly-CSharp"), name);
                Debug.Log(value+" "+value.Type);
                switch (value.Type) {
                    case JTokenType.Integer:
                        fi.SetValue(component, (int)value);
                        break;
                    case JTokenType.Float:
                        fi.SetValue(component, (float)value);
                        break;
                    case JTokenType.Boolean:
                        fi.SetValue(component, (bool)value);
                        break;
                    case JTokenType.String:
                        //fi.SetValue(component, (string)value);
                        break;
                    case JTokenType.Object:
                        //fi.SetValue(component, (string)value);
                        break;
                    default:
                        break;
                }
            }


        }

        private static void Prefix(BuildingDef __instance)
        {
            //Debug.Log(" === BuildingModifierMod_BuildingDef_PostProcess Prefix === ");
            /*
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

            //MethodInfo postfix = AccessTools.Method(typeof(PostProcessClass), "PostProcess");

            foreach (var method in Assets.BuildingDefs)
            {
                Debug.Log(method.PrefabID);
            }
            */

            foreach (KeyValuePair<string, Dictionary<string, object>> entry in BuildingModifierState.StateManager.State.Modifiers)
            {
                //BuildingDef def = Assets.GetBuildingDef(entry.Key);
                BuildingDef def = __instance;
                if (!def.PrefabID.Equals(entry.Key)) continue;
                Debug.Log(entry.Key);
                Debug.Log(def);

                foreach (KeyValuePair<string, object> modifier in entry.Value)
                {
                    Debug.Log(modifier.Key+": "+ modifier.Value.GetType()+"; "+modifier.Value);
                    Type value = modifier.Value.GetType();
                    if (value.Equals(typeof(JObject)))
                    {
                        try
                        {
                            Process(ref def.BuildingComplete, modifier.Key, (JObject)modifier.Value);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError(ex);
                        }
                    }
                    else if (value.Equals(typeof(Int64)))
                    {
                        FieldInfo fi = AccessTools.Field(typeof(BuildingDef), modifier.Key);
                        fi.SetValue(def, modifier.Value);
                    }
                    else if (value.Equals(typeof(Double)))
                    {
                        FieldInfo fi = AccessTools.Field(typeof(BuildingDef), modifier.Key);
                        fi.SetValue(def, modifier.Value);
                    }
                    else if (value.Equals(typeof(String)))
                    {
                        FieldInfo fi = AccessTools.Field(typeof(BuildingDef), modifier.Key);
                        string path = (string)modifier.Value;
                        string className = path.Substring(0, path.LastIndexOf("."));
                        string fieldName = path.Substring(path.LastIndexOf(".")+1);
                        Debug.Log(className+", "+ fieldName);
                        Type classType = Type.GetType(className + ", Assembly-CSharp");
                        Debug.Log("Type: " + classType);
                        FieldInfo fi2 = AccessTools.Field(classType,fieldName);
                        //var obj = Activator.CreateInstance(tp);
                        Debug.Log("FINAL: " + fi2.GetValue(null));
                        fi.SetValue(def, fi2.GetValue(null));
                    }
                    else if (value.Equals(typeof(Boolean)))
                    {
                        FieldInfo fi = AccessTools.Field(typeof(BuildingDef),modifier.Key);                      
                        fi.SetValue(def, modifier.Value);
                    }

                }
                //PostProcessClass.PostProcess(ref def.BuildingComplete);

                /*
                var original = Type.GetType(entry.Key+", Assembly-CSharp").GetMethod("DoPostConfigureComplete");
                //var prefix = typeof(MyPatchClass1).GetMethod("SomeMethod");
                //var postfix = typeof(TestClass).GetMethod("Modify");
               
                //harmony.Patch(original, new HarmonyMethod(prefix), new HarmonyMethod(postfix));
                harmony.Patch(original, new HarmonyMethod(null), new HarmonyMethod(postfix));
                */

            }

        }

       
    }
}
