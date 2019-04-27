using Harmony;
using System;

namespace TimeLapser
{
    [HarmonyPatch(typeof(CameraController), "OnKeyDown", new Type[] { typeof(KButtonEvent) })]
    internal class InputBlockerPatch
    {
        public static bool blockInput = false;
        private static bool Prefix(CameraController __instance, KButtonEvent e)
        {
            return !blockInput;
        }
    }
}