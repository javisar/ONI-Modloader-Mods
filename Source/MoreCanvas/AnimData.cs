using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreCanvas
{
	class AnimSymbolData
	{
		public KAnimHashedString symbol;
		public int frame;
		public KAnimHashedString folder;
		public int flags;
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
	}

	class AnimFrameData
	{
		public float num3;
		public float num4;
		public float num5;
		public float num6;
		public int numElements;
		public List<AnimSymbolData> symbolData = new List<AnimSymbolData>();
	}

	class AnimElementData
	{
		public string kleistring;
		public int hashvalue;
		public float frameRate;
		public int numFrames;
		public List<AnimFrameData> frameData = new List<AnimFrameData>();
	}

	class AnimHashData
	{
		public int hash;
		public string text;
	}

	class AnimData
	{
		public char[] header;
		public uint version;
		public int int1;
		public int int2;
		public int count;
		public List<AnimElementData> elementsData = new List<AnimElementData>();
		public int maxVisSymbolFrames;
		public int numHashData;
		public List<AnimHashData> hashData = new List<AnimHashData>();
	}
}
