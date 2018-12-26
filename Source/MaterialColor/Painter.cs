using Harmony;
using MaterialColor.Extensions;
using MaterialColor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MaterialColor
{
    /// <summary>
    /// Provides main mod functionality - applying colors to buildings and tiles.
    /// </summary>
    public static class Painter
    {
        private static readonly List<Type> _storageTypes = new List<Type>()
        {
            typeof(RationBox),
            typeof(Refrigerator),
            typeof(SolidConduitInbox),
            typeof(StorageLocker),
            typeof(TinkerStation)
        };

        public static void Refresh()
        {
            UpdateBuildingsColors();
            RebuildAllTiles();
        }

        public static void UpdateBuildingColor(BuildingComplete building)
        {
            string buildingName = building.name.Replace("Complete", string.Empty);
            Color color = ColorHelper.GetComponentMaterialColor(building);

            if (State.TileNames.Contains(buildingName))
            {
                ApplyColorToTile(building, color);
                return;
            }

            try
            {
                if (!State.TypeFilter.Check(buildingName))
                {
                    color = ColorHelper.DefaultColor;
                }
            }
            catch (Exception e)
            {
                State.Logger.Log("Error while filtering buildings");
                State.Logger.Log(e);
            }

            ApplyColorToBuilding(building, color);
        }

        private static FilteredStorage ExtractFilteredStorage(Component building)
        {
            foreach (Type storageType in _storageTypes)
            {
                Component comp = building.GetComponent(storageType);

                if (comp != null)
                {
                    return Traverse.Create(comp).Field<FilteredStorage>("filteredStorage").Value;
                }
            }
            return null;
        }

        private static void ApplyColorToBuilding(BuildingComplete building, Color color)
        {
            TreeFilterable treeFilterable;
            Ownable ownable;
            KAnimControllerBase kAnimBase;

            if ((ownable = building.GetComponent<Ownable>()) != null)
            {
                Traverse.Create(ownable).Field("ownedTint").SetValue(color);
                Traverse.Create(ownable).Method("UpdateTint").GetValue();
            }
            else if ((treeFilterable = building.GetComponent<TreeFilterable>()) != null)
            {
                FilteredStorage filteredStorage = ExtractFilteredStorage(treeFilterable);

                if (filteredStorage != null)
                {
                    filteredStorage.filterTint = color;
                    filteredStorage.FilterChanged();
                }
            }
            else if ((kAnimBase = building.GetComponent<KAnimControllerBase>()) != null)
            {
                kAnimBase.TintColour = color;
            }
            else
            {
                Debug.Log($"MaterialColor: Invalid building <{building}> and its not a registered tile.");
            }
        }

        private static void ApplyColorToTile(BuildingComplete building, Color color)
        {
            try
            {
                if (ColorHelper.TileColors == null)
                {
                    ColorHelper.TileColors = new Color?[Grid.CellCount];
                }

                ColorHelper.TileColors[Grid.PosToCell(building.gameObject)] = color.ToTileColor();

                return;
            }
            catch (Exception e)
            {
                State.Logger.Log("Error while getting cell color");
                State.Logger.Log(e);
            }
        }

        private static void RebuildAllTiles()
        {
            for (int i = 0; i < Grid.CellCount; i++)
            {
                World.Instance.blockTileRenderer.Rebuild(ObjectLayer.FoundationTile, i);
            }

            State.Logger.Log("All tiles rebuilt.");
        }

        private static void UpdateBuildingsColors()
        {
            State.Logger.Log($"Trying to update {Components.BuildingCompletes.Count} buildings.");

            try
            {
                foreach (BuildingComplete building in Components.BuildingCompletes.Items)
                {
                    UpdateBuildingColor(building);
                }

                State.Logger.Log("Buildings updated successfully.");
            }
            catch (Exception e)
            {
                State.Logger.Log("Buildings colors update failed.");
                State.Logger.Log(e);
            }
        }
    }
}
