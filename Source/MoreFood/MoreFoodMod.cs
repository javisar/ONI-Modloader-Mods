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
		public static EdiblesManager.FoodInfo MEATLOAF;

		public static EdiblesManager.FoodInfo FUNGISALAD;

		private static void Postfix(Game __instance)
		{
			if (!MoreFoodState.StateManager.State.Enabled) return;

			Debug.Log(" === MoreFood_CookingStationConfig_ConfigureRecipes Postfix === ");

			Strings.Add("STRINGS.ITEMS.FOOD.MEATLOAF.NAME", "Meatloaf");
			Strings.Add("STRINGS.ITEMS.FOOD.MEATLOAF.DESC", "");
			Strings.Add("STRINGS.ITEMS.FOOD.MEATLOAF.RECIPEDESC", "");
			MEATLOAF = new EdiblesManager.FoodInfo("Meatloaf", 3000000f, 3, 255.15f, 277.15f, 5200f, can_rot: true);
			MeatloafConfig.recipe = CreateMeatloafRecipe();

			Strings.Add("STRINGS.ITEMS.FOOD.FUNGISALAD.NAME", "Fungi Salad");
			Strings.Add("STRINGS.ITEMS.FOOD.FUNGISALAD.DESC", "");
			Strings.Add("STRINGS.ITEMS.FOOD.FUNGISALAD.RECIPEDESC", "");
			FUNGISALAD = new EdiblesManager.FoodInfo("FungiSalad", 3000000f, 3, 255.15f, 277.15f, 5200f, can_rot: true);
			FungiSaladConfig.recipe = CreateFungiSaladRecipe();
			



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
				new ComplexRecipe.RecipeElement(FungiSaladConfig.ID, 1f)
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
