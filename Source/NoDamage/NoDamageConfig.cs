
namespace NoDamageMod
{
	public class NoDamageConfig
	{
		public bool Enabled { get; set; } = true;

		public bool DisableAllDamage { get; set; } = true;

		public bool NoBuildingDamage { get; set; } = true;

		public bool NoCircuitOverload { get; set; } = true;
		public bool NoBuildingOverheat { get; set; } = true;
		public bool NoConduitContentsBoiling { get; set; } = true;
		public bool NoConduitContentsFrozen { get; set; } = true;




		// Load Config		
		public static NoDamageConfig Instance
		{
			get
			{
				return ConfigUtils<NoDamageConfig>.LoadConfig("Config", "NoDamageConfig.json");
			}
		}
	}
}