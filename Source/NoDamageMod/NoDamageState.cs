using System.Collections.Generic;
using ONI_Common.Json;

namespace NoDamageMod
{
    public class NoDamageState
    {
        public bool Enabled { get; set; } = true;

        public bool DisableAllDamage { get; set; } = true;

        public bool NoBuildingDamage { get; set; } = true;

        public bool NoCircuitOverload { get; set; } = true;
        public bool NoBuildingOverheat { get; set; } = true;
        public bool NoConduitContentsBoiling { get; set; } = true;
        public bool NoConduitContentsFrozen { get; set; } = true;

        public static BaseStateManager<NoDamageState> StateManager
                                = new BaseStateManager<NoDamageState>("NoDamage");
    }
}