using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Harmony;

namespace SuperMinerMod
{
	[HarmonyPatch(typeof(WorldDamage), "OnDigComplete")]
	internal class SuperMinerMod_WorldDamage_OnDigComplete
	{

		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
		{
			Debug.Log(" === SuperMinerMod_WorldDamage_OnDigComplete Transpiler === ");

			List<CodeInstruction> code = instr.ToList();
			foreach (CodeInstruction codeInstruction in code)
			{
				if (codeInstruction.opcode == OpCodes.Ldc_R4
					&& (float)codeInstruction.operand == (float)0.5)
				{
					Debug.Log(" === Transpiler applied === ");
					codeInstruction.operand = 1.0f;
				}
				yield return codeInstruction;
			}
		}


		/*
		public static float MASS_DIG_RATIO = 1.0f;


		private static void Prefix(WorldDamage __instance, int cell, ref float mass, float temperature, byte element_idx, byte disease_idx, int disease_count)
		{
			mass = mass * MASS_DIG_RATIO;
		}

		[HarmonyPatch(typeof(Diggable), "AwardExperience")]
		internal class SuperMinerMod_Diggable_AwardExperience
		{
			private static void Postfix(Diggable __instance, float work_dt, MinionResume resume)
			{
				//Debug.Log(" === SuperMinerMod_Diggable_AwardExperience Postfix === " + resume.GetIdentity.GetProperName());

				if (resume.ExperienceByRoleID[SeniorMiner.ID] > 0f)
				{
					//Debug.Log(" === MASS_DIG_RATIO = 1.0f === ");
					SuperMinerMod_WorldDamage_OnDigComplete.MASS_DIG_RATIO = 2.0f;
				}
				else
				{
					//Debug.Log(" === MASS_DIG_RATIO = 0.5f === ");
					SuperMinerMod_WorldDamage_OnDigComplete.MASS_DIG_RATIO = 1.0f;
				}
			}
		}
		*/

	}

	/*
	[HarmonyPatch(typeof(SeniorMiner))]
	internal class SuperMinerMod_SeniorMiner_ctor
	{
		private static void Postfix(SeniorMiner __instance)
		{
			Debug.Log(" === SuperMinerMod_SeniorMiner_ctor Postfix === ");

			List<RolePerk> ls = new List<RolePerk>((RolePerk[])__instance.perks);
			ls.Add(new RoleAttributePerk("IncreaseDigMass", "Digs all mass", Db.Get().Attributes.Digging.Id, (float)TUNING.ROLES.ATTRIBUTE_BONUS_FIRST, STRINGS.DUPLICANTS.ROLES.SENIOR_MINER.NAME));
			PropertyInfo pi = AccessTools.Property(typeof(RoleConfig), "perks");
			pi.SetValue(__instance,(RolePerk[])ls.ToArray(),null);

		}
	}
	*/

}
