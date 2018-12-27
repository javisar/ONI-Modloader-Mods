// FriedMushroomConfig
using STRINGS;
using TUNING;
using UnityEngine;

public class MeatloafConfig : IEntityConfig
{
    public const string ID = "Meatloaf";

    //public static readonly EdiblesManager.FoodInfo MEATLOAF 
    //    = new EdiblesManager.FoodInfo("Meatloaf", 2800000f, 1, 255.15f, 277.15f, 2400f, can_rot: true);

    public static ComplexRecipe recipe;

    public GameObject CreatePrefab()
    {
        string name = UI.FormatAsLink("Meatloaf", "MEATLOAF");
        string description = "";

        GameObject template = EntityTemplates.CreateLooseEntity("Meatloaf", name, description, 1f, false, Assets.GetAnim("creaturemeat_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, isPickupable: true);
        return EntityTemplates.ExtendEntityToFood(template, FOOD.FOOD_TYPES.COOKEDMEAT);
    }

    public void OnPrefabInit(GameObject inst)
    {
    }

    public void OnSpawn(GameObject inst)
    {
    }
}
