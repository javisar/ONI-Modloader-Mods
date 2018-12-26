namespace ImprovedTemperatureOverlay
{
    using System.IO;

    public static class Paths
    {
       
        public const string ModsDirectory = "Mods";
		
        public const string TemperatureStateFileName = "ImprovedTemperatureOverlayState.json";
        
        public static readonly string MaterialMainPath = ModsDirectory + Path.DirectorySeparatorChar + "ImprovedTemperatureOverlay";

        public static readonly string MaterialConfigPath = MaterialMainPath + Path.DirectorySeparatorChar + "Config";

		public static readonly string TemperatureStatePath = MaterialConfigPath + Path.DirectorySeparatorChar + TemperatureStateFileName;

		public const string MaterialColorLogFileName = "ImprovedTemperatureOverlay.txt";
	}
}