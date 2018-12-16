namespace MaterialColor.Extensions
{
    using JetBrains.Annotations;

    using Helpers;

    using UnityEngine;
    using MaterialColor.Data;
	using ONI_Common.Data;

	public static class SimHashesExtensions
    {
        public static bool ToMaterialColor(this SimHashes material, out Color result)
        {
            return State.ElementColorInfos.TryGetValue(material, out result);
        }

        public static Color32 ToDebugColor(this SimHashes material)
        {
            Element element = ElementLoader.FindElementByHash(material);

            if (element?.substance != null)
            {
                Color32 debugColor = element.substance.debugColour;

                debugColor.a = byte.MaxValue;

                return debugColor;
            }

            return Color.white;
        }
    }
}