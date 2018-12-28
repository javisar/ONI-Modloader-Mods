// PrickleFruitConfig
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class FungiSaladConfig : IEntityConfig
{
	public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

	public static string ID = "FungiSalad";

	public static ComplexRecipe recipe;

	
	public GameObject CreatePrefab()
	{
		string name = UI.FormatAsLink("Fungi Salad", "FUNGISALAD");
		string description = "FungiSalad";
		GameObject template = EntityTemplates.CreateLooseEntity(ID, name, description, 1f, false, Assets.GetAnim("mushbarfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, isPickupable: true);
		return EntityTemplates.ExtendEntityToFood(template, MoreFood.MoreFood_CookingStationConfig_ConfigureRecipes.FUNGISALAD);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{		
	}
	
}
