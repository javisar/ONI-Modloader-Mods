using Harmony;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BuildingModifierMod
{
    public class Helper
    {       
        public static HashSet<string> ModifiersAll = new HashSet<string>();
        public static HashSet<string> ModifiersFound = new HashSet<string>();

        // Applies mod config to building attributes
        public static void Process(BuildingDef def)
        {
            Helper.Log(" === [BuildingModifier] Process === " + def.PrefabID);
            bool error = false;

            ModifiersAll.Add(def.PrefabID);
          
            // If a modifier has been applied, skip it
            if (ModifiersFound.Contains(def.PrefabID)) return;

            // Get building mod modifiers
            Dictionary<string, object> entry = BuildingModifierState.StateManager.State.Modifiers[def.PrefabID];

            try
            {
                //BuildingDef def = Assets.GetBuildingDef(entry.Key);
                //BuildingDef def = __instance;
                //if (!def.PrefabID.Equals(entry.Key)) continue;

                //Debug.Log(entry.Key);
                //Debug.Log(def);

                foreach (KeyValuePair<string, object> modifier in entry)
                {
                    try
                    {
                        //Debug.Log(modifier.Key + ": " + modifier.Value.GetType() + "; " + modifier.Value);

                        ModifiersAll.Add(def.PrefabID + "_" + modifier.Key);

                        Type value = modifier.Value.GetType();
                        if (value.Equals(typeof(JObject)))
                        {       // Is a Component of the building
                            try
                            {
                                if (def.BuildingComplete != null)
                                {
                                    ProcessComponent(ref def.BuildingComplete, def.PrefabID, modifier.Key, (JObject)modifier.Value);
                                    Debug.Log(" === [BuildingModifier] Found: " + def.PrefabID + "_" + modifier.Key);
                                    ModifiersFound.Add(def.PrefabID + "_" + modifier.Key);
                                }
                                else // def.BuildingComplete still not present
                                {
                                    error = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                //Debug.LogError(ex);
                                error = true;
                                Debug.Log(" === [BuildingModifier] Warning: " + def.PrefabID + "_" + modifier.Key);
                            }
                        }
                        else if (value.Equals(typeof(Int64))
                            || value.Equals(typeof(Double))
                            || value.Equals(typeof(Boolean))
                            )
                        {   // Basic attributes in BuildingDef with system types

                            FieldInfo fi = AccessTools.Field(typeof(BuildingDef), modifier.Key);
                            fi.SetValue(def, modifier.Value);
                            Debug.Log(" === [BuildingModifier] Found: " + def.PrefabID + "_" + modifier.Key);
                            ModifiersFound.Add(def.PrefabID + "_" + modifier.Key);
                        }                        
                        else if (value.Equals(typeof(String)))
                        {   // Basic attributes in BuildingDef with complex Types
                            
                            // Tries to find the Type
                            FieldInfo fi = AccessTools.Field(typeof(BuildingDef), modifier.Key);
                            string path = (string)modifier.Value;
                            string className = path.Substring(0, path.LastIndexOf("."));
                            string fieldName = path.Substring(path.LastIndexOf(".") + 1);
                            //Debug.Log(className + ", " + fieldName);
                            Type classType = Type.GetType(className + ", Assembly-CSharp");
                            //Debug.Log("Type: " + classType);
                            FieldInfo fi2 = AccessTools.Field(classType, fieldName);
                            //Debug.Log("FINAL: " + fi2.GetValue(null));

                            fi.SetValue(def, fi2.GetValue(null));
                            Debug.Log(" === [BuildingModifier] Found: " + def.PrefabID + "_" + modifier.Key);
                            ModifiersFound.Add(def.PrefabID + "_" + modifier.Key);
                        }                        
                    }
                    catch (Exception ex)
                    {
                        //Debug.LogError(ex);
                        Debug.Log(" === [BuildingModifier] Warning: " + def.PrefabID + "_" + modifier.Key);

                    }
                }

            }
            catch (Exception ex)
            {
                error = true;
                //Debug.LogError(ex);
                Debug.Log(" === [BuildingModifier] Warning: " + def.PrefabID + "_" + entry);
            }

            if (!error)
            {
                Debug.Log(" === [BuildingModifier] Found: " + def.PrefabID);
                ModifiersFound.Add(def.PrefabID);
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

        public static void ProcessComponent(ref GameObject go, String buildingName, String componentName, JObject jobj)
        {
            Debug.Log(" === [BuildingModifier] ProcessComponent === " + go.PrefabID().Name);

            // For every component in the building
            foreach (JProperty x in (JToken)jobj)
            {                
                string name = x.Name;
                JToken value = x.Value;
                try
                {
                    //Debug.Log(componentName + ", " + name + ": " + value.ToString());

                    if (ModifiersFound.Contains(buildingName + "_" + componentName + "_" + name))
                        continue;

                    ModifiersAll.Add(buildingName + "_" + componentName + "_" + name);

                    // Tries to find the component
                    MethodInfo method = typeof(GameObject).GetMethod("GetComponent", new Type[] { typeof(Type) });
                    var component = method.Invoke(go, new object[] { Type.GetType(componentName + ", Assembly-CSharp") });
                    FieldInfo fi = AccessTools.Field(Type.GetType(componentName + ", Assembly-CSharp"), name);
                    //Debug.Log(value + " " + value.Type);
                    switch (value.Type)
                    {
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
                            Debug.Log(" === [BuildingModifier] Warning: Not implemented. +"+ buildingName + "_" + componentName + "_" + name+": "+(string)value);
                            break;
                        case JTokenType.Object:
                            //fi.SetValue(component, (string)value);
                            Debug.Log(" === [BuildingModifier] Warning: Not implemented. +" + buildingName + "_" + componentName + "_" + name + ": " + value);
                            break;
                        default:
                            break;
                    }
                    Debug.Log(" === [BuildingModifier] Found: " + buildingName + "_" + componentName + "_" + name);
                    ModifiersFound.Add(buildingName + "_" + componentName + "_" + name);
                }
                catch (Exception ex)
                {
                    //Debug.LogError(ex);
                    Debug.Log(" === [BuildingModifier] Warning: " + buildingName + "_" + componentName + "_" + name);
                    throw ex;
                }
            }

        }

        public static void Log(string txt)
        {
            if (BuildingModifierState.StateManager.State.Debug)
                Debug.Log(" === [BuildingModifier] Process === " + txt);
        }


        /*
		public static void PostProcessOld(ref GameObject go)
		{
			Debug.Log(" === PostProcess === " + go.PrefabID().Name);
			//Storage storage = go.AddOrGet<Storage>();
			//storage.capacityKg = 10000f;

			//var test = BuildingModifierState.StateManager.State.Modifiers[go.PrefabID];

			// MethodInfo method = typeof(EntityTemplateExtensions).GetMethod("AddOrGet");
			MethodInfo method = typeof(GameObject).GetMethod("GetComponent", new Type[] { typeof(Type) });

			Debug.Log("a" + method);
			//MethodInfo generic = method.MakeGenericMethod(Type.GetType("Storage, Assembly-CSharp"));
			//Debug.Log("b"+ generic);
			var component = method.Invoke(go, new object[] { Type.GetType("Storage, Assembly-CSharp") });
			Debug.Log("c" + component);
			FieldInfo fi = AccessTools.Field(Type.GetType("Storage, Assembly-CSharp"), "capacityKg");
			Debug.Log("d" + fi);
			fi.SetValue(component, 10000f);
			Debug.Log("e");
		}
		*/

    }
}
