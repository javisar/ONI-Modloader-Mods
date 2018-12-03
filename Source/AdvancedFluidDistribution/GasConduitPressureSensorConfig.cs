using System;
using TUNING;
using UnityEngine;

namespace AdvancedFluidDistribution
{
    public class GasConduitPressureConfig : ConduitSensorConfig
    {
        public static string ID;

        protected override ConduitType ConduitType
        {
            get
            {
                return ConduitType.Gas;
            }
        }

        static GasConduitPressureConfig()
        {
            GasConduitPressureConfig.ID = "GasConduitPressureSensor";
        }

        public GasConduitPressureConfig()
        {
        }

        public override BuildingDef CreateBuildingDef()
        {
            return base.CreateBuildingDef(GasConduitPressureConfig.ID, "gas_germs_sensor_kanim", BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Logger.Log(" ===GasConduitPressureConfig.DoPostConfigureComplete");
            base.DoPostConfigureComplete(go);
            var conduitType = go.AddComponent<ConduitPressureSensor>();
            conduitType.conduitType = this.ConduitType;
            conduitType.RangeMax = 1000f;
            conduitType.RangeMin = 0f;
            conduitType.Threshold = 0f;
            conduitType.ActivateAboveThreshold = true;
            conduitType.manuallyControlled = false;
            conduitType.defaultState = false;
        }
    }
}