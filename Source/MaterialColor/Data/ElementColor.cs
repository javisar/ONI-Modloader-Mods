using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MaterialColor.Data
{
    public class ElementColor
    {
        public float Red { get; set; } = 0;
        public float Green { get; set; } = 0;
        public float Blue { get; set; } = 0;

        public bool DarkMode { get; set; } = false;

        public ElementColor ForEachColorValue(Func<float, float> action)
        {
            return new ElementColor
            {
                Red = action.Invoke(this.Red),
                Green = action.Invoke(this.Green),
                Blue = action.Invoke(this.Blue)
            };
        }

        public Color ToColor()
        {
            return new Color
            {
                r = this.Red,
                g = this.Green,
                b = this.Blue,
                a = this.DarkMode ? 1 : 0
            };
        }
    }
}
