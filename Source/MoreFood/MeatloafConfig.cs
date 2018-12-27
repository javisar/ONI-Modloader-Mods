// PrickleFruitConfig
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class MeatloafConfig : IEntityConfig
{
	public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

	public static string ID = "Meatloaf";

	public static ComplexRecipe recipe;

	public static readonly EdiblesManager.FoodInfo MEATLOAF = new EdiblesManager.FoodInfo("Meatloaf", 3000000f, 3, 255.15f, 277.15f, 5200f, can_rot: true);
	/*
	private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate (Edible component, object data)
	{
		OnEatComplete(component);
	});
	*/
	public GameObject CreatePrefab()
	{
		string name = UI.FormatAsLink("Meatloaf", "MEATLOAF"); //ITEMS.FOOD.PRICKLEFRUIT.NAME;
		string description = "Meatloaf";
		GameObject template = EntityTemplates.CreateLooseEntity(ID, name, description, 1f, false, Assets.GetAnim("pepperbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, isPickupable: true);
		return EntityTemplates.ExtendEntityToFood(template, MEATLOAF);
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
		//inst.Subscribe(-10536414, OnEatCompleteDelegate);
	}

	private static void OnEatComplete(Edible edible)
	{
		if (edible != null)
		{
			int num = 0;
			float unitsConsumed = edible.unitsConsumed;
			int num2 = Mathf.FloorToInt(unitsConsumed);
			float num3 = unitsConsumed % 1f;
			if (Random.value < num3)
			{
				num2++;
			}
			for (int i = 0; i < num2; i++)
			{
				if (Random.value < SEEDS_PER_FRUIT_CHANCE)
				{
					num++;
				}
			}
			if (num > 0)
			{
				Vector3 pos = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
				pos = Grid.CellToPosCCC(Grid.PosToCell(pos), Grid.SceneLayer.Ore);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("PrickleFlowerSeed")), pos, Grid.SceneLayer.Ore);
				PrimaryElement component = edible.GetComponent<PrimaryElement>();
				PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
				component2.Temperature = component.Temperature;
				component2.Units = (float)num;
				gameObject.SetActive(value: true);
			}
		}
	}
}
