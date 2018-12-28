using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace MoreFood
{

	[HarmonyPatch(typeof(CookingStationConfig), "ConfigureRecipes")]
	internal class MoreFood_CookingStationConfig_ConfigureRecipes
	{

		private static void Postfix(Game __instance)
		{
			if (!MoreFoodState.StateManager.State.Enabled) return;

			Debug.Log(" === MoreFood_CookingStationConfig_ConfigureRecipes Postfix === ");

			MeatloafConfig.recipe = CreateMeatloafRecipe();
			Strings.Add("STRINGS.BUILDINGS.PREFABS.MEATLOAF.NAME", "Meatloaf");

			FungiSaladConfig.recipe = CreateFungiSaladRecipe();
			Strings.Add("STRINGS.BUILDINGS.PREFABS.FUNGISALAD.NAME", "Fungi Salad");



		}

		private static ComplexRecipe CreateMeatloafRecipe()
		{

			ComplexRecipe.RecipeElement[] inputs = new ComplexRecipe.RecipeElement[2]
			{
				new ComplexRecipe.RecipeElement(BasicPlantFoodConfig.ID, 2f),
				new ComplexRecipe.RecipeElement(SpiceNutConfig.ID, 1f)
			};
			ComplexRecipe.RecipeElement[] outputs = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement(MeatloafConfig.ID, 1f)
			};
			string id = ComplexRecipeManager.MakeRecipeID("CookingStation", inputs, outputs);

			ComplexRecipe complexRecipe = new ComplexRecipe(id, inputs, outputs);
			complexRecipe.time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME;
			//complexRecipe.description = STRINGS.ITEMS.FOOD.SPICEBREAD.RECIPEDESC;
			complexRecipe.description = "A dish of ground meat mixed with other ingredients and formed into a loaf shape, then baked or smoked.";
			complexRecipe.useResultAsDescription = true;
			complexRecipe.fabricators = new List<Tag>
			{
				"CookingStation"
			};
			complexRecipe.sortOrder = 201;
			return complexRecipe;
		}

		private static ComplexRecipe CreateFungiSaladRecipe()
		{

			ComplexRecipe.RecipeElement[] inputs = new ComplexRecipe.RecipeElement[2]
			{
				new ComplexRecipe.RecipeElement(BasicPlantFoodConfig.ID, 2f),
				new ComplexRecipe.RecipeElement(MushroomConfig.ID, 1f)
			};
			ComplexRecipe.RecipeElement[] outputs = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement(FungiSaladConfig.FUNGISALAD.Id, 1f)
			};
			string id = ComplexRecipeManager.MakeRecipeID("CookingStation", inputs, outputs);

			ComplexRecipe complexRecipe = new ComplexRecipe(id, inputs, outputs);
			complexRecipe.time = TUNING.FOOD.RECIPES.SMALL_COOK_TIME;
			//complexRecipe.description = STRINGS.ITEMS.FOOD.SPICEBREAD.RECIPEDESC;
			complexRecipe.description = "Fungi salad.";
			complexRecipe.useResultAsDescription = true;
			complexRecipe.fabricators = new List<Tag>
			{
				"CookingStation"
			};
			complexRecipe.sortOrder = 200;
			return complexRecipe;
		}

	}

	/*
	[HarmonyPatch(typeof(Game), "OnPrefabInit")]
	internal class MoreFood_Game_OnPrefabInit
	{

		private static void Postfix(Game __instance)
		{
			if (!MoreFoodState.StateManager.State.Enabled) return;

			Debug.Log(" === MoreFood_Game_OnPrefabInit Postfix === ");


		}
	}
	*/
}
