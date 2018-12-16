namespace MaterialColor.Helpers
{
    using Extensions;
    using System;
    using UnityEngine;

    public static class ColorHelper
    {
        public static readonly Color DefaultColor = new Color(1, 1, 1, 1);
        public static readonly Color InvalidColor = new Color(1, 0, 1, 1);

        public static readonly Color DefaultTileColor = HarmonyPatches.ToTileColor(DefaultColor);
        public static readonly Color InvalidTileColor = HarmonyPatches.ToTileColor(InvalidColor);

        public static Color?[] TileColors;

        public static Color GetCellColorDebug(int cellIndex)
        {
            Element   element   = Grid.Element[cellIndex];
            Substance substance = element.substance;

            Color32 debugColor = substance.debugColour;

            debugColor.a = byte.MaxValue;

            return debugColor;
        }

        public static Color GetCellColorJson(int cellIndex)
        {
            if (Grid.IsValidCell(cellIndex))
            {
                Element element;
                if (MaterialHelper.CellIndexToElement(cellIndex, out element))
                {
                    Color materialColor;
                    if (element.id.ToMaterialColor(out materialColor))
                    {
                        return materialColor;
                    }
                    else
                    {
                        return element.substance.overlayColour;
                    }
                }
            }
            return ColorHelper.InvalidTileColor;
        }

        // TODO: MOVE
        private static void BreakdownGridObjectsComponents(int cellIndex)
        {
            for (int i = 0; i <= 20; i++)
            {
                Debug.Log("Starting object from grid component breakdown, index: " + cellIndex);

                try
                {
                    Component[] comps = Grid.Objects[cellIndex, i].GetComponents<Component>();

                    foreach (Component comp in comps)
                    {
                        Debug.Log($"Object Layer: {i}, Name: {comp.name}, Type: {comp.GetType()}");
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Debug.Log($"Cell Index: {cellIndex}, Layer: {i}");
                    Debug.Log(e);
                }

                // catch { }
            }
        }
    }
}