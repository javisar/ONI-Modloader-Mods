namespace MaterialColor.Extensions
{
    using JetBrains.Annotations;

    using Helpers;

    using UnityEngine;
    using MaterialColor.Data;
	using ONI_Common.Data;

	public static class SimHashesExtensions
    {
        public static Color ToMaterialColor(this SimHashes material)
        {
			Color color;
			if (State.ElementColorInfos.TryGetValue(material, out color))
			{
                return color;
            }

            if (!State.ConfiguratorState.ShowMissingElementColorInfos)
            {
                return Color.white;
            }

            Debug.LogError($"Can't find <{material}> color info");
            return new Color(4, 0, 4);
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