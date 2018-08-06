// WireConfig
using TUNING;
using UnityEngine;

public class MultiWireConfig : BaseWireConfig
{
    public const string ID = "MultiWire";

    public override BuildingDef CreateBuildingDef()
    {
        string id = "MultiWire";
        string anim = "utilities_electric_kanim";
        float construction_time = 3f;
        float[] tIER = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
        float insulation = 0.05f;
        EffectorValues nONE = NOISE_POLLUTION.NONE;
        BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tIER, insulation, BUILDINGS.DECOR.PENALTY.TIER0, nONE);
        buildingDef.ViewMode = SimViewMode.Logic;
        buildingDef.ObjectLayer = ObjectLayer.LogicWires;
        buildingDef.TileLayer = ObjectLayer.LogicWiresTiling;
        buildingDef.SceneLayer = Grid.SceneLayer.Wires;
        buildingDef.isUtility = true;
        buildingDef.DragBuild = true;
        GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, id);
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        base.ConfigureBuildingTemplate(go, prefab_tag);
        GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		KAnimGraphTileVisualizer kAnimGraphTileVisualizer = go.AddOrGet<KAnimGraphTileVisualizer>();
		kAnimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Logic;
		kAnimGraphTileVisualizer.isPhysicalBuilding = true;
    }

    public override void DoPostConfigureUnderConstruction(GameObject go)
    {
        base.DoPostConfigureUnderConstruction(go);
        Constructable component = go.GetComponent<Constructable>();
        component.isDiggingRequired = false;
        component.choreTags = GameTags.ChoreTypes.WiringChores;
        KAnimGraphTileVisualizer kAnimGraphTileVisualizer = go.AddOrGet<KAnimGraphTileVisualizer>();
        kAnimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Logic;
        kAnimGraphTileVisualizer.isPhysicalBuilding = false;
    }


    public override void DoPostConfigureComplete(GameObject go)
    {
        go.AddOrGet<LogicWire>();
        base.DoPostConfigureComplete(Wire.WattageRating.Max1000, go);
    }
}
