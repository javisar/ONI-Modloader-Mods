using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MaterialColor.Helpers
{
    public class DebugHelper
    {
        public static void LogComponents(Component target)
        {
            Component[] comps = target.GetComponents<Component>();

            foreach (Component comp in comps)
            {
                Debug.Log($"BREAK: Name: {comp.name}, Type: {comp.GetType()}");
            }
        }

        public static void BreakdownGridObjectsComponents(int cellIndex)
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
            }
        }
    }
}
