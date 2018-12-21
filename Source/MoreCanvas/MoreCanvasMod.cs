using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace MoreCanvas
{
	[HarmonyPatch(typeof(Game), "OnPrefabInit")]
	internal class MoreCanvas_Game_OnPrefabInit
	{

		private static void Postfix(Game __instance)
		{
			if (!MoreCanvasState.StateManager.State.Enabled) return;

			Debug.Log(" === MoreCanvas_Game_OnPrefabInit Postfix === ");

		}
	}

	[HarmonyPatch(typeof(CanvasConfig), "DoPostConfigureComplete")]
	internal class MoreCanvas_CanvasConfig_DoPostConfigureComplete
	{

		private static void Postfix(CanvasConfig __instance, GameObject go)
		{
			if (!MoreCanvasState.StateManager.State.Enabled) return;

			Debug.Log(" === MoreCanvas_CanvasConfig_DoPostConfigureComplete Postfix === ");
			Artable artable = go.AddComponent<Painting>();

			//artable.stages.Add(new Artable.Stage("Good100", STRINGS.BUILDINGS.PREFABS.CANVAS.EXCELLENTQUALITYNAME, "art_e", 15, cheer_on_complete: true, Artable.Status.Great));
		}
	}

	[HarmonyPatch(typeof(KGlobalAnimParser), "ParseAnimData")]
	internal class MoreCanvas_KGlobalAnimParser_ParseAnimData
	{

		private static void Postfix(KGlobalAnimParser __instance, KBatchGroupData data, HashedString fileNameHash, FastReader reader, KAnimFileData animFile)
		{
			if (!MoreCanvasState.StateManager.State.Enabled) return;

			Debug.Log(" === MoreCanvas_KGlobalAnimParser_ParseAnimData Postfix === " + animFile.name);

			if (!animFile.name.Equals("painting_kanim")) return;
			//artable.stages.Add(new Artable.Stage("Good100", STRINGS.BUILDINGS.PREFABS.CANVAS.EXCELLENTQUALITYNAME, "art_e", 15, cheer_on_complete: true, Artable.Status.Great));
			reader.Position = 0;

			char[] header = reader.ReadChars("ANIM".Length);
			Debug.Log("header: "+header);
			uint version = reader.ReadUInt32();
			Debug.Log("version: " + version);
			reader.ReadInt32();
			reader.ReadInt32();
			int num2 = reader.ReadInt32();
			Debug.Log("num2: " + num2);
			for (int i = 0; i < num2; i++)
			{
				String kleistring = reader.ReadKleiString();
				Debug.Log("\tkleistring: " + kleistring);
				int hashvalue = reader.ReadInt32();
				Debug.Log("\thashvalue: " + hashvalue);
				float frameRate = reader.ReadSingle();
				Debug.Log("\tframeRate: " + frameRate);
				int numFrames = reader.ReadInt32();
				Debug.Log("\tnumFrames: " + numFrames);
				for (int j = 0; j < numFrames; j++)
				{
					float num3 = reader.ReadSingle();
					float num4 = reader.ReadSingle();
					float num5 = reader.ReadSingle();
					float num6 = reader.ReadSingle();

					Debug.Log("\t\tnum3: " + num3);
					Debug.Log("\t\tnum4: " + num4);
					Debug.Log("\t\tnum5: " + num5);
					Debug.Log("\t\tnum6: " + num6);

					int numElements = reader.ReadInt32();
					Debug.Log("\t\tnumElements: " + numElements);
					for (int k = 0; k < numElements; k++)
					{
						KAnimHashedString symbol = new KAnimHashedString(reader.ReadInt32());
						Debug.Log("\t\t\tsymbol: " + symbol);
						int frame = reader.ReadInt32();
						Debug.Log("\t\t\tframe: " + frame);
						KAnimHashedString folder = new KAnimHashedString(reader.ReadInt32());
						Debug.Log("\t\t\tfolder: " + folder);
						int flags = reader.ReadInt32();
						Debug.Log("\t\t\tflags: " + flags);
						float a = reader.ReadSingle();
						float b = reader.ReadSingle();
						float g = reader.ReadSingle();
						float r = reader.ReadSingle();

						Debug.Log("\t\t\ta: " + a);
						Debug.Log("\t\t\tb: " + b);
						Debug.Log("\t\t\tg: " + g);
						Debug.Log("\t\t\tr: " + r);

						Color multColour = new Color(r, g, b, a);
						float m = reader.ReadSingle();
						float m2 = reader.ReadSingle();
						float m3 = reader.ReadSingle();
						float m4 = reader.ReadSingle();
						float m5 = reader.ReadSingle();
						float m6 = reader.ReadSingle();

						Debug.Log("\t\t\tm: " + m);
						Debug.Log("\t\t\tm2: " + m2);
						Debug.Log("\t\t\tm3: " + m3);
						Debug.Log("\t\t\tm4: " + m4);
						Debug.Log("\t\t\tm5: " + m5);
						Debug.Log("\t\t\tm6: " + m6);

						reader.ReadSingle();
					}
				}
			}
			int maxVisSymbolFrames = reader.ReadInt32();
			Debug.Log("maxVisSymbolFrames: " + maxVisSymbolFrames);
			int num = reader.ReadInt32();
			Debug.Log("num: " + num);
			for (int i = 0; i < num; i++)
			{
				int hash = reader.ReadInt32();
				Debug.Log("hash: " + hash);
				string text = reader.ReadKleiString();
				Debug.Log("text: " + text);
			}
		}
	}
}
