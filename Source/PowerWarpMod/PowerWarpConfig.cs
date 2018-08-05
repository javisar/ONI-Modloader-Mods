// PowerTransformerSmallConfig
using TUNING;
using UnityEngine;

public class PowerWarpConfig : IBuildingConfig
{
    public const string ID = "PowerWarp";

    public override BuildingDef CreateBuildingDef()
    {
        string id = "PowerWarp";
        int width = 2;
        int height = 2;
        string anim = "transformer_small_kanim";
        int hitpoints = 30;
        float construction_time = 30f;
        float[] tIER = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
        string[] rAW_METALS = MATERIALS.RAW_METALS;
        float melting_point = 800f;
        BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
        EffectorValues tIER2 = NOISE_POLLUTION.NOISY.TIER5;
        BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tIER, rAW_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tIER2, 0.2f);
        buildingDef.RequiresPowerInput = true;
        buildingDef.UseWhitePowerOutputConnectorColour = true;
        buildingDef.PowerInputOffset = new CellOffset(0, 1);
        buildingDef.PowerOutputOffset = new CellOffset(1, 0);
        buildingDef.ElectricalArrowOffset = new CellOffset(1, 0);
        buildingDef.ExhaustKilowattsWhenActive = 0.25f;
        buildingDef.SelfHeatKilowattsWhenActive = 1f;
        buildingDef.ViewMode = SimViewMode.PowerMap;
        buildingDef.AudioCategory = "Metal";
        buildingDef.ExhaustKilowattsWhenActive = 0f;
        buildingDef.SelfHeatKilowattsWhenActive = 1f;
        buildingDef.Entombable = true;
        buildingDef.GeneratorWattageRating = 1000f;
        buildingDef.GeneratorBaseCapacity = 1000f;
        buildingDef.PermittedRotations = PermittedRotations.FlipH;
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        go.GetComponent<KPrefabID>().AddPrefabTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.AddComponent<RequireInputs>();
        BuildingDef def = go.GetComponent<Building>().Def;
        Battery battery = go.AddOrGet<Battery>();
        battery.powerSortOrder = 1000;
        battery.capacity = def.GeneratorWattageRating;
        battery.chargeWattage = def.GeneratorWattageRating;
        PowerTransformer powerTransformer = go.AddComponent<PowerTransformer>();
        powerTransformer.powerDistributionOrder = 9;

        ValveBase valveBase = go.AddOrGet<ValveBase>();
        //valveBase.conduitType = ConduitType.Liquid;
        valveBase.conduitType = (ConduitType)102;
        valveBase.maxFlow = 10f;
        valveBase.animFlowRanges = new ValveBase.AnimRangeInfo[3]
        {
            new ValveBase.AnimRangeInfo(3f, "lo"),
            new ValveBase.AnimRangeInfo(7f, "med"),
            new ValveBase.AnimRangeInfo(10f, "hi")
        };

        go.AddOrGet<Valve>();
        Workable workable = go.AddOrGet<Workable>();
        workable.workTime = 5f;
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        Object.DestroyImmediate(go.GetComponent<EnergyConsumer>());
        Object.DestroyImmediate(go.GetComponent<RequireInputs>());
        BuildingTemplates.DoPostConfigure(go);
        go.GetComponent<KPrefabID>().prefabInitFn += delegate (GameObject game_object)
        {
            PoweredActiveController.Instance instance = new PoweredActiveController.Instance(game_object.GetComponent<KPrefabID>());
            instance.StartSM();
        };
    }
}
