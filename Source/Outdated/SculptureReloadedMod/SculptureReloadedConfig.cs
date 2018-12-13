// IceSculptureConfig
using STRINGS;
using TUNING;
using UnityEngine;

public class SculptureReloadedConfig : IBuildingConfig
{
	public const string ID = "SculptureReloaded";

	public override BuildingDef CreateBuildingDef()
	{		
		int width = 2;
		int height = 2;
		string anim = "icesculpture_kanim";
		int hitpoints = 100;
		float construction_time = 240f;
		float[] tIER = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		//string[] construction_materials = new string[1]
		//{
		//	"Ice"
		//};
		string[] construction_materials = TUNING.MATERIALS.ANY_BUILDABLE;
		float melting_point = 273.15f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues nONE = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, tIER, construction_materials, melting_point, build_location_rule, new EffectorValues
		{
			amount = 10,
			radius = 10
		}, nONE, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "slab";
		return buildingDef;
	}


	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.DoPostConfigure(go);
		Artable artable = go.AddComponent<Sculpture>();
		artable.requiredRolePerk = RoleManager.rolePerks.CanArt.id;
		artable.stages.Add(new Artable.Stage("Default", STRINGS.BUILDINGS.PREFABS.ICESCULPTURE.NAME, "slab", 0, false, Artable.Status.Ready));
		artable.stages.Add(new Artable.Stage("Bad", STRINGS.BUILDINGS.PREFABS.ICESCULPTURE.POORQUALITYNAME, "crap", 5, false, Artable.Status.Ugly));
		artable.stages.Add(new Artable.Stage("Average", STRINGS.BUILDINGS.PREFABS.ICESCULPTURE.AVERAGEQUALITYNAME, "idle", 15, true, Artable.Status.Okay));
	}
}
