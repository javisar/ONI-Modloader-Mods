using JetBrains.Annotations;

using UnityEngine;

namespace MaterialColor.Extensions
{
	public static class SimHashesExtensions
    {
        public static bool ToMaterialColor(this SimHashes material, out Color color)
        {
            bool result = State.ElementColorInfos.TryGetValue(material, out color);

            color = ApplyColorCorrection(color);

            return result;
        }

        private static Color ApplyColorCorrection(Color color)
        {
            return color.ForEachColorValue
            (
                colorValue => colorValue * State.ConfiguratorState.IntensityFactor + State.ConfiguratorState.AdditionalBrightness,
                false
            );
        }
    }
}