// PressureDoorConfig
using System.Reflection;
using Harmony;
using TUNING;
using UnityEngine;

public class InsulatedPressureDoorConfig : IBuildingConfig
{
    public const string ID = "InsulatedPressureDoor";

    public override BuildingDef CreateBuildingDef()
    {
        Debug.Log(" === InsulatedPressureDoorConfig INI === ");
        string id = "InsulatedPressureDoor";
        int width = 1;
        int height = 2;
        string anim = "door_external_kanim";
        int hitpoints = 30;
        float construction_time = 60f;
        float[] tIER = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
        string[] aLL_METALS = MATERIALS.ANY_BUILDABLE;
        float melting_point = 1600f;
        BuildLocationRule build_location_rule = BuildLocationRule.Tile;
        EffectorValues nONE = NOISE_POLLUTION.NONE;
        BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tIER, aLL_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, nONE, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.Entombable = false;
		buildingDef.IsFoundation = true;
		buildingDef.ViewMode = SimViewMode.PowerMap;
		buildingDef.TileLayer = ObjectLayer.FoundationTile;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.R90;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;


		//buildingDef.Insulation = 0.01f;
		buildingDef.ThermalConductivity = 0.01f;		
        //buildingDef.UseStructureTemperature = false;

        return buildingDef;
    }

    public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
    {
        GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS);
    }

    public override void DoPostConfigureUnderConstruction(GameObject go)
    {
        GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS);
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
		Door door = go.AddOrGet<Door>();
		door.hasComplexUserControls = true;
        door.unpoweredAnimSpeed = 0.65f;
		door.poweredAnimSpeed = 5f;
		door.doorClosingSoundEventName = "MechanizedAirlock_closing";
		door.doorOpeningSoundEventName = "MechanizedAirlock_opening";
		go.AddOrGet<AccessControl>();
		go.AddOrGet<KBoxCollider2D>();
		Prioritizable.AddRef(go);
        Workable workable = go.AddOrGet<Workable>();
        workable.workTime = 5f;
        GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS);
        Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());        
		BuildingTemplates.DoPostConfigure(go);
		AccessControl component = go.GetComponent<AccessControl>();
		component.controlEnabled = true;
		KBatchedAnimController component2 = go.GetComponent<KBatchedAnimController>();
		component2.initialAnim = "closed";
	}
    
	
    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        go.AddOrGet<Insulator>();      
    }
    
}
