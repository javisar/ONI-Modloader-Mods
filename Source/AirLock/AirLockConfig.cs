// PressureDoorConfig
using Harmony;
using TUNING;
using UnityEngine;

public class AirLockConfig : IBuildingConfig
{
	public const string ID = "AirLock";

	public override BuildingDef CreateBuildingDef()
	{
		int width = 1;
		int height = 2;
		string anim = "door_external_kanim";
		int hitpoints = 60;
		float construction_time = 120f;
		float[] tIER = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] aLL_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues nONE = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, tIER, aLL_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER3, nONE, 1f);
		buildingDef.Overheatable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.Entombable = false;
		buildingDef.IsFoundation = true;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.TileLayer = ObjectLayer.FoundationTile;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.R90;
		buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
		buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
		SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Open_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);
		SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Close_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);
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
		door.doorType = (Door.DoorType) 100;
		//go.AddOrGet<ZoneTile>();

		Component val = go.AddComponent(AccessTools.TypeByName("ZoneTile"));
		KMonoBehaviour kMonoBehaviour = val as KMonoBehaviour;
		if (kMonoBehaviour != null)
		{
			kMonoBehaviour.CreateDef();
		}
		go.AddOrGet<AccessControl>();
		go.AddOrGet<KBoxCollider2D>();
		Prioritizable.AddRef(go);
		CopyBuildingSettings copyBuildingSettings = go.AddOrGet<CopyBuildingSettings>();
		copyBuildingSettings.copyGroupTag = GameTags.Door;
		Workable workable = go.AddOrGet<Workable>();
		workable.workTime = 5f;
		GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS);
		Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
		AccessControl component = go.GetComponent<AccessControl>();
		component.controlEnabled = true;
		KBatchedAnimController component2 = go.GetComponent<KBatchedAnimController>();
		component2.initialAnim = "closed";
	}
}
