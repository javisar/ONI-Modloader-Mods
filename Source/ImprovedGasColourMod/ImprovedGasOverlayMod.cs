using Harmony;
using MaterialColor.Extensions;
using UnityEngine;

namespace ImprovedGasColourMod
{
    public static class HarmonyPatches
    {
        private static readonly Color NotGasColor = new Color(0.6f, 0.6f, 0.6f);

        [HarmonyPatch(typeof(SimDebugView), "GetOxygenMapColour")]
        public static class ImprovedGasOverlayMod
        {
            public const float EarPopFloat = 5;

            // TODO: read from config (major fps drop when enabled)
            private static bool AdvancedDebugging => false;

            public static bool Prefix(int cell, ref Color __result)
            {
                //  ModSettings settings = ONI_Common.ModdyMcModscreen
                float maxMass = ONI_Common.State.ConfiguratorState.GasPressureEnd;

                Element element = Grid.Element[cell];

                if (!element.IsGas)
                {
                    __result = NotGasColor;
                    return false;
                }

                float mass = Grid.Mass[cell];
                SimHashes elementID = element.id;
                Color primaryColor = GetCellOverlayColor(cell);
                float pressureFraction = GetPressureFraction(mass, maxMass);

                __result = GetGasColor(elementID, primaryColor, pressureFraction, mass);

                return false;
            }

            private static ColorHSV GetGasColor(SimHashes elementID, Color primaryColor, float pressureFraction, float mass)
            {
                ColorHSV colorHSV = primaryColor.ToHSV();

                colorHSV = ScaleColorToPressure(colorHSV, pressureFraction, elementID);

                if (/*config.showEarPopMarker*/ true)
                {
                    colorHSV = MarkEarDrumPopPressure(colorHSV, mass);
                }

                if (AdvancedDebugging)
                {
                    colorHSV.CheckAndLogOverflow(elementID, pressureFraction);
                }

                colorHSV = colorHSV.Clamp();

                return colorHSV;
            }

            private static ColorHSV ScaleColorToPressure(ColorHSV color, float fraction, SimHashes elementID)
            {
                if (elementID == SimHashes.CarbonDioxide)
                {
                    color.V *= (1 - fraction) * 2;
                }
                else
                {
                    color.S *= fraction * 1.25f;
                    color.V -= (1 - fraction) / 2;
                }

                return color;
            }

            public static Color GetCellOverlayColor(int cellIndex)
            {
                Element element = Grid.Element[cellIndex];
                Substance substance = element.substance;

                Color32 overlayColor = substance.overlayColour;

                overlayColor.a = byte.MaxValue;

                return overlayColor;
            }

            private static float GetPressureFraction(float mass, float maxMass)
            {
                float minFraction = ONI_Common.State.ConfiguratorState.MinimumGasColorIntensity;

                float fraction = mass / maxMass;

                fraction = Mathf.Lerp(minFraction, 1, fraction);

                return fraction;
            }

            /// <summary>
            /// Add flat value to color hue when pressure reaches EarPopFloat
            /// </summary>
            private static ColorHSV MarkEarDrumPopPressure(ColorHSV color, float mass)
            {
                if (mass > EarPopFloat)
                {
                    color.H += 0.1f;
                }

                return color;
            }
        }
    }
}