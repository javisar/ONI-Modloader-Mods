using JetBrains.Annotations;

using UnityEngine;

namespace MaterialColor.Extensions
{
	public static class SimHashesExtensions
    {
        public static bool ToMaterialColor(this SimHashes material, out Color result)
        {
            return State.ElementColorInfos.TryGetValue(material, out result);
        }
    }
}