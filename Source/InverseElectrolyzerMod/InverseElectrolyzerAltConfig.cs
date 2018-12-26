using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STRINGS;
using TUNING;
using UnityEngine;


public class InverseElectrolyzerAltConfig : IBuildingConfig
{
	public const string ID = "InverseElectrolyzerAlt";

	private const float CO2_CONSUMPTION_RATE = 0.3f;

	private const float H2O_CONSUMPTION_RATE = 1f;

	private static readonly LogicPorts.Port[] INPUT_PORTS = new LogicPorts.Port[1]
	{
		LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(-1, 0), UI.LOGIC_PORTS.CONTROL_OPERATIONAL, false)
	};

	private ConduitPortInfo secondaryInputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 0));

	public override BuildingDef CreateBuildingDef()
	{
		string id = "InverseElectrolyzerAlt";
		int width = 4;
		int height = 3;
		string anim = "waterpurifier_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tIER = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] aLL_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tIER2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tIER, aLL_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tIER2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.MaterialCategory = MATERIALS.ALL_METALS;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.PowerInputOffset = new CellOffset(2, 0);
		buildingDef.UtilityInputOffset = new CellOffset(-1, 2);
		buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.capacityKg = 30f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);

		InverseElectrolyzerAlt airFilter = go.AddOrGet<InverseElectrolyzerAlt>();
		airFilter.filterTag = GameTagExtensions.Create(SimHashes.Hydrogen);
		airFilter.portInfo = this.secondaryInputPort;
		
	}

	private void AttachPort(GameObject go)
	{
		ConduitSecondaryInput conduitInput = go.AddComponent<ConduitSecondaryInput>();
		conduitInput.portInfo = this.secondaryInputPort;
	}

	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		GeneratedBuildings.RegisterLogicPorts(go, InverseElectrolyzerAltConfig.INPUT_PORTS);
		this.AttachPort(go);
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		GeneratedBuildings.RegisterLogicPorts(go, InverseElectrolyzerAltConfig.INPUT_PORTS);
		this.AttachPort(go);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.DoPostConfigure(go);
		GeneratedBuildings.RegisterLogicPorts(go, InverseElectrolyzerAltConfig.INPUT_PORTS);
		go.AddOrGet<LogicOperationalController>();
        /*
		go.GetComponent<KPrefabID>().prefabInitFn += delegate (GameObject game_object)
		{
			PoweredActiveController.Def.Instance instance = new PoweredActiveController.Instance(game_object.GetComponent<KPrefabID>());
			instance.StartSM();
		};
        */
        go.AddOrGetDef<PoweredActiveController.Def>();
    }
}

