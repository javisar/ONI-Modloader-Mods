using TUNING;
using UnityEngine;

namespace FluidWarpMod
{
    public class WarpRequesterLiquid : IBuildingConfig
    {
        public const ConduitType CONDUIT_TYPE = FluidWarpMod_Utils.LIQUID_CONDUIT_REQUESTER_TYPE;
        public const ConduitType _conduitType = ConduitType.Liquid;
        public const string ID = "WarpRequesterLiquid";

        public override BuildingDef CreateBuildingDef()
        {

            int width = 1;
            int height = 2;
            string anim = "valveliquid_kanim";
            int hitpoints = 30;
            float construction_time = 5f;
            float[] tIER = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
            string[] rAW_METALS = MATERIALS.ALL_METALS;
            float melting_point = 1600f;
            BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
            EffectorValues tIER6 = NOISE_POLLUTION.NOISY.TIER0;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, tIER, rAW_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER5, tIER6, 0.2f);
            buildingDef.InputConduitType = ConduitType.None;
            buildingDef.OutputConduitType = _conduitType;
            buildingDef.Floodable = false;
            buildingDef.ViewMode = _conduitType == ConduitType.Liquid ? OverlayModes.LiquidConduits.ID : OverlayModes.GasConduits.ID;
            buildingDef.AudioCategory = "Metal";
            buildingDef.PermittedRotations = PermittedRotations.R360;
            buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
            buildingDef.RequiresPowerInput = false;
            buildingDef.PowerInputOffset = new CellOffset(0, 1);
            buildingDef.EnergyConsumptionWhenActive = 0f;
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
            ValveBase valveBase = go.AddOrGet<ValveBase>();
            valveBase.conduitType = CONDUIT_TYPE;
            valveBase.maxFlow = 10f;

            go.AddOrGet<Valve>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Object.DestroyImmediate(go.GetComponent<RequireInputs>());
            Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
            Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
            go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
            BuildingTemplates.DoPostConfigure(go);
        }
    }

    public class WarpRequesterGas : IBuildingConfig
    {
        public const ConduitType CONDUIT_TYPE = FluidWarpMod_Utils.GAS_CONDUIT_REQUESTER_TYPE;
        public const ConduitType _conduitType = ConduitType.Gas;
        public const string ID = "WarpRequesterGas";

        public override BuildingDef CreateBuildingDef()
        {

            int width = 1;
            int height = 2;
            string anim = "valvegas_kanim";
            int hitpoints = 30;
            float construction_time = 5f;
            float[] tIER = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
            string[] rAW_METALS = MATERIALS.ALL_METALS;
            float melting_point = 1600f;
            BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
            EffectorValues tIER6 = NOISE_POLLUTION.NOISY.TIER0;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, tIER, rAW_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER5, tIER6, 0.2f);
            buildingDef.InputConduitType = ConduitType.None;
            buildingDef.OutputConduitType = _conduitType;
            buildingDef.Floodable = false;
            buildingDef.ViewMode = _conduitType == ConduitType.Liquid ? OverlayModes.LiquidConduits.ID : OverlayModes.GasConduits.ID;
            buildingDef.AudioCategory = "Metal";
            buildingDef.PermittedRotations = PermittedRotations.R360;
            buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
            buildingDef.RequiresPowerInput = false;
            buildingDef.PowerInputOffset = new CellOffset(0, 1);
            buildingDef.EnergyConsumptionWhenActive = 0f;
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
            ValveBase valveBase = go.AddOrGet<ValveBase>();
            valveBase.conduitType = CONDUIT_TYPE;
            valveBase.maxFlow = 10f;

            go.AddOrGet<Valve>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Object.DestroyImmediate(go.GetComponent<RequireInputs>());
            Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
            Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
            go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
            BuildingTemplates.DoPostConfigure(go);
        }
    }
}