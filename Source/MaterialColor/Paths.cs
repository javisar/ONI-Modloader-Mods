namespace MaterialColor
{
    using System.IO;

    // TODO: refactor, split
    public static class Paths
    {
        // TODO: fix filename
        public const string MaterialColorLogFileName = "MaterialCoreLog.txt";

        public const string MaterialColorOverlayIconFileName = "overlay_materialColor.png";

        public const string MaterialColorStateFileName = "MaterialColorState.json";

        public const string ModsDirectory = "Mods";

        public static readonly string MaterialMainPath = ModsDirectory + Path.DirectorySeparatorChar + "MaterialColor";

        public static readonly string MaterialConfigPath = MaterialMainPath + Path.DirectorySeparatorChar + "Config";

        public static readonly string ElementColorInfosDirectory =
        MaterialConfigPath + Path.DirectorySeparatorChar + "ElementColorInfos";

        public static readonly string MaterialColorStatePath =
        MaterialConfigPath + Path.DirectorySeparatorChar + MaterialColorStateFileName;

        public static readonly string SpritesPath = MaterialMainPath + Path.DirectorySeparatorChar + "Sprites";

        public static readonly string MaterialColorOverlayIconPath =
        SpritesPath + Path.DirectorySeparatorChar + MaterialColorOverlayIconFileName;
    }
}