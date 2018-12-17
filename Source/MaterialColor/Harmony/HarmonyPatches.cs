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

    public static class HarmonyPatches
    {
        private static bool _configuratorStateChanged;
        private static bool _elementColorInfosChanged;

        public static void OnLoad()
        {
            SubscribeToFileChangeNotifier();
        }

        public static void RefreshMaterialColor()
        {
            UpdateBuildingsColors();
            RebuildAllTiles();
        }

        // TODO: move
        /// <summary>
        /// Tries to get material color for given component, if not possible fallsback to to substance.overlayColour, then to ColorHelper.DefaultColor
        /// </summary>
        /// <param name="outColor">on true outputs color offset, otherwise fallsback to substance.overlayColour, then to ColorHelper.DefaultColor</param>
        public static bool ToMaterialColor(Component component, out Color outColor)
        {
            if (State.ConfiguratorState.Enabled)
            {
                PrimaryElement primaryElement = component.GetComponent<PrimaryElement>();

                if (primaryElement != null)
                {
                    SimHashes material = primaryElement.ElementID;

                    bool materialColorResult = material.ToMaterialColor(out outColor);

                    if (!materialColorResult)
                    {
                        outColor = primaryElement.Element.substance.overlayColour;
                        if (State.ConfiguratorState.ShowMissingElementColorInfos)
                        {
                            Debug.Log($"Missing color for material: {material}, while coloring building: {component.GetComponent<BuildingComplete>()}");
                        }
                    }

                    return materialColorResult;
                }
            }

            outColor = ColorHelper.DefaultColorOffset;
            return false;
        }

        public static void UpdateBuildingColor(BuildingComplete building)
        {
            string buildingName = building.name.Replace("Complete", string.Empty);

            Color color;
            bool colorAsOffset = ToMaterialColor(building, out color);

            if (State.TileNames.Contains(buildingName))
            {
                try
                {
                    if (ColorHelper.TileColors == null)
                    {
                        ColorHelper.TileColors = new Color?[Grid.CellCount];
                    }

                    ColorHelper.TileColors[Grid.PosToCell(building.gameObject)] = ToTileColor(color, colorAsOffset);

                    return;
                }
                catch (Exception e)
                {
                    State.Logger.Log("Error while getting cell color");
                    State.Logger.Log(e);
                }
            }

            IUserControlledCapacity userControlledCapacity = building.GetComponent<IUserControlledCapacity>();
            Ownable ownable = building.GetComponent<Ownable>();

            if (userControlledCapacity == null && ownable == null)
            {
                KAnimControllerBase kAnimControllerBase = building.GetComponent<KAnimControllerBase>();

                if (kAnimControllerBase != null)
                {
                    SetTintColour(kAnimControllerBase, color);
                }
                else
                {
                    Debug.Log($"MaterialColor: Can't find KAnimControllerBase component in <{buildingName}> and its not a registered tile.");
                }
            }
        }

        // TODO: move, to extension?
        public static Color ToTileColor(Color color, bool useColorAsOffset)
        {
            if (useColorAsOffset)
            {
                return new Color
                (
                    color.r >= 0
                        ? 1 + color.r
                        : Mathf.Abs(color.r),
                    color.g >= 0
                        ? 1 + color.g
                        : Mathf.Abs(color.g),
                    color.b >= 0
                        ? 1 + color.b
                        : Mathf.Abs(color.b),
                    1
                );
            }
            else
            {
                return new Color
                (
                    color.r,
                    color.g,
                    color.b,
                    1
                );
            }
        }

        private static void SetTintColour(KAnimControllerBase kAnimControllerBase, Color color)
        {
            KBatchedAnimInstanceData batchInstanceData = Traverse.Create(kAnimControllerBase).Field("batchInstanceData").GetValue<KBatchedAnimInstanceData>();
            if (batchInstanceData.SetTintColour(color))
            {
                kAnimControllerBase.SetDirty();

                Traverse.Create(kAnimControllerBase).Method("SuspendUpdates", false).GetValue();

                kAnimControllerBase.OnTintChanged?.Invoke(color);
            }
        }

        private static void OnBuildingsCompletesAdd(BuildingComplete building) => UpdateBuildingColor(building);

        private static void OnElementColorsInfosChanged(object sender, FileSystemEventArgs e)
        {
            bool reloadColorInfosResult = false;

            try
            {
                reloadColorInfosResult = State.TryReloadElementColorInfos();
            }
            catch (Exception ex)
            {
                State.Logger.Log("ReloadElementColorInfos failed.");
                State.Logger.Log(ex);
            }

            if (reloadColorInfosResult)
            {
                _elementColorInfosChanged = true;

                const string message = "Element color infos changed.";

                State.Logger.Log(message);
                Debug.LogError(message);
            }
            else
            {
                State.Logger.Log("Reload element color infos failed");
            }
        }

        private static void OnMaterialStateChanged(object sender, FileSystemEventArgs e)
        {
            if (!State.TryReloadConfiguratorState())
            {
                return;
            }

            _configuratorStateChanged = true;

            const string message = "Configurator state changed.";

            State.Logger.Log(message);
            Debug.LogError(message);
        }

        private static void RebuildAllTiles()
        {
            for (int i = 0; i < Grid.CellCount; i++)
            {
                World.Instance.blockTileRenderer.Rebuild(ObjectLayer.FoundationTile, i);
            }

            State.Logger.Log("All tiles rebuilt.");
        }

        private static void SubscribeToFileChangeNotifier()
        {
            const string jsonFilter = "*.json";

            try
            {
                FileChangeNotifier.StartFileWatch
                (
                    jsonFilter,
                    Paths.ElementColorInfosDirectory,
                    OnElementColorsInfosChanged
                );

                FileChangeNotifier.StartFileWatch
                (
                    Paths.MaterialColorStateFileName,
                    Paths.MaterialConfigPath,
                    OnMaterialStateChanged
                );
            }
            catch (Exception e)
            {
                State.Logger.Log("SubscribeToFileChangeNotifier failed");
                State.Logger.Log(e);
            }
        }

        private static void UpdateBuildingsColors()
        {
            State.Logger.Log($"Trying to update {Components.BuildingCompletes.Count} buildings.");

            try
            {
                foreach (BuildingComplete building in Components.BuildingCompletes.Items)
                {
                    OnBuildingsCompletesAdd(building);
                }

                State.Logger.Log("Buildings updated successfully.");
            }
            catch (Exception e)
            {
                State.Logger.Log("Buildings colors update failed.");
                State.Logger.Log(e);
            }
        }

        [HarmonyPatch(typeof(Ownable), "UpdateTint")]
        public static class Ownable_UpdateTint
        {
            public static void Postfix(Ownable __instance)
            {
                Color tint;
                bool colorAsOffset = HarmonyPatches.ToMaterialColor(__instance, out tint);
                bool owned = __instance.assignee != null;

                if (owned)
                {
                    KAnimControllerBase animBase = __instance.GetComponent<KAnimControllerBase>();
                    if (animBase != null && animBase.HasBatchInstanceData) // TODO: is second check needed?
                    {
                        SetTintColour(animBase, tint);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(FilteredStorage), "OnFilterChanged")]
        public static class FilteredStorage_OnFilterChanged
        {
            public static void Postfix(KMonoBehaviour ___root, Tag[] tags)
            {
                Color tint;
                bool colorAsOffset = HarmonyPatches.ToMaterialColor(___root, out tint);
                bool active = tags != null && tags.Length != 0;

                if (active)
                {
                    KAnimControllerBase animBase = ___root.GetComponent<KAnimControllerBase>();
                    if (animBase != null && animBase.HasBatchInstanceData)
                    {
                        SetTintColour(animBase, tint);
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

        [HarmonyPatch(typeof(Game), "Update")]
        public static class Game_Update_EnterEveryUpdate
        {
            public static void Prefix()
            {
                try
                {
                    if (_elementColorInfosChanged || _configuratorStateChanged)
                    {
                        RefreshMaterialColor();
                        _elementColorInfosChanged = _configuratorStateChanged = false;
                    }
                }
                catch (Exception e)
                {
                    State.Logger.Log("EnterEveryUpdate failed.");
                    State.Logger.Log(e);
                }
            }
        }

        [HarmonyPatch(typeof(Game), "Update")]
        public static class Game_Update_EnterEveryUpdate_CoreUpdateQueueManager
        {
            public static void Prefix()
            {
                UpdateQueueManager.OnGameUpdate();
            }
        }

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

        [HarmonyPatch(typeof(OverlayMenu), "InitializeToggles")]
        public static class OverlayMenu_InitializeToggles
        {
            // TODO: read from file instead
            public static void Postfix(OverlayMenu __instance, ref List<KIconToggleMenu.ToggleInfo> __result)
            {
				Type oti = AccessTools.Inner(typeof(OverlayMenu), "OverlayToggleInfo");
				
				ConstructorInfo ci =  oti.GetConstructor(new Type[] { typeof(string), typeof(string), typeof(SimViewMode), typeof(string), typeof(Action), typeof(string), typeof(string) });
				object ooti = ci.Invoke(new object[] {
						"Toggle MaterialColor",
						"overlay_materialcolor",
						(SimViewMode)IDs.ToggleMaterialColorOverlayID,
						string.Empty,
						(Action)IDs.ToggleMaterialColorOverlayAction,
						"Toggles MaterialColor overlay",
						"MaterialColor"
				});
				((KIconToggleMenu.ToggleInfo)ooti).getSpriteCB = GetUISprite;

				__result.Add((KIconToggleMenu.ToggleInfo)ooti);

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
                    switch (OverlayScreen.Instance.GetMode())
                    {
                        case SimViewMode.PowerMap:
                        case SimViewMode.GasVentMap:
                        case SimViewMode.LiquidVentMap:
                        case SimViewMode.Logic:
                            RefreshMaterialColor();
                            break;
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
					bool toggleMaterialColor = Traverse.Create(toggle_info).Field<SimViewMode>("simView").Value
											== (SimViewMode)IDs.ToggleMaterialColorOverlayID;

                    if (!toggleMaterialColor)
                    {
                        return true;
                    }

                    State.ConfiguratorState.Enabled = !State.ConfiguratorState.Enabled;

                    RefreshMaterialColor();

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
        public static class SimDebugView_OnPrefabInit_EnterOnce
        {
            [HarmonyPostfix]
            private static void Postfix()
            {
                try
                {
                    Components.BuildingCompletes.OnAdd += OnBuildingsCompletesAdd;
                    _elementColorInfosChanged = _configuratorStateChanged = true;
                }
                catch (Exception e)
                {
                    string message = "Injection failed\n" + e.Message + '\n';

                    State.Logger.Log(message);
                    State.Logger.Log(e);

                    Debug.LogError(message);
                }
            }
        }
    }
}