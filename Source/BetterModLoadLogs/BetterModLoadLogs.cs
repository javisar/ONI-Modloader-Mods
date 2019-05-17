using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Harmony;

namespace BetterModLoadLogs
{
	[HarmonyPatch(typeof(KMod.Manager), MethodType.Constructor)]
	internal class BetterModLoadLogs_KMod_Manager_Constructor
	{
		private static void Postfix(KMod.Manager __instance)
		{
			Debug.Log(" === BetterModLoadLogs_Game_OnPrefabInit Postfix === ");
			Assembly design = System.Reflection.Assembly.GetAssembly(typeof(KMod.Mod));
			Type designHost = design.GetType("KMod.DLLLoader");
			var original = designHost.GetMethod("LoadDLLs");
			var transpiler = typeof(BetterModLoadLogs_KMod_Manager_Constructor).GetMethod("Transpiler");
			HarmonyInstance.Create("BetterModLoadLogs_KMod_Manager_Constructor").Patch(original, null, null, new HarmonyMethod(transpiler));

		}

		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
		{
			//Debug.Log(" === BetterModLoadLogs_Game_OnPrefabInit Transpiler === ");
			List<CodeInstruction> instructions = new List<CodeInstruction>(instr);
			
			int idxAnchor = instructions.LastIndexOf(new CodeInstruction(OpCodes.Pop));
			if (idxAnchor == -1)
			{
				Debug.Log("Could not find Page_ModsConfig.DoWindowContents transpiler anchor - not injecting code");
				//return instructions;
			}


			var index = -1;
			List<CodeInstruction> code = instr.ToList();
			foreach (CodeInstruction codeInstruction in code)
			{
				index++;

				if (idxAnchor >= 0
					&& codeInstruction.opcode == OpCodes.Pop
					&& index == idxAnchor)
				{
					Debug.Log(" === Transpiler applied === ");
					codeInstruction.opcode = OpCodes.Call;
					codeInstruction.operand = AccessTools.Method(typeof(Debug), nameof(Debug.LogException));					
				}
				else
				{
					yield return codeInstruction;
				}
			}			

		}
	}

	/*
	[HarmonyPatch(Assembly.LoadFile("").GetType("System.ComponentModel.Design.DesignerHost"), "LoadDLL")]
	internal class SuperMinerMod_WorldDamage_OnDigComplete
	{

		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
		{
			//Debug.Log(" === SuperMinerMod_WorldDamage_OnDigComplete Transpiler === ");

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

	}
	*/
}
