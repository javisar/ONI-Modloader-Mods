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

			Debug.Log(" === MoreCanvas_KGlobalAnimParser_ParseAnimData Postfix === " + animFile.name+" "+ fileNameHash.HashValue);

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
			MoreCanvasState.StateManager.JsonLoader.TrySaveConfiguration("testAnim.json", animData);
		}
	}


	[HarmonyPatch(typeof(KGlobalAnimParser), "ParseBuildData")]
	internal class MoreCanvas_KGlobalAnimParser_ParseBuildData
	{

		private static void Postfix(KGlobalAnimParser __instance, KBatchGroupData data, KAnimHashedString fileNameHash, FastReader reader, List<Texture2D> textures)
		{
			if (!MoreCanvasState.StateManager.State.Enabled) return;

			Debug.Log(" === MoreCanvas_KGlobalAnimParser_ParseBuildData Postfix === " + fileNameHash.HashValue);
			
			if (fileNameHash.HashValue != -1320822911) return;
			//artable.stages.Add(new Artable.Stage("Good100", STRINGS.BUILDINGS.PREFABS.CANVAS.EXCELLENTQUALITYNAME, "art_e", 15, cheer_on_complete: true, Artable.Status.Great));
			reader.Position = 0;

			BuildData buildData = new BuildData();

			buildData.header = reader.ReadChars("BILD".Length);
			buildData.version = reader.ReadInt32();
			if (buildData.version != 10 && buildData.version != 9)
			{
				Debug.LogError(fileNameHash + " has invalid build.bytes version [" + buildData.version + "]");
				return;
			}
			
			KAnimGroupFile.Group group = KAnimGroupFile.GetGroup(data.groupID);
			if (group == null)
			{
				Debug.LogErrorFormat("[{1}] Failed to get group [{0}]", data.groupID, fileNameHash.DebuggerDisplay);
				return;
			}
			//KAnim.Build build = null;
			buildData.numSymbols = reader.ReadInt32();
			buildData.numSymbolFrames = reader.ReadInt32();
			buildData.name = reader.ReadKleiString();

			Debug.Log("1");
			/*
			build = data.AddNewBuildFile(fileNameHash);
			build.textureCount = textures.Count;
			if (textures.Count > 0)
			{
				data.AddTextures(textures);
			}
			build.symbols = new KAnim.Build.Symbol[buildData.numSymbols];
			build.frames = new KAnim.Build.SymbolFrame[buildData.numSymbolFrames];
			build.name = reader.ReadKleiString();
			build.batchTag = ((!group.swapTarget.IsValid) ? data.groupID : group.target);
			build.fileHash = fileNameHash;
			*/
			int num4 = 0;
			for (int i = 0; i < buildData.numSymbols; i++)
			{
				Debug.Log("2");
				BuildSymbolData symbolData = new BuildSymbolData();
				symbolData.hash = new KAnimHashedString(reader.ReadInt32());
				
				//KAnim.Build.Symbol symbol = new KAnim.Build.Symbol();
				//symbol.build = build;
				//symbol.hash = hash;
				if (buildData.version > 9)
				{
					symbolData.path = new KAnimHashedString(reader.ReadInt32());
				}
				symbolData.colourChannel = new KAnimHashedString(reader.ReadInt32());
				symbolData.flags = reader.ReadInt32();
				//symbolData.firstFrameIdx = data.symbolFrameInstances.Count;
				symbolData.firstFrameIdx = num4;
				symbolData.numFrames = reader.ReadInt32();
				symbolData.symbolIndexInSourceBuild = i;
				Debug.Log("3");
				int num5 = 0;
				for (int j = 0; j < symbolData.numFrames; j++)
				{
					Debug.Log("4");
					BuildSymbolFrameData buildSymbolFrameData = new BuildSymbolFrameData();
					Debug.Log("4a");
					//KAnim.Build.SymbolFrame symbolFrame = new KAnim.Build.SymbolFrame();
					//KAnim.Build.SymbolFrameInstance item = default(KAnim.Build.SymbolFrameInstance);
					//item.symbolFrame = symbolFrame;
					buildSymbolFrameData.fileNameHash = fileNameHash;
					Debug.Log("4b");
					buildSymbolFrameData.sourceFrameNum = reader.ReadInt32();
					Debug.Log("4c");
					buildSymbolFrameData.duration = reader.ReadInt32();
					Debug.Log("4d");
					buildSymbolFrameData.buildImageIdx = reader.ReadInt32();
					Debug.Log("4e");
					/*
					if (item.buildImageIdx >= textures.Count + data.textureStartIndex[fileNameHash])
					{
						Debug.LogErrorFormat("{0} Symbol: [{1}] tex count: [{2}] buildImageIdx: [{3}] group total [{4}]", fileNameHash.ToString(), symbol.hash, textures.Count, item.buildImageIdx, data.textureStartIndex[fileNameHash]);
					}
					*/
					//item.symbolIdx = data.GetSymbolCount();
					num5 = Math.Max(buildSymbolFrameData.sourceFrameNum + buildSymbolFrameData.duration, num5);
					Debug.Log("5");
					buildSymbolFrameData.num6 = reader.ReadSingle();
					buildSymbolFrameData.num7 = reader.ReadSingle();
					buildSymbolFrameData.num8 = reader.ReadSingle();
					buildSymbolFrameData.num9 = reader.ReadSingle();
					//symbolFrame.bboxMin = new Vector2(num6 - num8 * 0.5f, num7 - num9 * 0.5f);
					//symbolFrame.bboxMax = new Vector2(num6 + num8 * 0.5f, num7 + num9 * 0.5f);
					buildSymbolFrameData.x = reader.ReadSingle();
					buildSymbolFrameData.num10 = reader.ReadSingle();
					buildSymbolFrameData.x2 = reader.ReadSingle();
					buildSymbolFrameData. num11 = reader.ReadSingle();
					//symbolFrame.uvMin = new Vector2(x, 1f - num10);
					//symbolFrame.uvMax = new Vector2(x2, 1f - num11);
					//build.frames[num4] = symbolFrame;
					//data.symbolFrameInstances.Add(item);
					num4++;

					symbolData.symbolFrameData.Add(buildSymbolFrameData);
				}
				Debug.Log("6");
				symbolData.numLookupFrames = num5;
				//data.AddBuildSymbol(symbol);
				//build.symbols[i] = symbol;

				buildData.symbolData.Add(symbolData);
			}
			Debug.Log("7");
			//ParseHashTable(reader);
			buildData.numHashData = reader.ReadInt32();
			//Debug.Log("num: " + num);
			for (int i = 0; i < buildData.numHashData; i++)
			{
				BuildHashData buildHashData = new BuildHashData();

				buildHashData.hash = reader.ReadInt32();
				buildHashData.text = reader.ReadKleiString();			

				buildData.hashData.Add(buildHashData);
			}

			// Write file
			MoreCanvasState.StateManager.JsonLoader.TrySaveConfiguration("testBuild.json", buildData);
			
		}
	}
}
