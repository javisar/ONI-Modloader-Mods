using MaterialColor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ImprovedGasColourMod
{
    public static class HSVExtensions
    {
        public static void CheckAndLogOverflow(this ColorHSV color, SimHashes elementID, float fraction)
        {
            bool error = false;
            string message = string.Empty;

            if (color.H < 0 || color.H > 1)
            {
                error = true;
                message += $"hue ({color.H}) ";
            }

            if (color.S < 0 || color.S > 1)
            {
                error = true;
                message += $"saturation ({color.S}) ";
            }

            if (color.V < 0 || color.V > 1)
            {
                error = true;
                message += $"value ({color.V}) ";
            }

            if (error)
            {
                Debug.LogError($"Gas color {message.Trim()} under/overflow for <{elementID}> at intensity [{fraction}]");
            }
        }

        public static ColorHSV Clamp(this ColorHSV color)
        {
            color.H = Mathf.Clamp01(color.H);
            color.S = Mathf.Clamp01(color.S);
            color.V = Mathf.Clamp01(color.V);

            return color;
        }

    }
}
