using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;

namespace ElementTraits
{
    /*
    [HarmonyPatch(typeof(OffsetTableTracker), "UpdateCell")]
    internal class ReachabilityRangeMod_OffsetTableTracker_UpdateCell
    {

        private static void Prefix(OffsetTableTracker __instance, int previous_cell, int current_cell)
        {
            Debug.Log(" === ReachabilityRangeMod_OffsetTableTracker_UpdateCell Prefix === ");
            if (previous_cell != current_cell)
            {
                __instance.DebugDrawOffsets(current_cell);
                if (!__instance.solidPartitionerEntry.IsValid())
                {
                    Extents extents = new Extents(current_cell, (CellOffset[][])AccessTools.Field(typeof(OffsetTableTracker), "table").GetValue(__instance));
                    extents.height += 2;
                    extents.y--;
                    //__instance.DebugDrawOffsets(current_cell);
                }
            }
        }
    }
    */

    [HarmonyPatch(typeof(KSerialization.Manager), nameof(KSerialization.Manager.Initialize))]
    internal class ReachabilityRangeMod_KSerialization_Manager_Initialize
    {

        private static void Postfix()
        {
            Debug.Log(" === ReachabilityRangeMod_KSerialization_Manager_Initialize Prefix === ");
            Debug.Log("area_offsets: ");
            CellOffset[][] area_offsets = OffsetGroups.InvertedStandardTableWithCorners;
         
            Debug.Log("table: ");
            int idx = 0;
            foreach (CellOffset[] a in area_offsets)
            {
                Debug.Log("group "+idx++);
                foreach (CellOffset b in a)
                {
                    Debug.Log(b.ToString());
                }

            }
            OffsetGroups.InvertedStandardTableWithCorners = InvertedStandardTableWithCorners;
        }


        public static CellOffset[][] InvertedStandardTableWithCorners = OffsetTable.Mirror(new CellOffset[][]
        {
            new CellOffset[1]
            {
                new CellOffset(0, 0)
            },
            new CellOffset[1]
            {
                new CellOffset(0, 1)
            },
            new CellOffset[2]
            {
                new CellOffset(0, 2),
                new CellOffset(0, 1)
            },
            new CellOffset[3]
            {
                new CellOffset(0, 3),
                new CellOffset(0, 1),
                new CellOffset(0, 2)
            },
            //0,4
            new CellOffset[4]
            {
                new CellOffset(0, 4),                
                new CellOffset(0, 1),
                new CellOffset(0, 2),
                new CellOffset(0, 3)
            },
            //
            new CellOffset[1]
            {
                new CellOffset(0, -1)
            },
            new CellOffset[1]
            {
                new CellOffset(0, -2)
            },
            new CellOffset[3]
            {
                new CellOffset(0, -3),
                new CellOffset(0, -2),
                new CellOffset(0, -1)
            },
            //0,-4
            new CellOffset[4]
            {
                new CellOffset(0, -4),
                new CellOffset(0, -1),
                new CellOffset(0, -2),                
                new CellOffset(0, -3)
            },
            //
            new CellOffset[1]
            {
                new CellOffset(1, 0)
            },
            new CellOffset[1]
            {
                new CellOffset(1, 1)
            },
            new CellOffset[2]
            {
                new CellOffset(1, 1),
                new CellOffset(1, 0)
            },
            new CellOffset[3]
            {
                new CellOffset(1, 2),
                new CellOffset(1, 0),
                new CellOffset(1, 1)
            },
            new CellOffset[3]
            {
                new CellOffset(1, 2),
                new CellOffset(0, 1),
                new CellOffset(0, 2)
            },
            new CellOffset[3]
            {
                new CellOffset(1, 3),
                new CellOffset(1, 2),
                new CellOffset(1, 1)
            },
            new CellOffset[4]
            {
                new CellOffset(1, 3),
                new CellOffset(0, 1),
                new CellOffset(0, 2),
                new CellOffset(0, 3)
            },
            //1,4
            new CellOffset[4]
            {
                new CellOffset(1, 4),
                new CellOffset(1, 3),
                new CellOffset(1, 2),
                new CellOffset(1, 1)
            },
            new CellOffset[5]
            {
                new CellOffset(1, 4),
                new CellOffset(0, 1),
                new CellOffset(0, 2),
                new CellOffset(0, 3),
                new CellOffset(1, 3)
            },
            //
            new CellOffset[1]
            {
                new CellOffset(1, -1)
            },
            new CellOffset[3]
            {
                new CellOffset(1, -2),
                new CellOffset(1, 0),
                new CellOffset(1, -1)
            },
            new CellOffset[2]
            {
                new CellOffset(1, -2),
                new CellOffset(1, -1)
            },
            new CellOffset[4]
            {
                new CellOffset(1, -3),
                new CellOffset(1, 0),
                new CellOffset(1, -1),
                new CellOffset(1, -2)
            },
            new CellOffset[3]
            {
                new CellOffset(1, -3),
                new CellOffset(1, -2),
                new CellOffset(1, -1)
            },
            //1,-4
            new CellOffset[5]
            {
                new CellOffset(1, -4),
                new CellOffset(1, 0),
                new CellOffset(1, -1),
                new CellOffset(1, -2),
                new CellOffset(1, -3)
            },
            new CellOffset[4]
            {
                new CellOffset(1, -4),
                new CellOffset(1, -2),
                new CellOffset(1, -1),
                new CellOffset(1, -3)
            },
            //
            new CellOffset[2]
            {
                new CellOffset(2, 0),
                new CellOffset(1, 0)
            },
            new CellOffset[2]
            {
                new CellOffset(2, 1),
                new CellOffset(1, 1)
            },
            new CellOffset[3]
            {
                new CellOffset(2, 2),
                new CellOffset(1, 2),
                new CellOffset(1, 1)
            },
            new CellOffset[4]
            {
                new CellOffset(2, 3),
                new CellOffset(1, 1),
                new CellOffset(1, 2),
                new CellOffset(1, 3)
            },
            //2,4
            new CellOffset[5]
            {
                new CellOffset(2, 4),
                new CellOffset(1, 1),
                new CellOffset(1, 2),
                new CellOffset(1, 3),
                new CellOffset(1, 4)
            },
            //
            new CellOffset[3]
            {
                new CellOffset(2, -1),
                new CellOffset(2, 0),
                new CellOffset(1, 0)
            },
            new CellOffset[4]
            {
                new CellOffset(2, -2),
                new CellOffset(1, 0),
                new CellOffset(1, -1),
                new CellOffset(2, -1)
            },
            new CellOffset[4]
            {
                new CellOffset(2, -3),
                new CellOffset(1, 0),
                new CellOffset(1, -1),
                new CellOffset(1, -2)
            },
            //2,-4
            new CellOffset[5]
            {
                new CellOffset(2, -4),
                new CellOffset(1, 0),
                new CellOffset(1, -1),
                new CellOffset(1, -2),
                new CellOffset(1, -3)
            },
            //
        });
    }
    

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
                       id = "Ice",
                       statBonus = -TUNING.DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
                       probability = TUNING.DUPLICANTSTATS.PROBABILITY_MED
                   }
               );

            TUNING.TRAITS.TRAIT_CREATORS.Add(
                    (System.Action)CreateNamedTraitM.Invoke(null,
                            new object[] { "Ice", "Ice", "This duplicant doesn't get hypothermia.", true }     // true: positive trait
                        )
                );

            TUNING.DUPLICANTSTATS.GOODTRAITS.Add(
                   new TUNING.DUPLICANTSTATS.TraitVal
                   {
                       id = "Amphibious",
                       statBonus = -TUNING.DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
                       probability = TUNING.DUPLICANTSTATS.PROBABILITY_MED
                   }
               );

            TUNING.TRAITS.TRAIT_CREATORS.Add(
                    (System.Action)CreateNamedTraitM.Invoke(null,
                            new object[] { "Amphibious", "Amphibious", "This duplicant breaths under water.", true }     // true: positive trait
                        )
                );


        }
    }


    [HarmonyPatch(typeof(TemperatureMonitor.Instance), nameof(TemperatureMonitor.Instance.IsHypothermic))]
    internal static class AmphibiousMod_TemperatureMonitor_IsHypothermic
    {

        private static bool Prefix(TemperatureMonitor.Instance __instance, ref bool __result)
        {
            Klei.AI.Traits traits = __instance.gameObject.GetComponent<Klei.AI.Traits>();
            bool flag = traits.GetTraitIds().Contains("Ice");
            if (!flag) return true;
            __result = false;
            return false;
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
