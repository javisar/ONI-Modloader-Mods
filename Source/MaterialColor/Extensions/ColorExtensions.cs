using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MaterialColor.Extensions
{
    public static class ColorExtensions
    {
        public static Color ToTileColor(this Color color)
        {
            if (color.a == 0)
            {
                color = color.ForEachColorValue(colorValue => colorValue + 1);
            }

            color.a = 1;

            return color;
        }

        public static Color ForEachColorValue(this Color color, Func<float, float> action)
        {
            for (int i = 0; i < 3; i++)
            {
                color[i] = action.Invoke(color[i]);
            }
            return color;
        }
    }
}
