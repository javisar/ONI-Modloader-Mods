using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;

namespace AmphibiousMod
{
    [HarmonyPatch(typeof(ModifierSet), "LoadTraits")]
    internal static class AmphibiousMod_ModifierSet_LoadTraits
    {
        private static MethodInfo CreateNamedTraitM = AccessTools.Method(typeof(TUNING.TRAITS), "CreateNamedTrait");

        private static void Prefix()
        {
            //Debug.Log(" === AmphibiousMod_ModifierSet_LoadTraits === ");

            TUNING.DUPLICANTSTATS.GOODTRAITS.Add(
                   new TUNING.DUPLICANTSTATS.TraitVal
                   {
                       id = "Amphibious",
                       statBonus = -TUNING.DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
                       probability = 2.5f
                   }
               );

            TUNING.TRAITS.TRAIT_CREATORS.Add(
                    (System.Action)CreateNamedTraitM.Invoke(null,
                            new object[] { "Amphibious", "Amphibious", "This duplicant breaths under water.", true}     // true: positive trait
                        )
                );
        }
    }


	[HarmonyPatch(typeof(GasBreatherFromWorldProvider), "ConsumeGas")]
	internal static class AmphibiousMod_GasBreatherFromWorldProvider_ConsumeGas
	{

		private static bool Prefix(GasBreatherFromWorldProvider __instance, ref bool __result, ref OxygenBreather oxygen_breather, ref float gas_consumed)
		{

			//Debug.Log(" === AmphibiousMod_GasBreatherFromWorldProvider_ConsumeGas === ");
			Klei.AI.Traits traits = oxygen_breather.gameObject.GetComponent<Klei.AI.Traits>();
			bool flag = traits.GetTraitIds().Contains("Amphibious");
			if (!flag) return true;

			SimHashes getBreathableElement = oxygen_breather.GetBreathableElement;
			if (getBreathableElement == SimHashes.Vacuum)
			{
				__result = false;
				return false;
			}
			//HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(OnSimConsumeCallback, __instance, "GasBreatherFromWorldProvider");
			//SimMessages.ConsumeMass(oxygen_breather.mouthCell, getBreathableElement, gas_consumed/5f, 3, handle.index);
			SimMessages.ConsumeMass(oxygen_breather.mouthCell, getBreathableElement, gas_consumed / 5f, 1);
			__result = true;
			return false;
		}
	}


	[HarmonyPatch(typeof(OxygenBreather), "GetBreathableElementAtCell")]
    internal static class AmphibiousMod_OxygenBreather_GetBreathableElementAtCell
    {
        
        private static MethodInfo GetMouthCellAtCellM = AccessTools.Method(typeof(OxygenBreather), "GetMouthCellAtCell");
       
        private static bool Prefix(OxygenBreather __instance, ref SimHashes __result, ref int cell, ref CellOffset[] offsets)
        {
            //Debug.Log(" === AmphibiousMod_OxygenBreather_GetBreathableElementAtCell === ");

            Klei.AI.Traits traits = __instance.gameObject.GetComponent<Klei.AI.Traits>();
            bool flag = traits.GetTraitIds().Contains("Amphibious");
            //Debug.Log(" === AmphibiousMod_OxygenBreather_GetBreathableElementAtCell === " + flag);
            if (!flag) return true;

 			if (offsets == null)
            {
                offsets = __instance.breathableCells;
            }

            //this.GetMouthCellAtCell(cell, offsets);
            int mouthCellAtCell = (int)GetMouthCellAtCellM.Invoke(__instance, new object[] { cell, offsets });            

            if (!Grid.IsValidCell(mouthCellAtCell))
            {
                __result = SimHashes.Vacuum;
                return false;
            }
            Element element = Grid.Element[mouthCellAtCell];

			__result = ((!element.IsGas || !element.HasTag(GameTags.Breathable)) && (!element.IsLiquid || !element.HasTag(GameTags.AnyWater)) || !(Grid.Mass[mouthCellAtCell] > __instance.noOxygenThreshold)) ? SimHashes.Vacuum : element.id;
			return false;
        }
        
    }   
}
