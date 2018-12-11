using System;
using TUNING;
using UnityEngine;

namespace FluidPressureSensor
{
    public class LiquidConduitPressureConfig : ConduitSensorConfig
    {
        public static string ID = "LiquidConduitPressureSensor";

        protected override ConduitType ConduitType
        {
            get
            {
                return ConduitType.Liquid;
            }
        }

        static LiquidConduitPressureConfig()
        {
        }

        public LiquidConduitPressureConfig()
        {
        }

        public override BuildingDef CreateBuildingDef()
        {
            return base.CreateBuildingDef(LiquidConduitPressureConfig.ID, "liquid_germs_sensor_kanim", BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Logger.Log(" ===LiquidConduitPressureConfig.DoPostConfigureComplete");
            base.DoPostConfigureComplete(go);
            var conduitType = go.AddComponent<ConduitPressureSensor>();
            conduitType.conduitType = this.ConduitType;
            conduitType.rangeMax = 10000f;
            conduitType.rangeMin = 0f;
            conduitType.Threshold = 0f;
            conduitType.ActivateAboveThreshold = true;
            conduitType.manuallyControlled = false;
            conduitType.defaultState = false;
        }
    }
}