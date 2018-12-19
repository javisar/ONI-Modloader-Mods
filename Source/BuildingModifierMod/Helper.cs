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
        public static BuildingModifierState Config = BuildingModifierState.StateManager.State;

		public enum BuildingType
		{
			None = 0,
			Building = 1,
			BuildingComplete = 2,
			BuildingPreview = 3,
			BuildingUnderConstruction = 4
		};

        // Applies mod config to building attributes
        public static void Process(BuildingDef def, GameObject go, BuildingType type = BuildingType.None)
        {
            Helper.Log(" === [BuildingModifier] Process === " + def.PrefabID);
            bool error = false;

            ModifiersAll.Add(def.PrefabID);
          
            // If a modifier has been applied, skip it
            if (ModifiersFound.Contains(def.PrefabID)) return;

            // Get building config modifiers
            Dictionary<string, object> entry = Config.Modifiers[def.PrefabID];

            try
            {
                //BuildingDef def = Assets.GetBuildingDef(entry.Key);
                //BuildingDef def = __instance;
                //if (!def.PrefabID.Equals(entry.Key)) continue;
                
                foreach (KeyValuePair<string, object> modifier in entry)
                {
                    try
                    {
						Type valueType = modifier.Value.GetType();
						Helper.Log(" === [BuildingModifier] "+modifier.Key + ": " + valueType + "; " + modifier.Value);

						ModifiersAll.Add(def.PrefabID + "_" + modifier.Key);

						if (valueType.Equals(typeof(JObject)))
						{       // Is a Component of the building
							try
							{
								GameObject building = null;

								if (type == BuildingType.Building)
									building = go;
								else if (type == BuildingType.BuildingComplete)
									building = def.BuildingComplete;
								else if (type == BuildingType.BuildingPreview)
									building = def.BuildingPreview;
								else if(type == BuildingType.BuildingUnderConstruction)
									building = def.BuildingUnderConstruction;

								if (building != null)
								{
                                    UnityEngine.Object _building = building;
                                    ProcessObject(ref _building, def, modifier.Key, modifier);

									Debug.Log(" === [BuildingModifier] Found: " + def.PrefabID + "_" + modifier.Key);
									ModifiersFound.Add(def.PrefabID + "_" + modifier.Key);
								}
								else // def.Building
								{
									error = true;
								}
							}
							catch (Exception ex)
							{
								//Debug.LogError(ex);
								error = true;
								Debug.Log(" === [BuildingModifier] JObject Warning: " + def.PrefabID + "_" + modifier.Key);
							}
						}   
                        /*
                        else if (valueType.Equals(typeof(String)))
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
                        */
                        else
                        {   // Basic attributes in BuildingDef with system types

                            bool found = SetValue(def, new JProperty(modifier.Key, modifier.Value), typeof(BuildingDef));                           
                            Debug.Log(" === [BuildingModifier] Found: " + def.PrefabID + "_" + modifier.Key);
                            ModifiersFound.Add(def.PrefabID + "_" + modifier.Key);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);

						//Debug.Log(ex.StackTrace);
						Debug.Log(" === [BuildingModifier] Attribute Warning: " + def.PrefabID + "_" + modifier.Key);

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


        }        
        
        private static void ProcessObject(ref UnityEngine.Object go, BuildingDef buildingDef, String componentName, KeyValuePair<string, object> modifier)
        {
            Debug.Log(" === [BuildingModifier] ProcessObject === " + go.name+ " "+componentName);

            // For every component in the building
            foreach (JProperty x in (JToken)modifier.Value)
            {                
                string name = x.Name;
                JToken value = x.Value;
                try
                {
                    //Debug.Log(componentName + ", " + name + ": " + value.ToString());

                    if (ModifiersFound.Contains(buildingDef.PrefabID + "_" + componentName + "_" + name))
                        continue;

                    ModifiersAll.Add(buildingDef.PrefabID + "_" + componentName + "_" + name);

                    // Tries to find the component
                    MethodInfo method = typeof(GameObject).GetMethod("GetComponent", new Type[] { typeof(Type) });
					//Debug.Log("method: " + method);
                    Component component = (Component) method.Invoke(go, new object[] { Type.GetType(componentName + ", Assembly-CSharp") });
                    //Debug.Log("component: " + component);

                    if (x.Value.Type.Equals(JTokenType.Object))
                    {
                        if (component.GetType().Equals(typeof(Component)))
                        {
                            //fi.SetValue(component, (string)value);
                            //Debug.Log(" === [BuildingModifier] Warning: JTokenType.Object Not implemented. " + buildingName + "_" + componentName + "_" + name + ": " + value);
                            //continue;

                            //Component _component = (Component)component;
                            //ProcessComponent(ref _component, buildingDef, component.name, name, (JObject)value);

                            UnityEngine.Object _component =component;
                            ProcessObject(ref _component, buildingDef, component.name, ((JObject)value).ToObject<KeyValuePair<string, object>>());
                        }
                        else
                        {
                            Debug.Log(" === [BuildingModifier] Warning: JTokenType.Object Not implemented. " + component.name + "_" + component.name + "_" + name + ": " + value);
                        }
                    }
                    else
                    {
						//Debug.Log("component: " + component);
						//Debug.Log("x: " + x);
						//Debug.Log("getType: " + Type.GetType(componentName + ", Assembly-CSharp"));
						bool found = SetValue(component, x, Type.GetType(componentName + ", Assembly-CSharp"));
						//Debug.Log("found: " + found);
					}
                    
                    Debug.Log(" === [BuildingModifier] Found: " + buildingDef.PrefabID + "_" + componentName + "_" + name);
                    ModifiersFound.Add(buildingDef.PrefabID + "_" + componentName + "_" + name);
                }
                catch (Exception ex)
                {
                    //Debug.LogError(ex);
                    Debug.Log(" === [BuildingModifier] Warning: " + buildingDef.PrefabID + "_" + componentName + "_" + name);
                    throw ex;
                }
            }

        }

        private static bool SetValue(UnityEngine.Object component, JProperty property, Type type)
        {
            string name = property.Name;
            JToken value = property.Value;
            if (value == null) Debug.Log(String.Format(" === [BuildingModifier] Warning: null value for property {0} while processing type {1}", property.Name, type.Name));


            FieldInfo fi = AccessTools.Field(type, name);
            //Debug.Log("fi: " + fi);
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
					//Debug.Log(" === [BuildingModifier] Warning: JTokenType.String Not implemented. " + "_" + component.name + "_" + name + ": " + (string)value);
					// Basic attributes in BuildingDef with complex Types

					// Tries to find the Type
					//FieldInfo fi1 = AccessTools.Field(typeof(BuildingDef), name);
					FieldInfo fi1 = AccessTools.Field(type, name);
                    if (null == fi1)
                    {
                        Debug.Log(String.Format(" === [BuildingModifier] Warning: can't find field {0}.{1}", type.Name, name));
                        return false;
                    }
                    string path = (string)value;
                    string className = path.Substring(0, path.LastIndexOf("."));
                    string fieldName = path.Substring(path.LastIndexOf(".") + 1);
//                    Debug.Log(className + ", " + fieldName);
                    Type classType = Type.GetType(className + ", Assembly-CSharp");
                    if (null == classType)
                    {
                        Debug.Log(String.Format(" === [BuildingModifier] Warning: can't find class {0} in Assembly-CSharp assembly", className));
                        return false;
                    }
//                    Debug.Log("Type: " + classType);
                    FieldInfo fi2 = AccessTools.Field(classType, fieldName);
                    if (null == fi2)
                    {
                        Debug.Log(String.Format(" === [BuildingModifier] Warning: can't find field {0}.{1}", classType.Name, fieldName));
                        return false;
                    }
                    //Debug.Log("FINAL: " + fi2.GetValue(null));

                    fi1.SetValue(component, fi2.GetValue(null));
                    //Debug.Log(" === [BuildingModifier] Found: " + component.name + "_" + name + ": " + value);
                    break;

                case JTokenType.Object:
                    Debug.Log(" === [BuildingModifier] Warning: JTokenType.Object Not implemented. " + (component ? component.name : "null") + "_" + name + ": " + value);
                    break;

                default:
                    Debug.Log(" === [BuildingModifier] Warning: " + value.Type + " Not implemented. " + "_" + (component ? component.name : "null") + "_" + name + ": " + value);
                    return false;
            }
            return true;
        }

        public static void Log(string txt)
        {
            if (Config.Debug)
                Debug.Log(txt);
        }

        

    }
}
