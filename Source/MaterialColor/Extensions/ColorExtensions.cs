using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MaterialColor.Extensions
{
    public static class ColorExtensions
    {
        public static Color ToTileColor(this Color color, bool useColorAsOffset)
        {
            if (useColorAsOffset)
            {
                color = color.ForEachColorValue
                (
                    colorValue => colorValue >= 0
                        ? colorValue + 1
                        : Mathf.Abs(colorValue),
                    false
                );
            }

            color.a = 1;

            return color;
        }

        public static Color ForEachColorValue(this Color color, Func<float, float> action, bool modifyAlpha)
        {
            int count = modifyAlpha ? 4 : 3;
            for (int i = 0; i < count; i++)
            {
                color[i] = action.Invoke(color[i]);
            }
            return color;
        }
    }
}
