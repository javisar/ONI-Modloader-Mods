namespace MaterialColor.Extensions
{
    using JetBrains.Annotations;

    using Helpers;

    using UnityEngine;
    using MaterialColor.Data;
	using ONI_Common.Data;

	public static class SimHashesExtensions
    {
        public static Color GetMaterialColorForType(this SimHashes material, string objectTypeName)
        {
            ElementColorInfo elementColorInfo = material.GetMaterialColorInfo();

            // TODO extract method (extension?)
            return new Color
            (
                elementColorInfo.ColorMultiplier.Red,
                elementColorInfo.ColorMultiplier.Green,
                elementColorInfo.ColorMultiplier.Blue
            );
        }

        [NotNull]
        public static ElementColorInfo GetMaterialColorInfo(this SimHashes materialHash)
        {
			ElementColorInfo elementColorInfo;
			if (State.ElementColorInfos.TryGetValue(materialHash, out elementColorInfo))
			{
                return elementColorInfo;
            }

            if (!State.ConfiguratorState.ShowMissingElementColorInfos)
            {
                return new ElementColorInfo(Color32Multiplier.One);
            }

            Debug.LogError($"Can't find <{materialHash}> color info");
            return new ElementColorInfo(new Color32Multiplier(1, 0, 1));
        }

        public static Color ToCellMaterialColor(this SimHashes material)
        {
            ElementColorInfo colorInfo = material.GetMaterialColorInfo();

            Color result = new Color(
                                     colorInfo.ColorMultiplier.Red,
                                     colorInfo.ColorMultiplier.Green,
                                     colorInfo.ColorMultiplier.Blue) * colorInfo.Brightness;

            result.a = byte.MaxValue;

            return result;
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