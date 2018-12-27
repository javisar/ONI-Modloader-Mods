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

	public static readonly EdiblesManager.FoodInfo FUNGISALAD = new EdiblesManager.FoodInfo("FungiSalad", 3000000f, 3, 255.15f, 277.15f, 5200f, can_rot: true);

	public GameObject CreatePrefab()
	{
		string name = UI.FormatAsLink("Fungi Salad", "FUNGISALAD");
		string description = "FungiSalad";
		GameObject template = EntityTemplates.CreateLooseEntity(ID, name, description, 1f, false, Assets.GetAnim("mushbarfried_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, isPickupable: true);
		return EntityTemplates.ExtendEntityToFood(template, FUNGISALAD);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{		
	}
	
}
