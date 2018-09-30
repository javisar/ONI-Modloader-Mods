using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ZeroPointModuleConfig : BaseBatteryConfig
{
    public const string ID = "ZeroPointModule";

    public override BuildingDef CreateBuildingDef()
    {
        //string id = "ZeroPointModule";
        int width = 2;
        int height = 2;
        int hitpoints = 30;
        string anim = "batterymed_kanim";
        float construction_time = 60f;
        float[] tIER = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
        string[] aLL_METALS = TUNING.MATERIALS.ALL_METALS;
        float melting_point = 800f;
        float exhaust_temperature_active = 0.25f;
        float self_heat_kilowatts_active = 1f;
        EffectorValues tIER2 = TUNING.NOISE_POLLUTION.NOISY.TIER1;
        BuildingDef result = base.CreateBuildingDef(ID, width, height, hitpoints, anim, construction_time, tIER, aLL_METALS, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tIER2);
        SoundEventVolumeCache.instance.AddVolume("batterymed_kanim", "Battery_med_rattle", TUNING.NOISE_POLLUTION.NOISY.TIER2);
        return result;
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        Battery battery = go.AddOrGet<Battery>();
        battery.capacity = 40000f;
        battery.joulesLostPerSecond = battery.capacity * 0.05f / 600f;
        base.DoPostConfigureComplete(go);
    }
}

