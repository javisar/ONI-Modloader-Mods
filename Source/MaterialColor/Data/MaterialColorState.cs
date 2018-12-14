namespace MaterialColor.Data
{
    public class MaterialColorState
    {
        public bool Enabled { get; set; } = true;

        public bool LogElementsData { get; set; } = true;

        public ColorMode ColorMode { get; set; } = ColorMode.Json;

        public bool LegacyTileColorHandling { get; set; } = false;

        public bool ShowBuildingsAsWhite { get; set; }

        public bool ShowMissingElementColorInfos { get; set; }

        public bool ShowMissingTypeColorOffsets { get; set; }
    }
}