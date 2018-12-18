namespace MaterialColor.Data
{
    public class MaterialColorState
    {
        public bool Enabled { get; set; } = true;

        public bool LogElementsData { get; set; } = true;

        public bool ShowMissingElementColorInfos { get; set; }

        public float AdditionalBrightness { get; set; } = 0;

        public float IntensityFactor { get; set; } = 1;
    }
}