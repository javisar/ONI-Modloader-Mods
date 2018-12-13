using System;
using TUNING;
using UnityEngine;

namespace TemperatureFilterMod
{
    public class GasTemperatureFilterConfig : IBuildingConfig
    {
        private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 0));
        public const string ID = "GasTemperatureFilter";
        private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

        public override BuildingDef CreateBuildingDef()
        {
            string id = ID;
            int width = 3;
            int height = 1;
            string anim = "filter_gas_kanim";
            int hitpoints = 30;
            float construction_time = 10f;
            float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
            string[] rawMetals = MATERIALS.RAW_METALS;
            float melting_point = 1600f;
            BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
            EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, rawMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, tieR1, 0.2f);
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 120f;
            buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
            buildingDef.ExhaustKilowattsWhenActive = 0.0f;
            buildingDef.InputConduitType = ConduitType.Gas;
            buildingDef.OutputConduitType = ConduitType.Gas;
            buildingDef.Floodable = false;
            buildingDef.ViewMode = OverlayModes.GasConduits.ID;
            buildingDef.AudioCategory = "Metal";
            buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
            buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
            buildingDef.PermittedRotations = PermittedRotations.R360;
            return buildingDef;
        }

        private void AttachPort(GameObject go)
        {
            go.AddComponent<ConduitSecondaryOutput>().portInfo = this.secondaryPort;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            base.DoPostConfigurePreview(def, go);
            this.AttachPort(go);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            base.DoPostConfigureUnderConstruction(go);
            this.AttachPort(go);
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
            go.AddOrGet<Structure>();
            go.AddOrGet<TemperatureFilter>().portInfo = this.secondaryPort;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
            TemperatureFilter filter = go.AddOrGet<TemperatureFilter>();
            filter.rangeMin = 0f;
            filter.rangeMax = 9999f;
        }
    }
}
