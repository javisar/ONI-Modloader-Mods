using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class GasReservoirSmartConfig : GasReservoirConfig
{
    private static readonly LogicPorts.Port[] OUTPUT_PORTS = new LogicPorts.Port[1]
    {
    LogicPorts.Port.OutputPort(BatterySmart.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT_DESC, true)
    };

    public new const string ID = "GasReservoirSmart";

    public override BuildingDef CreateBuildingDef()
    {
        BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 5, 3, "gasstorage_kanim", 100, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
        buildingDef.InputConduitType = ConduitType.Gas;
        buildingDef.OutputConduitType = ConduitType.Gas;
        buildingDef.Floodable = false;
        buildingDef.ViewMode = SimViewMode.GasVentMap;
        buildingDef.AudioCategory = "HollowMetal";
        buildingDef.UtilityInputOffset = new CellOffset(1, 2);
        buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        base.ConfigureBuildingTemplate(go, prefab_tag);
    }

    public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
    {
        GeneratedBuildings.RegisterLogicPorts(go, (LogicPorts.Port[])null, GasReservoirSmartConfig.OUTPUT_PORTS);
    }

    public override void DoPostConfigureUnderConstruction(GameObject go)
    {
        GeneratedBuildings.RegisterLogicPorts(go, (LogicPorts.Port[])null, GasReservoirSmartConfig.OUTPUT_PORTS);
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        GeneratedBuildings.RegisterLogicPorts(go, (LogicPorts.Port[])null, GasReservoirSmartConfig.OUTPUT_PORTS);
        base.DoPostConfigureComplete(go);
    }
}
