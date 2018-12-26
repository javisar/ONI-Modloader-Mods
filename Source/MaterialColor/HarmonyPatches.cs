namespace MaterialColor
{
    using Harmony;
    using Extensions;
    using Helpers;
    using ONI_Common.Core;
    using ONI_Common.IO;
    using Rendering;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using JetBrains.Annotations;

    using UnityEngine;
    using Action = Action;
	using static KInputController;
	using System.Reflection;
    using MaterialColor.Data;
    using MaterialColor.IO;

    public static class HarmonyPatches
    {
        private static ConfigWatcher Watcher;

        [HarmonyPatch(typeof(Ownable), "UpdateTint")]
        public static class Ownable_UpdateTint
        {
            public static void Postfix(Ownable __instance)
            {
                Color color = ColorHelper.GetComponentMaterialColor(__instance);
                bool owned = __instance.assignee != null;

                if (owned)
                {
                    KAnimControllerBase animBase = __instance.GetComponent<KAnimControllerBase>();
                    if (animBase != null)
                    {
                        animBase.TintColour = color;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(FilteredStorage), "OnFilterChanged")]
        public static class FilteredStorage_OnFilterChanged
        {
            public static void Postfix(KMonoBehaviour ___root, Tag[] tags)
            {
                Color color = ColorHelper.GetComponentMaterialColor(___root);
                bool active = tags != null && tags.Length != 0;

                if (active)
                {
                    KAnimControllerBase animBase = ___root.GetComponent<KAnimControllerBase>();
                    if (animBase != null)
                    {
                        animBase.TintColour = color;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(BlockTileRenderer), nameof(BlockTileRenderer.GetCellColour))]
        public static class BlockTileRenderer_GetCellColour
        {
            public static void Postfix(int cell, SimHashes element, BlockTileRenderer __instance, ref Color __result)
            {
                try
                {
                    if
                    (
                        State.ConfiguratorState.Enabled &&
                        ColorHelper.TileColors.Length > cell &&
                        ColorHelper.TileColors[cell].HasValue
                    )
                    {
                        __result *= ColorHelper.TileColors[cell].Value;
                    }
                }
                catch (Exception e)
                {
                    State.Logger.Log("EnterCell failed.");
                    State.Logger.Log(e);
                }
            }
        }

        [HarmonyPatch(typeof(Deconstructable), "OnCompleteWork")]
        public static class Deconstructable_OnCompleteWork_MatCol
        {
            public static void Postfix(Deconstructable __instance)
            {
                ResetCell(__instance.GetCell());
            }

            public static void ResetCell(int cellIndex)
            {
                if (ColorHelper.TileColors.Length > cellIndex)
                {
                    ColorHelper.TileColors[cellIndex] = null;
                }
            }
        }
        
        // TODO: still needs rework
        [HarmonyPatch(typeof(Global), "GenerateDefaultBindings")]
        public static class Global_GenerateDefaultBindings
        {
            public static void Postfix(ref BindingEntry[] __result)
            {

                if (State.ConfiguratorState.LogElementsData)
                {
                    State.Logger.Log("Element List:");
                    var values = Enum.GetNames(typeof(SimHashes));
                    Array.Sort(values);
                    string elementsLog = "";
                    foreach (var name in values)
                    {
                        elementsLog += Environment.NewLine+name;
                    }
                    State.Logger.Log(elementsLog);
                }

                try
                {
                    List<BindingEntry> bind = __result.ToList();
                    BindingEntry entry = new BindingEntry(
                                                          "Root",
                                                          GamepadButton.NumButtons,
                                                          KKeyCode.F6,
                                                          Modifier.Alt,
                                                          (Action)IDs.ToggleMaterialColorOverlayAction,
                                                          true,
                                                          true);
                    bind.Add(entry);
                    __result = bind.ToArray();
                }
                catch (Exception e)
                {
                    State.Logger.Log("Keybindings failed:\n" + e);
                    throw;
                }

            }
        }

        // TODO: still needs rework
        [HarmonyPatch(typeof(OverlayMenu), "InitializeToggles")]
        public static class OverlayMenu_InitializeToggles
        {
            // TODO: read from file instead
            public static void Postfix(OverlayMenu __instance)
            {
                FieldInfo overlayToggleInfosFI = AccessTools.Field(typeof(OverlayMenu), "overlayToggleInfos");
                var overlayToggleInfos = (List<KIconToggleMenu.ToggleInfo> )overlayToggleInfosFI.GetValue(__instance);

                                Type oti = AccessTools.Inner(typeof(OverlayMenu), "OverlayToggleInfo");
				
				ConstructorInfo ci =  oti.GetConstructor(new Type[] { typeof(string), typeof(string), typeof(HashedString), typeof(string), typeof(Action), typeof(string), typeof(string) });
				object ooti = ci.Invoke(new object[] {
						"Toggle MaterialColor",
						"overlay_materialcolor",
						IDs.MaterialColorOverlayHS,
						string.Empty,
						(Action)IDs.ToggleMaterialColorOverlayAction,
						"Toggles MaterialColor overlay",
						"MaterialColor"
				});
				((KIconToggleMenu.ToggleInfo)ooti).getSpriteCB = GetUISprite;

                overlayToggleInfos.Add((KIconToggleMenu.ToggleInfo)ooti);

				/*
				__result.Add(
                             new OverlayMenu.OverlayToggleInfo(
                                                               "Toggle MaterialColor",
                                                               "overlay_materialcolor",
                                                               (SimViewMode)IDs.ToggleMaterialColorOverlayID,
                                                               string.Empty,
                                                               (Action)IDs.ToggleMaterialColorOverlayAction,
                                                               "Toggles MaterialColor overlay",
                                                               "MaterialColor") {
                                                                                   getSpriteCB = () => GetUISprite()
                                                                                });
				*/
			}

            private static Sprite GetUISprite()
            {
                return FileManager.LoadSpriteFromFile(Paths.MaterialColorOverlayIconPath, 256, 256);
            }
        }

        [HarmonyPatch(typeof(OverlayMenu), "OnOverlayChanged")]
        public static class OverlayMenu_OnOverlayChanged_OverlayChangedEntry
        {
            public static void Prefix()
            {
                try
                {
                    var currentOverlayMode = OverlayScreen.Instance.GetMode();
                    if (OverlayModes.Power.ID.Equals(currentOverlayMode) ||
                        OverlayModes.GasConduits.ID.Equals(currentOverlayMode) ||
                        OverlayModes.LiquidConduits.ID.Equals(currentOverlayMode) ||
                        OverlayModes.Logic.ID.Equals(currentOverlayMode)
                        )
                    { 
                        Painter.Refresh();
                    }
                }
                catch (Exception e)
                {
                    State.Logger.Log("OverlayChangedEntry failed");
                    State.Logger.Log(e);
                }
            }
        }

		[HarmonyPatch(typeof(KeyDef), MethodType.Constructor)]
		[HarmonyPatch(new Type[] {typeof(KKeyCode), typeof(Modifier) })]
		public static class KeyDef_Constructor
		{
			[HarmonyPostfix]

			// ReSharper disable once InconsistentNaming
			public static void ExitKeyDef(KeyDef __instance)
			{
				__instance.mActionFlags = new bool[1000];
			}
		}

		[HarmonyPatch(typeof(KInputController), MethodType.Constructor)]
		[HarmonyPatch(new Type[] { typeof(bool) })]
		public static class KInputController_Constructor
		{
			[HarmonyPostfix]

			// ReSharper disable once InconsistentNaming
			public static void KInputControllerMod(ref bool[] ___mActionState)
			{
				___mActionState = new bool[1000];
			}
		}

		[HarmonyPatch(typeof(OverlayMenu), "OnToggleSelect")]
        public static class OverlayMenu_OnToggleSelect_MatCol
        {
            [HarmonyPrefix]

            // ReSharper disable once InconsistentNaming
            public static bool EnterToggle(OverlayMenu __instance, KIconToggleMenu.ToggleInfo toggle_info)
            {
                try
                {
					bool toggleMaterialColor = Traverse.Create(toggle_info).Field<HashedString>("simView").Value
                                            == IDs.MaterialColorOverlayHS;

                    if (!toggleMaterialColor)
                    {
                        return true;
                    }

                    State.ConfiguratorState.Enabled = !State.ConfiguratorState.Enabled;

                    Painter.Refresh();

                    return false;
                }
                catch (Exception e)
                {
                    State.Logger.Log("EnterToggle failed.");
                    State.Logger.Log(e);
                    return true;
                }
            }
        }

        /// <summary>
        /// Material + element color
        /// </summary>
        [HarmonyPatch(typeof(Game), "OnPrefabInit")]
        public static class Game_OnPrefabInit
        {
            [HarmonyPostfix]
            private static void Postfix()
            {
                try
                {
                    Components.BuildingCompletes.OnAdd += Painter.UpdateBuildingColor;

                    Watcher = new ConfigWatcher();
                    SimAndRenderScheduler.instance.render1000ms.Add(Watcher);
                    Painter.Refresh();
                }
                catch (Exception e)
                {
                    State.Logger.Log("MaterialColor init failed");
                    State.Logger.Log(e);
                }
            }
        }

        [HarmonyPatch(typeof(Game), "DestroyInstances")]
        public static class Game_DestroyInstances
        {
            public static void Postfix()
            {
                SimAndRenderScheduler.instance.render1000ms.Remove(Watcher);
                Watcher.Dispose();
                Watcher = null;
            }
        }
    }
}