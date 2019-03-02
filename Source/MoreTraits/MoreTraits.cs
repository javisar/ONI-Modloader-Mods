using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;

namespace MoreTraits
{
    [HarmonyPatch(typeof(ModifierSet), "LoadTraits")]
    internal static class MoreTraits_ModifierSet_LoadTraits
    {
        private static MethodInfo mi = AccessTools.Method(typeof(TUNING.TRAITS), "CreateNamedTrait");

        private static void Prefix()
        {
            Debug.Log(" === MoreTraits_ModifierSet_LoadTraits === ");
            TUNING.DUPLICANTSTATS.GOODTRAITS.Add(
                   new TUNING.DUPLICANTSTATS.TraitVal
                   {
                       id = "Amphibious",
                       statBonus = -TUNING.DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
                       probability = TUNING.DUPLICANTSTATS.PROBABILITY_LOW
                   }
               );

            TUNING.TRAITS.TRAIT_CREATORS.Add(
                    //(System.Action) mi.Invoke(null, new object[] { "Archaeologist", STRINGS.DUPLICANTS.TRAITS.ARCHAEOLOGIST.NAME, STRINGS.DUPLICANTS.TRAITS.ARCHAEOLOGIST.DESC })
                    (System.Action)mi.Invoke(null, new object[] { "Amphibious", "Amphibious", "This Duplicant breaths under water.", true})
                );
        }
    }


    [HarmonyPatch(typeof(OxygenBreather), "GetBreathableElementAtCell")]
    internal static class MoreTraits_OxygenBreather_GetBreathableElementAtCell
    {
        
        private static MethodInfo mi = AccessTools.Method(typeof(OxygenBreather), "GetMouthCellAtCell");
        /*
        private static MethodInfo _GetMouthCellAtCellM = AccessTools.Method(typeof(OxygenBreather), "GetMouthCellAtCell");
        private delegate int GetMouthCellAtCell_Delegate(int cell, CellOffset[] offsets);
        private static GetMouthCellAtCell_Delegate _GetMouthCellAtCell =
            (GetMouthCellAtCell_Delegate)Delegate.CreateDelegate(typeof(GetMouthCellAtCell_Delegate), _GetMouthCellAtCellM);
        */
        private static bool Prefix(OxygenBreather __instance, ref SimHashes __result, ref int cell, ref CellOffset[] offsets)
        {

            //Debug.Log(" === MoreTraits_OxygenBreather_GetBreathableElementAtCell === ");
            Klei.AI.Traits traits = __instance.gameObject.GetComponent<Klei.AI.Traits>();
            bool flag = traits.GetTraitIds().Contains("Amphibious");
            //Debug.Log(" === MoreTraits_OxygenBreather_GetBreathableElementAtCell === " + flag);


            if (offsets == null)
            {
                offsets = __instance.breathableCells;
            }
            int mouthCellAtCell = (int)mi.Invoke(__instance, new object[] { cell, offsets });//this.GetMouthCellAtCell(cell, offsets);
            //int mouthCellAtCell = GetMouthCellAtCell(__instance, cell, offsets);
            //int mouthCellAtCell = _GetMouthCellAtCell(cell, offsets);
            if (!Grid.IsValidCell(mouthCellAtCell))
            {
                __result = SimHashes.Vacuum;
                return false;
            }
            Element element = Grid.Element[mouthCellAtCell];

            if (flag)
            {
                __result = ((!element.IsGas || !element.HasTag(GameTags.Breathable)) && (!element.IsLiquid || !element.HasTag(GameTags.AnyWater)) || !(Grid.Mass[mouthCellAtCell] > __instance.noOxygenThreshold)) ? SimHashes.Vacuum : element.id;
            }
            else
            {
                __result = (!element.IsGas || !element.HasTag(GameTags.Breathable) || !(Grid.Mass[mouthCellAtCell] > __instance.noOxygenThreshold)) ? SimHashes.Vacuum : element.id;
            }
            return false;
        }
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
    internal static class MoreTraits_DUPLICANTSTATS_Constructor
    {
        //private static MethodInfo mi = AccessTools.Method(typeof(TUNING.TRAITS), "CreateNamedTrait");

        private static void Postfix()
        {
            Debug.Log(" === MoreTraits_DUPLICANTSTATS_Constructor === ");

           
        }
    }
    
    [HarmonyPatch(typeof(GasBreatherFromWorldProvider), "ConsumeGas")]
    internal static class MoreTraits_GasBreatherFromWorldProvider_ConsumeGas
    {

        //private static MethodInfo mi = AccessTools.Method(typeof(OxygenBreather), "GetMouthCellAtCell");
        
        private static bool Prefix(GasBreatherFromWorldProvider __instance, OxygenBreather oxygen_breather, ref float gas_consumed)
        {
            Debug.Log(" === MoreTraits_GasBreatherFromWorldProvider_ConsumeGas === "+ gas_consumed);
            Klei.AI.Traits mi = oxygen_breather.gameObject.GetComponent<Klei.AI.Traits>();
            bool flag = mi.GetTraitIds().Contains("Amphibious");
            Debug.Log(" === MoreTraits_GasBreatherFromWorldProvider_ConsumeGas === " + flag);
            
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
    internal static class MoreTraits_Attributes_Constructor
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
