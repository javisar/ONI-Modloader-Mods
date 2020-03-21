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
        int hitpoints = 60;
        string anim = "smartbattery_kanim";
        float construction_time = 20f;
        float[] tIER = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
        string[] aLL_METALS = TUNING.MATERIALS.ALL_METALS;
        float melting_point = 800f;
        float exhaust_temperature_active = 0f;
        float self_heat_kilowatts_active = 0f;
       
        BuildingDef result = base.CreateBuildingDef(ID, width, height, hitpoints, anim, construction_time, tIER, aLL_METALS, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, TUNING.BUILDINGS.DECOR.PENALTY.TIER5, TUNING.NOISE_POLLUTION.NOISY.TIER6);
        //result.EnergyConsumptionWhenActive = 120f;
        result.Floodable = false;
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

