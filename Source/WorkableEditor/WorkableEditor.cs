using System;
using Harmony;

namespace WorkableEditorMod
{

	[HarmonyPatch(typeof(Game), "OnPrefabInit")]
	internal class WorkableEditorMod_Game_OnPrefabInit
	{
		private static void Postfix(Game __instance)
		{
			//Debug.Log(" === WorkableEditorMod_Game_OnPrefabInit Postfix === ");
			if(!WorkableEditorConfig.Instance.Enabled) return;

			if (WorkableEditorConfig.Instance.Logging)
			{
				Debug.Log(" === WorkableEditorMod_Game_OnPrefabInit === 'Workable' Derived Types:");
				foreach (Type tp in WorkableEditorUtils.WorkableSubTypes)
				{
					Debug.Log(tp.FullName);
				}
			}
		}
	}

	[HarmonyPatch(typeof(Workable), "GetEfficiencyMultiplier", new[] { typeof(Worker) })]
    internal static class WorkableEditorMod_Workable_GetEfficiencyMultiplier
	{
        private static void Postfix(Workable __instance, Worker worker, ref float __result)
        {
			if (!WorkableEditorConfig.Instance.Enabled) return;
			//Debug.Log(" === WorkEditorMod_Constructable_GetEfficiencyMultiplier === " + __instance.GetType().ToString());

			__result = WorkableEditorUtils.GetMultiplier("EfficiencyMultiplier", __result, __instance);			
			
			/*
			
			*/

			/*
			foreach (var workable in WorkableEditorConfig.Config.Workables)
			{
				//Debug.Log("workable.Key: "+ workable.Key);
				Type type = AccessTools.TypeByName(workable.Key);
				//Debug.Log("type: " + type);
				if (WorkableEditorMod_Utils.WorkableSubTypes.Contains(type)) {
					//Debug.Log("__instance.GetType(): "+ __instance.GetType());
					if (__instance.GetType() == type)
					{
						Debug.Log("workable.Value[EfficiencyMultiplier]: " + workable.Value["EfficiencyMultiplier"]);
						__result = workable.Value["EfficiencyMultiplier"];
					}
				}
			}
			*/

			//Debug.Log("__instance.GetType(): " + __instance.GetType());

		}		
	}

	[HarmonyPatch(typeof(Workable), "GetAttributeExperienceMultiplier", new Type[] {  })]
	internal static class WorkableEditorMod_Workable_GetAttributeExperienceMultiplier
	{
		private static void Postfix(Workable __instance, ref float __result)
		{
			if (!WorkableEditorConfig.Instance.Enabled) return;
			//Debug.Log(" === WorkEditorMod_Constructable_GetAttributeExperienceMultiplier === " + __instance.GetType().ToString());

			__result = WorkableEditorUtils.GetMultiplier("AttributeExperienceMultiplier", __result, __instance);			
		}
	}

	[HarmonyPatch(typeof(Workable), "GetSkillExperienceMultiplier", new Type[] {  })]
	internal static class WorkableEditorMod_Workable_GetSkillExperienceMultiplier
	{
		private static void Postfix(Workable __instance, ref float __result)
		{
			if (!WorkableEditorConfig.Instance.Enabled) return;
			//Debug.Log(" === WorkEditorMod_Constructable_GetSkillExperienceMultiplier === " + __instance.GetType().ToString());

			__result = WorkableEditorUtils.GetMultiplier("SkillExperienceMultiplier", __result, __instance);
		}
	}


	/*
    [HarmonyPatch(typeof(Database.AttributeConverters), "Create", new Type[] { typeof(string), typeof(string), typeof(string), typeof(Klei.AI.Attribute), typeof(float), typeof(float), typeof(IAttributeFormatter) })]
    internal class InstantDigAndBuildMod
    {
        private static bool Prefix(Database.AttributeConverters __instance, string id, string name, string description, Klei.AI.Attribute attribute, ref float multiplier, float base_value, IAttributeFormatter formatter)
        {
            //Debug.Log(" === GetEfficiencyMultiplier InstantDigAndBuildMod loaded === " + id + " "+multiplier);
            if (id.Equals("ConstructionSpeed") || id.Equals("DiggingSpeed"))
            {
                multiplier = 100.0f;
            }
            return true;
        }
    }
    */
}