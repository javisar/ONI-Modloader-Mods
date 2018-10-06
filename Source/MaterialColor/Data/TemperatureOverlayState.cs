namespace MaterialColor.Data
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using UnityEngine;

    [SuppressMessage("ReSharper", "StyleCop.SA1600")]
    public class TemperatureOverlayState
    {


        public bool CustomRangesEnabled { get; set; } = true;

        public bool LogThresholds { get; set; } = false;

        public List<float> Temperatures =
        new List<float>()
        {
            0,                  // AbsoluteZero,
            273.15f,            // Cold,
            283.15f,            // Chilled,
            293.15f,            // Temperate,
            303.15f,            // Warm,
            310.15f,            // Hot,
            373.15f,            // Scorching,
            2073.15f            // Molten
        };


        public List<Color32> Colors =
        new List<Color32>()
        {
        new Color32(25,  102, 255, 192),    //AbsoluteZeroColor,
        new Color32(25,  179, 255, 192),    //ColdColor,
        new Color32(25,  255, 255, 192),    //ChilledColor
        new Color32(25, 255, 25, 192),      //TemperateColor,
        new Color32(236, 255, 25,  192),    //WarmColor,
        new Color32(255, 163, 25,  192),    //HotColor,
        new Color32(255, 83,  25,  192),    //ScorchingColor,
        new Color32(255, 25,  25,  192)     //MoltenColor
        };        

    }
}