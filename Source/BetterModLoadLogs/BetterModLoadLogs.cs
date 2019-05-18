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
                if (Testing.dll_loading == Testing.DLLLoading.Fail)
                {
                    return false;
                }
                if (Testing.dll_loading == Testing.DLLLoading.UseModLoaderDLLExclusively)
                {
                    return false;
                }
                Debug.LogFormat("Using built-in mod system...looking for DLL mods in {0}", Manager.GetDirectory());
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                if (!directoryInfo.Exists)
                {
                    return false;
                }
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
                if (list.Count == 0)
                {
                    return false;
                }
                HarmonyInstance harmonyInstance = HarmonyInstance.Create($"OxygenNotIncluded_v{0}.{1}");
                if (harmonyInstance != null)
                {
                    foreach (Assembly item in list)
                    {
                        harmonyInstance.PatchAll(item);
                    }
                }
                Type[] types = new Type[0];
                Type[] types2 = new Type[1]
                {
                    typeof(string)
                };
                object[] parameters = new object[1]
                {
                    path
                };
                foreach (Assembly item2 in list)
                {
                    Type[] types3 = item2.GetTypes();
                    foreach (Type type in types3)
                    {
                        if (type != null)
                        {
                            type.GetMethod("OnLoad", types)?.Invoke(null, null);
                            type.GetMethod("OnLoad", types2)?.Invoke(null, parameters);
                        }
                    }
                }
                return true;
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
