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
    }
}
