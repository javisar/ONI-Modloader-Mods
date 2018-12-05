using TUNING;
using UnityEngine;

public class GasWarpConfig : IBuildingConfig
{
	public const string ID = "GasWarp";

	public const ConduitType CONDUIT_TYPE = (ConduitType)101;
    
    public override BuildingDef CreateBuildingDef()
	{
		
		int width = 1;
		int height = 2;
		string anim = "valvegas_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] Tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] Raw_Metals = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues Tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, Tier, Raw_Metals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, Tier2, 0.2f);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
        buildingDef.RequiresPowerInput = true;
        buildingDef.PowerInputOffset = new CellOffset(0, 1);
        buildingDef.EnergyConsumptionWhenActive = 480f;
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
		Workable workable = go.AddOrGet<Workable>();
		workable.workTime = 5f;
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		BuildingTemplates.DoPostConfigure(go);
	}
}
