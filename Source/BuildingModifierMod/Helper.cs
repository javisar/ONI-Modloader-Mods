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
        //private static HashSet<string> ModifiersRecheck = new HashSet<string>();
        public static BuildingModifierState Config = BuildingModifierState.StateManager.State;
        private static string State = null;

		public enum BuildingType
		{
			None = 0,
			Building = 1,
			BuildingComplete = 2,
			BuildingPreview = 3,
			BuildingUnderConstruction = 4
		};

        // Applies mod config to building attributes
        public static void Process(string state, BuildingDef def, GameObject go, BuildingType type = BuildingType.None)
        {
            State = state;
            Helper.Log(" === [BuildingModifier] Process("+State+")=== " + def.PrefabID);

            bool allgood = true;

            ModifiersAll.Add(def.PrefabID);

            //Debug.Log("def.PrefabID 1 = "+ def.PrefabID);
            // If a modifier has been applied, skip it
            if (ModifiersFound.Contains(def.PrefabID)) return;
            //Debug.Log("def.PrefabID 2 = " + def.PrefabID);

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
						
						ModifiersAll.Add(def.PrefabID + "_" + modifier.Key);

                        //Debug.Log("def.PrefabID_modifier.Key 1 = " + def.PrefabID + "_" + modifier.Key);
                        if (ModifiersFound.Contains(def.PrefabID + "_" + modifier.Key)) continue;
                        //Debug.Log("def.PrefabID_modifier.Key 2 = " + def.PrefabID + "_" + modifier.Key);

                        Helper.Log(" === [BuildingModifier] " + modifier.Key + ": " + valueType + "; " + modifier.Value);

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
                                    bool found =  ProcessObject(ref _building, def, modifier.Key, modifier);

									Debug.Log(" === [BuildingModifier] Found: " + def.PrefabID + "_" + modifier.Key);
                                    if (found)
                                        ModifiersFound.Add(def.PrefabID + "_" + modifier.Key);
                                    allgood = allgood && found;

                                }
								else // def.Building
								{
                                    allgood = allgood && false;
                                    Debug.Log(" === [BuildingModifier] JObject Warning def.Building: " + def.PrefabID + "_" + modifier.Key);
                                    
								}
							}
							catch (Exception ex)
							{
                                //Debug.LogError(ex);    
                                allgood = allgood && false;
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
                            if (found)
                                ModifiersFound.Add(def.PrefabID + "_" + modifier.Key);
                            allgood = allgood && found;
                        }

                        //Debug.Log("allgood = "+allgood);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                        allgood = false;
                        //Debug.Log(ex.StackTrace);
                        Debug.Log(" === [BuildingModifier] Attribute Warning: " + def.PrefabID + "_" + modifier.Key);

                    }
                }

            }
            catch (Exception ex)
            {
                allgood = true;
                //Debug.LogError(ex);
                Debug.Log(" === [BuildingModifier] Warning: " + def.PrefabID + "_" + entry);
            }

            if (allgood)
            {
                Debug.Log(" === [BuildingModifier] Found: " + def.PrefabID);
                ModifiersFound.Add(def.PrefabID);
            }


        }        
        
        private static bool ProcessObject(ref UnityEngine.Object go, BuildingDef buildingDef, String componentName, KeyValuePair<string, object> modifier)
        {
            Debug.Log(" === [BuildingModifier] ProcessObject === " + go.name+ " "+componentName);
            //Debug.Log(" State = " +State);
            bool allgood = true;
            // For every component in the building
            foreach (JProperty x in (JToken)modifier.Value)
            {                
                string name = x.Name;
                JToken value = x.Value;
                string state = "";
                if (name.IndexOf("[") >= 0)
                    state = name.Substring(name.IndexOf('[') + 1, name.IndexOf(']') - name.IndexOf('[') - 1);

                //Debug.Log("state = " + state);

                if (name.IndexOf("[") >= 0)
                {
                    if (!state.Equals(State))
                    {
                        allgood = allgood && false;
                        continue;
                    }
                }
               
                try
                {
                    //Debug.Log(componentName + ", " + name + ": " + value.ToString());

                    if (ModifiersFound.Contains(buildingDef.PrefabID + "_" + componentName + "_" + name))
                        continue;

                    ModifiersAll.Add(buildingDef.PrefabID + "_" + componentName + "_" + name);

					// Tries to find the component
					MethodInfo method = typeof(GameObject).GetMethod("GetComponent", new Type[] { typeof(Type) });
					//Debug.Log("method: " + method);
					Component component = (Component)method.Invoke(go, new object[] { Type.GetType(componentName + ", Assembly-CSharp") });
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
                            bool found = ProcessObject(ref _component, buildingDef, component.name, ((JObject)value).ToObject<KeyValuePair<string, object>>());
                            if (found)
                                ModifiersFound.Add(buildingDef.PrefabID + "_" + componentName + "_" + name);
                            allgood = allgood && found;
                        }
                        else
                        {
                            Debug.Log(" === [BuildingModifier] Warning: JTokenType.Object Not implemented. " + component.name + "_" + component.name + "_" + name + ": " + value);
                        }

                        Debug.Log(" === [BuildingModifier] Found: " + buildingDef.PrefabID + "_" + componentName + "_" + name);
                        //if (name.IndexOf('[') >= 0 && name.IndexOf(']') >= 0)
                       
                    }
                    else
                    {
                        //Debug.Log("component: " + component);
                        //Debug.Log("x: " + x);
                        //Debug.Log("getType: " + Type.GetType(componentName + ", Assembly-CSharp"));

                        bool found = SetValue(component, x, Type.GetType(componentName + ", Assembly-CSharp"));
                        //Debug.Log("found: " + found);

                        Debug.Log(" === [BuildingModifier] Found: " + buildingDef.PrefabID + "_" + componentName + "_" + name);
                        //if (name.IndexOf('[') >= 0 && name.IndexOf(']') >= 0)
                        if (found)
                            ModifiersFound.Add(buildingDef.PrefabID + "_" + componentName + "_" + name);
                        allgood = allgood && found;
                        //Debug.Log("allgood: " + allgood);
                    }
             
                }
                catch (Exception ex)
                {
                    //Debug.LogError(ex);
                    Debug.Log(" === [BuildingModifier] Warning: " + buildingDef.PrefabID + "_" + componentName + "_" + name);
                    throw ex;
                }
            }
            return allgood;

        }

        private static bool SetValue(UnityEngine.Object component, JProperty property, Type type)
        {            
            string name = property.Name;
            if (name.IndexOf('[') >= 0 && name.IndexOf(']') >= 0)
            {
                name = name.Substring(0, name.IndexOf('['));
            }

            JToken value = property.Value;
            if (value == null) Debug.Log(String.Format(" === [BuildingModifier] Warning: null value for property {0} while processing type {1}", property.Name, type.Name));

			//Debug.Log("type: " + type);
			//Debug.Log("name: " + name);
			//Debug.Log("value: " + value);
			FieldInfo fi = AccessTools.Field(type, name);
			PropertyInfo pi = AccessTools.Property(type, name);

			//Debug.Log("fi: " + fi);
			//Debug.Log("pi: " + pi);
			//Debug.Log(value + " " + value.Type);
            switch (value.Type)
            {
                case JTokenType.Integer:
                    if (fi != null) fi.SetValue(component, (int)value);
					else if (pi != null) pi.SetValue(component, (int)value, null);
					break;
                case JTokenType.Float:
					if (fi != null) fi.SetValue(component, (float)value);
					else if (pi != null) pi.SetValue(component, (float)value, null);
					break;
                case JTokenType.Boolean:
                    if (fi != null) fi.SetValue(component, (bool)value);
					else if (pi != null) pi.SetValue(component, (bool)value, null);
					break;
                case JTokenType.String:
					//fi.SetValue(component, (string)value);
					//Debug.Log(" === [BuildingModifier] Warning: JTokenType.String Not implemented. " + "_" + component.name + "_" + name + ": " + (string)value);
					// Basic attributes in BuildingDef with complex Types

					// Tries to find the Type
					//FieldInfo fi1 = AccessTools.Field(typeof(BuildingDef), name);
					FieldInfo fi1 = AccessTools.Field(type, name);
					PropertyInfo pi1 = AccessTools.Property(type, name);

					if (fi1 == null && pi1 == null)
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
					PropertyInfo pi2 = AccessTools.Property(classType, fieldName);
					if (fi1 == null && pi1 == null)
					{
                        Debug.Log(String.Format(" === [BuildingModifier] Warning: can't find field {0}.{1}", classType.Name, fieldName));
                        return false;
                    }
					//Debug.Log("FINAL: " + fi2.GetValue(null));

					if (fi1 != null)
					{
						if (fi2 != null)
							fi1.SetValue(component, fi2.GetValue(null));
						else if (fi2 != null)
							fi1.SetValue(component, pi2.GetValue(null, null));
					}
					else
					{
						if (fi2 != null)
							pi1.SetValue(component, fi2.GetValue(null),null);
						else if (fi2 != null)
							pi1.SetValue(component, pi2.GetValue(null,null),null);
					}
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
