using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreCanvas
{
	class BuildSymbolData
	{
		public KAnimHashedString hash;
		//public int frame;
		public KAnimHashedString path;
		public KAnimHashedString colourChannel;
		public int flags;
		public int firstFrameIdx;
		public int numFrames;
		public int symbolIndexInSourceBuild;
		public int numLookupFrames;
		public List<BuildSymbolFrameData> symbolFrameData = new List<BuildSymbolFrameData>();
		/*
		public float a;
		public float b;
		public float g;
		public float r;
		public float m;
		public float m2;
		public float m3;
		public float m4;
		public float m5;
		public float m6;
		public float float1;
		*/
	}

	class BuildSymbolFrameData
	{
		public KAnimHashedString fileNameHash;
		public int sourceFrameNum;
		public int duration;
		public int buildImageIdx;
		public float num6;
		public float num7;
		public float num8;
		public float num9;

		public float x;
		public float num10;
		public float x2;
		public float num11;
		/*
		public float num3;
		public float num4;
		public float num5;
		public float num6;
		public int numElements;
		public List<BuildSymbolData> symbolData = new List<BuildSymbolData>();
		*/
	}
	/*
	class BuildElementData
	{
		public string kleistring;
		public int hashvalue;
		public float frameRate;
		public int numFrames;
		public List<BuildFrameData> frameData = new List<BuildFrameData>();
	}
	*/
	class BuildHashData
	{
		public int hash;
		public string text;
	}

	class BuildData
	{
		public char[] header;
		public int version;
		public int numSymbols;
		public int numSymbolFrames;
		public string name;
		public int count;
		public List<BuildSymbolData> symbolData = new List<BuildSymbolData>();
		public int maxVisSymbolFrames;
		public int numHashData;
		public List<BuildHashData> hashData = new List<BuildHashData>();
	}
}
