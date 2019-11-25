using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony;
using Klei.CustomSettings;
using ProcGen;
using ProcGenGame;

namespace CustomWorldSize
{

    //[HarmonyPatch(typeof(GridSettings), nameof(GridSettings.Reset))]
    //public static class GridSettings_Reset
    [HarmonyPatch(typeof(WorldGenSettings), MethodType.Constructor)]
    [HarmonyPatch(new Type[] { typeof(string),typeof(List<string>),typeof(bool) })]
    public static class WorldGenSettings_Constructor
    {
        public const string ModName = "CustomWorldSize";

        public static void Postfix(WorldGenSettings __instance, ref string worldName, ref List<string> traits)
        {
            // 256x512 default

            Debug.Log(" === CWS: Using custom world size ... Default: " + __instance.world.worldsize.x + "/" + __instance.world.worldsize.y +" === ");
            //if (!CustomGameSettings.Get().is_custom_game)
            //if (!CustomGameSettings.Instance.is_custom_game)
            Debug.Log(" === "+CustomGameSettings.Instance.customGameMode+" === ");
            if (CustomGameSettings.Instance.customGameMode != CustomGameSettings.CustomGameMode.Custom)
            {
                Debug.Log(" === CWS: Nah, no custom game ... === ");
                return;
            }

            //SettingConfig settingConfig = CustomGameSettings.Get().QualitySettings[CustomWorldSize.UseCustomWorldSize];
            SettingConfig settingConfig = CustomGameSettings.Instance.QualitySettings[CustomWorldSize.UseCustomWorldSize];
            SettingLevel currentQualitySetting =
            //CustomGameSettings.Get().GetCurrentQualitySetting(CustomWorldSize.UseCustomWorldSize);
            CustomGameSettings.Instance.GetCurrentQualitySetting(CustomWorldSize.UseCustomWorldSize);

            bool allowCustomSize = !settingConfig.IsDefaultLevel(currentQualitySetting.id);

            if (!allowCustomSize)
            {
                Debug.Log(" === CWS: No custom size applied ... === "+ "Using " + __instance.world.worldsize.x + " / " + __instance.world.worldsize.y + " as new world size === ");
                return;
            }

            //SettingLevel currentQualitySettingX = CustomGameSettings.Get().GetCurrentQualitySetting(CustomWorldSize.WorldsizeX);
            //SettingLevel currentQualitySettingY = CustomGameSettings.Get().GetCurrentQualitySetting(CustomWorldSize.WorldsizeY);
            SettingLevel currentQualitySettingX = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomWorldSize.WorldsizeX);
            SettingLevel currentQualitySettingY = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomWorldSize.WorldsizeY);
            

            int width = __instance.world.worldsize.x;
            int height = __instance.world.worldsize.y;
            
            Int32.TryParse(currentQualitySettingX.id, out width);
            Int32.TryParse(currentQualitySettingY.id, out height);
            
            Vector2I worldsize = __instance.world.worldsize;
            worldsize.x = width;
            worldsize.y = height; 
            
            if (worldsize.x > worldsize.y)
            {
                Debug.Log(" === CWS: Fixing width. Width must be equals or less than height. There is a bug in ONI code somewhere :( otherwise, game crashes === ");
                worldsize.x = worldsize.y;
            }

            AccessTools.Property(typeof(ProcGen.World), "worldsize").SetValue(__instance.world, worldsize, null);
            
            Debug.Log(" === CWS: Using " + __instance.world.worldsize.x + "/" + __instance.world.worldsize.y + " as new world size === ");

            //  if (Config.Enabled && Config.CustomWorldSize)
            //{
            //    width  = Config.width;
            //    height = Config.height;
            //}
        }
    }

}