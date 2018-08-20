
using MaterialColor.Extensions;
using ONI_Common.Data;

namespace MaterialColor.Data
{
    public class ElementColorInfo
    {
        public ElementColorInfo(Color32Multiplier multiplier, float brightness = 1)
        {
            this.ColorMultiplier = multiplier;
            this.Brightness      = brightness;
        }

        public float Brightness { get; set; }

        public Color32Multiplier ColorMultiplier { get; set; }
    }
}