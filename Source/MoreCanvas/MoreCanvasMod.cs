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

			AnimData animData = new AnimData();

			animData.header = reader.ReadChars("ANIM".Length);
			//Debug.Log("header: "+header);
			animData.version = reader.ReadUInt32();
			//Debug.Log("version: " + version);
			animData.int1 = reader.ReadInt32();
			animData.int2 = reader.ReadInt32();
			animData.count = reader.ReadInt32();
			//Debug.Log("num2: " + num2);
			for (int i = 0; i < animData.count; i++)
			{
				AnimElementData animElementData = new AnimElementData();

				animElementData.kleistring = reader.ReadKleiString();
				//Debug.Log("\tkleistring: " + kleistring);
				animElementData.hashvalue = reader.ReadInt32();
				//Debug.Log("\thashvalue: " + hashvalue);
				animElementData.frameRate = reader.ReadSingle();
				//Debug.Log("\tframeRate: " + frameRate);
				animElementData.numFrames = reader.ReadInt32();
				//Debug.Log("\tnumFrames: " + numFrames);
				for (int j = 0; j < animElementData.numFrames; j++)
				{
					AnimFrameData animFrameData = new AnimFrameData();

					animFrameData.num3 = reader.ReadSingle();
					animFrameData.num4 = reader.ReadSingle();
					animFrameData.num5 = reader.ReadSingle();
					animFrameData.num6 = reader.ReadSingle();

					//Debug.Log("\t\tnum3: " + num3);
					//Debug.Log("\t\tnum4: " + num4);
					//Debug.Log("\t\tnum5: " + num5);
					//Debug.Log("\t\tnum6: " + num6);

					animFrameData.numElements = reader.ReadInt32();
					//Debug.Log("\t\tnumElements: " + numElements);
					for (int k = 0; k < animFrameData.numElements; k++)
					{
						AnimSymbolData animSymbolData = new AnimSymbolData();
						animSymbolData.symbol = new KAnimHashedString(reader.ReadInt32());
						//Debug.Log("\t\t\tsymbol: " + symbol);
						animSymbolData.frame = reader.ReadInt32();
						//Debug.Log("\t\t\tframe: " + frame);
						animSymbolData.folder = new KAnimHashedString(reader.ReadInt32());
						//Debug.Log("\t\t\tfolder: " + folder);
						animSymbolData.flags = reader.ReadInt32();
						//Debug.Log("\t\t\tflags: " + flags);

						animSymbolData.a = reader.ReadSingle();
						animSymbolData.b = reader.ReadSingle();
						animSymbolData.g = reader.ReadSingle();
						animSymbolData.r = reader.ReadSingle();

						//Debug.Log("\t\t\ta: " + a);
						//Debug.Log("\t\t\tb: " + b);
						//Debug.Log("\t\t\tg: " + g);
						//Debug.Log("\t\t\tr: " + r);

						Color multColour = new Color(animSymbolData.r, animSymbolData.g, animSymbolData.b, animSymbolData.a);
						animSymbolData.m = reader.ReadSingle();
						animSymbolData.m2 = reader.ReadSingle();
						animSymbolData.m3 = reader.ReadSingle();
						animSymbolData.m4 = reader.ReadSingle();
						animSymbolData.m5 = reader.ReadSingle();
						animSymbolData.m6 = reader.ReadSingle();

						//Debug.Log("\t\t\tm: " + m);
						//Debug.Log("\t\t\tm2: " + m2);
						//Debug.Log("\t\t\tm3: " + m3);
						//Debug.Log("\t\t\tm4: " + m4);
						//Debug.Log("\t\t\tm5: " + m5);
						//Debug.Log("\t\t\tm6: " + m6);

						animSymbolData.float1 = reader.ReadSingle();

						animFrameData.symbolData.Add(animSymbolData);
					}
					animElementData.frameData.Add(animFrameData);
				}
				animData.elementsData.Add(animElementData);
			}
			animData.maxVisSymbolFrames = reader.ReadInt32();
			//Debug.Log("maxVisSymbolFrames: " + maxVisSymbolFrames);
			animData.numHashData = reader.ReadInt32();
			//Debug.Log("num: " + num);
			for (int i = 0; i < animData.numHashData; i++)
			{
				AnimHashData animHashData = new AnimHashData();

				animHashData.hash = reader.ReadInt32();
				//Debug.Log("hash: " + hash);
				animHashData.text = reader.ReadKleiString();
				//Debug.Log("text: " + text);

				animData.hashData.Add(animHashData);
			}


			// Write file
			MoreCanvasState.StateManager.JsonLoader.TrySaveConfiguration("test.json", animData);
		}
	}
}
