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
                       statBonus = -TUNING.DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS*2,
                       probability = TUNING.DUPLICANTSTATS.PROBABILITY_MINISCULE*2f
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

		/*
		private static MethodInfo OnSimConsumeM = AccessTools.Method(typeof(GasBreatherFromWorldProvider), "OnSimConsume");

		private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
		{
			//((GasBreatherFromWorldProvider)data).OnSimConsume(mass_cb_info);
			//mi.Invoke((GasBreatherFromWorldProvider) data, new object[] { mass_cb_info });			
			mass_cb_info.mass /= 5f;
			OnSimConsume(((GasBreatherFromWorldProvider)data), mass_cb_info);
			
		}

		private static FieldInfo oxygenBreatherF = AccessTools.Field(typeof(GasBreatherFromWorldProvider), "oxygenBreather");

		private static void OnSimConsume(GasBreatherFromWorldProvider data, Sim.MassConsumedCallback mass_cb_info)
		{
			OxygenBreather oxygenBreather = (OxygenBreather) oxygenBreatherF.GetValue(data);
			if (!(oxygenBreather == null) && !oxygenBreather.GetComponent<KPrefabID>().HasTag(GameTags.Dead))
			{
				//Game.Instance.accumulators.Accumulate(oxygenBreather.O2Accumulator, mass_cb_info.mass);
				float value = 0f - mass_cb_info.mass;
				//ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, value, oxygenBreather.GetProperName());
				oxygenBreather.Consume(mass_cb_info);
			}
		}
		*/
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

        /*
       private static MethodInfo _GetMouthCellAtCellM = AccessTools.Method(typeof(OxygenBreather), "GetMouthCellAtCell");
       private delegate int GetMouthCellAtCell_Delegate(int cell, CellOffset[] offsets);
       private static GetMouthCellAtCell_Delegate _GetMouthCellAtCell =
           (GetMouthCellAtCell_Delegate)Delegate.CreateDelegate(typeof(GetMouthCellAtCell_Delegate), _GetMouthCellAtCellM);
       */

        /*
        private static int GetMouthCellAtCell(OxygenBreather __instance, int cell, CellOffset[] offsets)
        {
            float num = 0f;
            int result = cell;
            foreach (CellOffset offset in offsets)
            {
                int num2 = Grid.OffsetCell(cell, offset);
                float oxygenPressure = GetOxygenPressure(num2);
                if (oxygenPressure > num && oxygenPressure > __instance.noOxygenThreshold)
                {
                    num = oxygenPressure;
                    result = num2;
                }
            }
            return result;
        }

        private static float GetOxygenPressure(int cell)
        {
            if (Grid.IsValidCell(cell))
            {
                Element element = Grid.Element[cell];
                if (element.HasTag(GameTags.Breathable))
                {
                    return Grid.Mass[cell];
                }
            }
            return 0f;
        }
        */
    }
    /*
    [HarmonyPatch(typeof(TUNING.DUPLICANTSTATS), MethodType.Constructor)]
    internal static class AmphibiousMod_DUPLICANTSTATS_Constructor
    {
        //private static MethodInfo mi = AccessTools.Method(typeof(TUNING.TRAITS), "CreateNamedTrait");

        private static void Postfix()
        {
            Debug.Log(" === AmphibiousMod_DUPLICANTSTATS_Constructor === ");

           
        }
    }
    
    [HarmonyPatch(typeof(GasBreatherFromWorldProvider), "ConsumeGas")]
    internal static class AmphibiousMod_GasBreatherFromWorldProvider_ConsumeGas
    {

        //private static MethodInfo mi = AccessTools.Method(typeof(OxygenBreather), "GetMouthCellAtCell");
        
        private static bool Prefix(GasBreatherFromWorldProvider __instance, OxygenBreather oxygen_breather, ref float gas_consumed)
        {
            Debug.Log(" === AmphibiousMod_GasBreatherFromWorldProvider_ConsumeGas === "+ gas_consumed);
            Klei.AI.Traits mi = oxygen_breather.gameObject.GetComponent<Klei.AI.Traits>();
            bool flag = mi.GetTraitIds().Contains("Amphibious");
            Debug.Log(" === AmphibiousMod_GasBreatherFromWorldProvider_ConsumeGas === " + flag);
            
            SimHashes getBreathableElement = oxygen_breather.GetBreathableElement;
            if (getBreathableElement == SimHashes.Vacuum)
            {
                return false;
            }
            HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(OnSimConsumeCallback, this, "GasBreatherFromWorldProvider");
            SimMessages.ConsumeMass(oxygen_breather.mouthCell, getBreathableElement, gas_consumed, 3, handle.index);
            
            return true;
        }

        private static SimHashes GetBreathableElementAtCell(int cell, CellOffset[] offsets = null)
        {
            if (offsets == null)
            {
                offsets = breathableCells;
            }
            int mouthCellAtCell = GetMouthCellAtCell(cell, offsets);
            if (!Grid.IsValidCell(mouthCellAtCell))
            {
                return SimHashes.Vacuum;
            }
            Element element = Grid.Element[mouthCellAtCell];
            return (!element.IsGas || !element.HasTag(GameTags.Breathable) || !(Grid.Mass[mouthCellAtCell] > noOxygenThreshold)) ? SimHashes.Vacuum : element.id;
        }
        
    }

    
    [HarmonyPatch(typeof(Database.Attributes), MethodType.Constructor)]
    internal static class AmphibiousMod_Attributes_Constructor
    {
        public static Klei.AI.Attribute BreathableElement = null;

        private static void Postfix(ref Database.Attributes __instance, ref ResourceSet parent)
        {
            BreathableElement = new Klei.AI.Attribute("BreathableElement", is_trainable: false, Klei.AI.Attribute.Display.Normal, is_profession: false);
            __instance.Add(BreathableElement);
            BreathableElement.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerSecond));

        }
    }
    */


}
