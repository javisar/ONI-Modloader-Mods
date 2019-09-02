using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Harmony;
using KMod;

namespace BetterModLoadLogs
{   

    [HarmonyPatch(typeof(Mod), "Load")]
    internal class BetterModLoadLogs_KMod_Mod_Load
    {
        private static bool IsPatched = false;

        private static bool Prefix(Mod __instance, ref Content content)
        {
            if (IsPatched) return true;
            IsPatched = true;

            Debug.Log(" === BetterModLoadLogs_KMod_Mod_Load Prefix === ");
            Assembly design = System.Reflection.Assembly.GetAssembly(typeof(KMod.Mod));
            Type designHost = design.GetType("KMod.DLLLoader");
            var original = designHost.GetMethod("LoadDLLs");
            var prefix = AccessTools.Method(typeof(BetterModLoadLogs_KMod_Mod_Load), nameof(BetterModLoadLogs_KMod_Mod_Load.LoadDLLs_Prefix));
            HarmonyInstance.Create("BetterModLoadLogs_KMod_DLLLoader_LoadDLLs").Patch(original, new HarmonyMethod(prefix), null, null);

            return true;
        }

        private static bool LoadDLLs_Prefix(ref bool __result, string path)
        {
            __result = LoadDLLs(path);
            return false;
        }

        private static bool LoadDLLs(string path)
        {
            try
            {
                if (Testing.dll_loading != Testing.DLLLoading.Fail)
                {
                    if (Testing.dll_loading != Testing.DLLLoading.UseModLoaderDLLExclusively)
                    {
                        Debug.LogFormat("Using built-in mod system...looking for DLL mods in {0}", Manager.GetDirectory());
                        DirectoryInfo directoryInfo = new DirectoryInfo(path);
                        if (directoryInfo.Exists)
                        {
                            List<Assembly> list = new List<Assembly>();
                            FileInfo[] files = directoryInfo.GetFiles();
                            foreach (FileInfo fileInfo in files)
                            {
                                if (fileInfo.Name.ToLower().EndsWith(".dll"))
                                {
                                    Debug.Log($"Loading MOD dll: {fileInfo.Name}");
                                    Assembly assembly = Assembly.LoadFrom(fileInfo.FullName);
                                    if (assembly != null)
                                    {
                                        list.Add(assembly);
                                    }
                                }
                            }
                            if (list.Count != 0)
                            {
                                ListPool<MethodInfo, Manager>.PooledList pooledList = ListPool<MethodInfo, Manager>.Allocate();
                                ListPool<MethodInfo, Manager>.PooledList pooledList2 = ListPool<MethodInfo, Manager>.Allocate();
                                ListPool<MethodInfo, Manager>.PooledList pooledList3 = ListPool<MethodInfo, Manager>.Allocate();
                                ListPool<MethodInfo, Manager>.PooledList pooledList4 = ListPool<MethodInfo, Manager>.Allocate();
                                Type[] types = new Type[0];
                                Type[] types2 = new Type[1]
                                {
                            typeof(string)
                                };
                                Type[] types3 = new Type[1]
                                {
                            typeof(HarmonyInstance)
                                };
                                MethodInfo methodInfo = null;
                                foreach (Assembly item in list)
                                {
                                    Type[] types4 = item.GetTypes();
                                    foreach (Type type in types4)
                                    {
                                        if (type != null)
                                        {
                                            methodInfo = type.GetMethod("OnLoad", types);
                                            if (methodInfo != null)
                                            {
                                                pooledList3.Add(methodInfo);
                                            }
                                            methodInfo = type.GetMethod("OnLoad", types2);
                                            if (methodInfo != null)
                                            {
                                                pooledList4.Add(methodInfo);
                                            }
                                            methodInfo = type.GetMethod("PrePatch", types3);
                                            if (methodInfo != null)
                                            {
                                                pooledList.Add(methodInfo);
                                            }
                                            methodInfo = type.GetMethod("PostPatch", types3);
                                            if (methodInfo != null)
                                            {
                                                pooledList2.Add(methodInfo);
                                            }
                                        }
                                    }
                                }
                                HarmonyInstance harmonyInstance = HarmonyInstance.Create($"OxygenNotIncluded_v{0}.{1}");
                                if (harmonyInstance != null)
                                {
                                    object[] parameters = new object[1]
                                    {
                                harmonyInstance
                                    };
                                    foreach (MethodInfo item2 in pooledList)
                                    {
                                        item2.Invoke(null, parameters);
                                    }
                                    foreach (Assembly item3 in list)
                                    {
                                        harmonyInstance.PatchAll(item3);
                                    }
                                    foreach (MethodInfo item4 in pooledList2)
                                    {
                                        item4.Invoke(null, parameters);
                                    }
                                }
                                pooledList.Recycle();
                                pooledList2.Recycle();
                                foreach (MethodInfo item5 in pooledList3)
                                {
                                    item5.Invoke(null, null);
                                }
                                object[] parameters2 = new object[1]
                                {
                            path
                                };
                                foreach (MethodInfo item6 in pooledList4)
                                {
                                    item6.Invoke(null, parameters2);
                                }
                                pooledList3.Recycle();
                                pooledList4.Recycle();
                                return true;
                            }
                            return false;
                        }
                        return false;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception obj)
            {
                Debug.Log("Exception LoadDLLs");
                Debug.LogException(obj);
                return false;
            }
        }
    }


    /*
    //var transpiler = typeof(BetterModLoadLogs_KMod_Mod_Load).GetMethod("Transpiler");
    //HarmonyInstance.Create("BetterModLoadLogs_KMod_Manager_Constructor").Patch(original, null, null, new HarmonyMethod(transpiler));
    try
    {
        content &= (Content)(byte)((int)__instance.available_content & (int)(byte)(~(uint)__instance.loaded_content));
        if ((content & Content.DLL) != 0 && LoadDLLs(__instance.label.install_path))
        {
            AccessTools.Property(typeof(Mod), "loaded_content").SetValue(__instance, __instance.loaded_content | Content.DLL, null);
            return false;
        }
    }
    catch (Exception ex)
    {
        Debug.Log("Exception Mod.Load");
        Debug.LogError(ex);
        return false;
    }
    return true;
    */
}
