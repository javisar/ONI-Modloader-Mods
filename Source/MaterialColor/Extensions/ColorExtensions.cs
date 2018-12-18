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
    }
}
