using System;
using Harmony;

namespace WorkableMultipliersMod
{

	[HarmonyPatch(typeof(Workable), "GetEfficiencyMultiplier", new[] { typeof(Worker) })]
    internal static class WorkableMultipliersMod_Workable_GetEfficiencyMultiplier
	{
        private static void Postfix(Workable __instance, Worker worker, ref float __result)
        {
			if (!WorkableMultipliersConfig.Instance.Enabled) return;
			//Debug.Log(" === WorkEditorMod_Constructable_GetEfficiencyMultiplier === " + __instance.GetType().ToString());

			__result = WorkableMultipliersUtils.GetMultiplier("EfficiencyMultiplier", __result, __instance);			

		}		
	}

	[HarmonyPatch(typeof(Workable), "GetAttributeExperienceMultiplier", new Type[] {  })]
	internal static class WorkableMultipliersMod_Workable_GetAttributeExperienceMultiplier
	{
		private static void Postfix(Workable __instance, ref float __result)
		{
			if (!WorkableMultipliersConfig.Instance.Enabled) return;
			//Debug.Log(" === WorkEditorMod_Constructable_GetAttributeExperienceMultiplier === " + __instance.GetType().ToString());

			__result = WorkableMultipliersUtils.GetMultiplier("AttributeExperienceMultiplier", __result, __instance);			
		}
	}

	[HarmonyPatch(typeof(Workable), "GetSkillExperienceMultiplier", new Type[] {  })]
	internal static class WorkableMultipliersMod_Workable_GetSkillExperienceMultiplier
	{
		private static void Postfix(Workable __instance, ref float __result)
		{
			if (!WorkableMultipliersConfig.Instance.Enabled) return;
			//Debug.Log(" === WorkEditorMod_Constructable_GetSkillExperienceMultiplier === " + __instance.GetType().ToString());
            
            __result = WorkableMultipliersUtils.GetMultiplier("SkillExperienceMultiplier", __result, __instance);
    
        }
	}


    [HarmonyPatch(typeof(Game), "OnPrefabInit")]
    internal class WorkableMultipliersMod_Game_OnPrefabInit
    {
        private static void Postfix(Game __instance)
        {
            //Debug.Log(" === WorkableMultipliersMod_Game_OnPrefabInit Postfix === ");
            if (!WorkableMultipliersConfig.Instance.Enabled) return;

            if (WorkableMultipliersConfig.Instance.Logging)
            {
                Debug.Log(" === WorkableMultipliersMod_Game_OnPrefabInit === 'Workable' Derived Types:");
                foreach (Type tp in WorkableMultipliersUtils.WorkableSubTypes)
                {
                    Debug.Log(tp.FullName);
                }
            }
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

    /*
    [HarmonyPatch(typeof(MinionResume), "GetAptitudeExperienceMultiplier", new Type[] {typeof(HashedString),typeof(float) })]
    internal static class MinionResumeMod_GetAptitudeExperienceMultiplier_GetSkillExperienceMultiplier
    {
        private static void Postfix(MinionResume __instance, ref float __result, HashedString skillGroupId, float buildingFrequencyMultiplier)
        {
            Debug.Log(" === WorkEditorMod_Constructable_GetSkillExperienceMultiplier === " + __instance.GetType().ToString());
            Debug.Log(" __result = " + __result);
            Debug.Log(" buildingFrequencyMultiplier = " + buildingFrequencyMultiplier);


            float value = 0f;
            __instance.AptitudeBySkillGroup.TryGetValue(skillGroupId, out value);
            float total = 1f + value * TUNING.SKILLS.APTITUDE_EXPERIENCE_MULTIPLIER * buildingFrequencyMultiplier;
            Debug.Log(" value = " + value);
            Debug.Log(" TUNING.SKILLS.APTITUDE_EXPERIENCE_MULTIPLIER = " + TUNING.SKILLS.APTITUDE_EXPERIENCE_MULTIPLIER);
            Debug.Log(" buildingFrequencyMultiplier = " + value);
            Debug.Log(" total = " + total);
        }
    }
    */


}